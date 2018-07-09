using System;

using Foundation;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class TitleHeader : UITableViewCell
    {
        public static readonly NSString Key = new NSString("TitleHeader");
        public static readonly UINib Nib = UINib.FromName("TitleHeader", NSBundle.MainBundle);
        protected TitleHeader(IntPtr handle) : base(handle) {}

        public void Configure(string text)
        {
            this.BackgroundColor = Colors.SelectBlue;
            UILabelExtensions.SetupLabelAppearance(_label, text, Colors.White, 16f, UIFontWeight.Medium);
            CustomUIExtensions.LabelShadow(_label);
        }
    }
}
