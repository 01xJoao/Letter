using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class SwitchCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SwitchCell");
        public static readonly UINib Nib;

        static SwitchCell()
        {
            Nib = UINib.FromName("SwitchCell", NSBundle.MainBundle);
        }

        protected SwitchCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
