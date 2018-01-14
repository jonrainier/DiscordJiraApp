// Copyright (c) 2018 Initial Servers LLC. All rights reserved.
// https://initialservers.com/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiscordJiraApp.Enums;
using DiscordJiraApp.Helpers;
using DiscordJiraApp.Models;
using Newtonsoft.Json;

namespace DiscordJiraApp
{
    public class Application
    {
        public static string LogFileDirectory { get; set; } =
            Path.Combine(Directory.GetCurrentDirectory(), "DiscordJiraApp.Logs");

        private static IPAddress JiraAddress { get; set; }

        private static string DiscordWebhookUrl { get; set; }

        private static string DiscordUsername { get; set; }

        private static string DiscordAvatarUrl { get; set; }

        private static int ListenerPort { get; set; }

        private static void Main(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                if (!Directory.Exists(LogFileDirectory))
                    Directory.CreateDirectory(LogFileDirectory);
            }).Wait();

            Task.Factory.StartNew(() => ProcessArgs(args)).Wait();

            var httpListener = new HttpListener();
            httpListener.Prefixes.Add($"http://*:{ListenerPort}/");
            httpListener.Start();
            httpListener.BeginGetContext(RequestCallback, httpListener);

            LoggerHelper.Write(LogType.Info, $"Started listener on {httpListener.Prefixes.First()}");

            while (httpListener.IsListening)
                Thread.Sleep(100);
        }

        private static void RequestCallback(IAsyncResult asyncResult)
        {
            var httpListener = (HttpListener) asyncResult.AsyncState;
            var context = httpListener.GetContext();

            if (context.Request.RemoteEndPoint == null)
                return;

            if (!context.Request.RemoteEndPoint.Address.Equals(JiraAddress))
            {
                LoggerHelper.Write(LogType.Warning, $"Denied request from {context.Request.RemoteEndPoint.Address}");
            }
            else
            {
                LoggerHelper.Write(LogType.Info, $"Request from: {context.Request.RemoteEndPoint?.Address}");
                using (var streamReader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
                {
                    var value = streamReader.ReadToEnd();
                    var jiraPostModel = JsonConvert.DeserializeObject<JiraPostModel>(value);
                    var postValues = JsonConvert.SerializeObject(new Dictionary<string, string>
                    {
                        {
                            "content",
                            $"```[{jiraPostModel.Issue.Key} ({jiraPostModel.Issue.Fields.Status.Name}) :: {jiraPostModel.Issue.Fields.Summary}]\r\n==========\r\n{jiraPostModel.User.Name} trigged a {jiraPostModel.IssueEventTypeName} event at {DateTime.UtcNow:F}```[Reference {jiraPostModel.Issue.Key}]({jiraPostModel.Issue.Fields.Status.IconUrl}browse/{jiraPostModel.Issue.Key})"
                        },
                        {"avatar_url", DiscordAvatarUrl},
                        {"username", DiscordUsername}
                    });

                    var httpContent = new StringContent(postValues, Encoding.UTF8, "application/json");

                    Task.Factory.StartNew(async () =>
                    {
                        using (var httpClient = new HttpClient())
                        {
                            var result = await httpClient.PostAsync(DiscordWebhookUrl, httpContent);

                            if (result.IsSuccessStatusCode)
                                LoggerHelper.Write(LogType.Info, "Successfully posted to Discord!");
                            else
                                LoggerHelper.Write(LogType.Warning, $"Failed to post to Discord - {result.StatusCode}");
                        }
                    });
                }
            }

            httpListener.BeginGetContext(RequestCallback, httpListener);
        }

        private static void ProcessArgs(IEnumerable<string> args)
        {
            foreach (var s in args)
            {
                var cl = s.Split('=');
                var clSwitch = cl[0];
                var clParam = cl[1];

                switch (clSwitch)
                {
                    case "-jiraAddress":
                        JiraAddress = IPAddress.Parse(clParam);
                        break;
                    case "-discordWebhook":
                        DiscordWebhookUrl = clParam;
                        break;
                    case "-discordAvatarUrl":
                        DiscordAvatarUrl = clParam;
                        break;
                    case "-discordUsername":
                        DiscordUsername = clParam;
                        break;
                    case "-listenerPort":
                        ListenerPort = int.Parse(clParam);
                        break;
                }
            }
        }
    }
}