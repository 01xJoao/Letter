using System;

using Foundation;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class SubtitleCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SubtitleCell");
        public static readonly UINib Nib = UINib.FromName("SubtitleCell", NSBundle.MainBundle);
        protected SubtitleCell(IntPtr handle) : base(handle) {}

        public void Configure(string text)
        {
            UILabelExtensions.SetupLabelAppearance(_subtitleLabel, text, Colors.White, 15f);
        }
    }
}
