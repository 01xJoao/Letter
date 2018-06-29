using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class SubtitleCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SubtitleCell");
        public static readonly UINib Nib;

        static SubtitleCell()
        {
            Nib = UINib.FromName("SubtitleCell", NSBundle.MainBundle);
        }

        protected SubtitleCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
