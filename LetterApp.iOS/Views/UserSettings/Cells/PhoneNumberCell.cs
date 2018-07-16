using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.UserSettings.Cells
{
    public partial class PhoneNumberCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PhoneNumberCell");
        public static readonly UINib Nib;

        static PhoneNumberCell()
        {
            Nib = UINib.FromName("PhoneNumberCell", NSBundle.MainBundle);
        }

        protected PhoneNumberCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
