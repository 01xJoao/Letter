using System;
namespace LetterApp.Core.Models
{
    public class SettingsAllowCallsModel
    {
        public SettingsAllowCallsModel(string allowCallsTitle, string allowCallsDescription, EventHandler<bool> allowCalls)
        {
            AllowCallsTitle = allowCallsTitle;
            AllowCallsDescription = allowCallsDescription;
            AllowCalls = allowCalls;
        }

        public string AllowCallsTitle { get; set; }
        public string AllowCallsDescription { get; set; }
        public EventHandler<bool> AllowCalls { get; set; }
    }
}
