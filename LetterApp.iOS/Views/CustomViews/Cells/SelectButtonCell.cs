using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class SelectButtonCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SelectButtonCell");
        public static readonly UINib Nib;

        static SelectButtonCell()
        {
            Nib = UINib.FromName("SelectButtonCell", NSBundle.MainBundle);
        }

        protected SelectButtonCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
