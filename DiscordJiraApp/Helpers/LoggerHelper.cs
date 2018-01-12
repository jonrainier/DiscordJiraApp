// Copyright (c) 2018 Initial Servers LLC. All rights reserved.
// https://initialservers.com/

using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using DiscordJiraApp.Enums;
using DiscordJiraApp.Extensions;

namespace DiscordJiraApp.Helpers
{
    public class LoggerHelper
    {
        public static void Write(LogType logType, object logMessage, bool logFile = false)
        {
            var logTypeDescription = logType.GetAttributeOfType<DescriptionAttribute>().Description;
            var tempString =
                $"{DateTime.UtcNow:u} [{logTypeDescription}]: {logMessage}";
            var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;

            Console.WriteLine(tempString);

            if (logFile)
                Log(logTypeDescription, tempString, assemblyName);
        }

        private static void Log(string logTypeDescription, object tempString, string assembly)
        {
            var logFileName = Path.Combine(Application.LogFileDirectory,
                $"{assembly}-{logTypeDescription}-{DateTime.UtcNow:yyyy-MM-dd}.log");

            try
            {
                using (var streamWriter = File.AppendText(logFileName))
                {
                    streamWriter.WriteLine(tempString);
                }
            }
            catch (IOException)
            {
                Log(logTypeDescription, tempString, assembly);
            }
        }
    }
}