using System;
namespace LetterApp.Core.Models
{
    public class SettingsPhoneModel
    {
        public SettingsPhoneModel(string phoneDescription, string phoneNumber, EventHandler<int> changeNumber)
        {
            PhoneDescription = phoneDescription;
            PhoneNumber = phoneNumber;
            ChangeNumber = changeNumber;
        }

        public string PhoneDescription { get; set; }
        public string PhoneNumber { get; set; }
        public EventHandler<int> ChangeNumber { get; set; }
    }
}
