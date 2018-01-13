# DiscordJiraApp
An application that will push Jira notifications to your Discord server.

How to Run
------
You must launch `DiscordJiraApp.exe` with specific command line arguments. Below is a summary of those arguments:

Syntax is the same for each argument: `-argument=parameter`

| Argument | Description | Required |
| :-------------: |:-------------:| :-----:|
| `-jiraAddress` | IP Address of your Jira server | **true** |
| `-discordWebhook` | WebHook link generated from your Discord server | **true** |
| `-listenerPort` | Port that you would like the listener to wait on | **true** |

Example: `DiscordJiraApp.exe -jiraAddress=127.0.0.1 -listenerPort=2555 -discordWebhook=https://canary.discordapp.com/api/webhooks/<server_uid>/<unique_identifier>`
