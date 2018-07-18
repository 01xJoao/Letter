using System;
using LetterApp.Core.Helpers.Commands;

namespace LetterApp.Core.Models
{
    public class SettingsAllowCallsModel
    {
        public SettingsAllowCallsModel(string allowCallsTitle, string allowCallsDescription, XPCommand<bool> allowCalls)
        {
            AllowCallsTitle = allowCallsTitle;
            AllowCallsDescription = allowCallsDescription;
            AllowCalls = allowCalls;
        }

        public string AllowCallsTitle { get; set; }
        public string AllowCallsDescription { get; set; }
        public XPCommand<bool> AllowCalls { get; set; }
    }
}
