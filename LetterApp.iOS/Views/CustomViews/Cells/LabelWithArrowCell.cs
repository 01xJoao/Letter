using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class LabelWithArrowCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("LabelWithArrowCell");
        public static readonly UINib Nib;

        static LabelWithArrowCell()
        {
            Nib = UINib.FromName("LabelWithArrowCell", NSBundle.MainBundle);
        }

        protected LabelWithArrowCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
