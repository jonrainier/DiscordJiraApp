// Copyright (c) 2018 Initial Servers LLC. All rights reserved.
// https://initialservers.com/

using System.ComponentModel;

namespace DiscordJiraApp.Enums
{
    public enum LogType
    {
        [Description("DEBUG")] Debug,
        [Description("INFO")] Info,
        [Description("WARNING")] Warning,
        [Description("ERROR")] Error
    }
}