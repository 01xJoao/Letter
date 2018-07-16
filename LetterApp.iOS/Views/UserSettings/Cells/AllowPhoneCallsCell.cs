using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.UserSettings.Cells
{
    public partial class AllowPhoneCallsCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("AllowPhoneCallsCell");
        public static readonly UINib Nib;

        static AllowPhoneCallsCell()
        {
            Nib = UINib.FromName("AllowPhoneCallsCell", NSBundle.MainBundle);
        }

        protected AllowPhoneCallsCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
