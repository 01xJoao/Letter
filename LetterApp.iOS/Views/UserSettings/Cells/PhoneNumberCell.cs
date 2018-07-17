using System;

using Foundation;
using LetterApp.Core.Models;
using UIKit;

namespace LetterApp.iOS.Views.UserSettings.Cells
{
    public partial class PhoneNumberCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PhoneNumberCell");
        public static readonly UINib Nib = UINib.FromName("PhoneNumberCell", NSBundle.MainBundle);
        protected PhoneNumberCell(IntPtr handle) : base(handle) {}

        public void Configure(SettingsPhoneModel phone)
        {
            
        }
    }
}
