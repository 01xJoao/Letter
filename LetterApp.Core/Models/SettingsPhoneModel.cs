using System;
using LetterApp.Core.Helpers.Commands;

namespace LetterApp.Core.Models
{
    public class SettingsPhoneModel
    {
        public SettingsPhoneModel(string phoneDescription, string phoneNumber, XPCommand<string> changeNumber)
        {
            PhoneDescription = phoneDescription;
            PhoneNumber = phoneNumber;
            ChangeNumber = changeNumber;
        }

        public string PhoneDescription { get; set; }
        public string PhoneNumber { get; set; }
        public XPCommand<string> ChangeNumber { get; set; }
    }
}
