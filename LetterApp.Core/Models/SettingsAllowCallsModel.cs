using System;
using LetterApp.Core.Helpers.Commands;

namespace LetterApp.Core.Models
{
    public class SettingsAllowCallsModel
    {
        public SettingsAllowCallsModel(string allowCallsTitle, string allowCallsDescription, XPCommand<bool> allowCalls, bool isActive)
        {
            AllowCallsTitle = allowCallsTitle;
            AllowCallsDescription = allowCallsDescription;
            AllowCalls = allowCalls;
            IsActive = isActive;
        }

        public bool IsActive { get; set; }
        public string AllowCallsTitle { get; set; }
        public string AllowCallsDescription { get; set; }
        public XPCommand<bool> AllowCalls { get; set; }
    }
}
