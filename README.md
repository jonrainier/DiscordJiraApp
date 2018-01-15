# DiscordJiraApp
An application that will push Jira notifications to your Discord server.

Create a Webhook
------
1. In your selected server, open up `Server Settings`.
2. Click on `Webhooks` and select `Create Webhook`.
3. `Edit` your Webhook with your desired settings. You can optionally set an avatar and name now, however, they will be overwritten on app startup.
![Discord Webhook Edit Settings](http://screenshots.initialservers.com/jrainier/ae80a7c7e25d858d87ad26e177236045.png)
4. Set your `Webhook URL` a side and continue over to Jira.

Enable Webhook in Jira
------
1. Select the settings button next to your avatar ![Jira Settings Cogwheel](http://screenshots.initialservers.com/jrainier/df2d5bf273bcc4fe2f0f070f4f90666d.png) and click on `System`.
2. On the left panel, navigate to `WebHooks` under the `Advanced` section.
3. Click on `Create a WebHook` on the top right of the current tab.
4. Give your Webhook a `Name`, set the Status to `Enabled`, and the `URL` to `http://<ip_of_listener>:<port_of_listener>`.
5. Set the `Events` of the Webhook. **Not all events have been tested. The ones shown in the screenshot below will work without an issue.**
![Example Jira Webhook and Triggers](http://screenshots.initialservers.com/jrainier/e61eae875eaf715ed539da1171481c63.png)

How to Run
------
You must launch `DiscordJiraApp.exe` with specific command line arguments. Below is a summary of those arguments:

Syntax is the same for each argument: `-argument=parameter`

| Argument | Description | Required |
| :-------------: |:-------------:| :-----:|
| `-jiraAddress` | IP Address of your Jira server | **true** |
| `-listenerPort` | Port that you would like the listener to wait on | **true** |
| `-discordWebhook` | Webhook link generated from your Discord server | **true** |
| `-discordUsername` | Username of your bot | **true** |
| `-discordAvatarUrl` | URL that specifies your bot's avatar | **true** |

Example: `DiscordJiraApp.exe -jiraAddress=127.0.0.1 -listenerPort=2555 -discordWebhook=https://canary.discordapp.com/api/webhooks/<server_uid>/<unique_identifier> -discordUsername="Jira Issue Tracker" -discordAvatarUrl=https://i.imgur.com/mdp3NY3.png`
