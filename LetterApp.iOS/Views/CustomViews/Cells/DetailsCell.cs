using System;

using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class DetailsCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("DetailsCell");
        public static readonly UINib Nib = UINib.FromName("DetailsCell", NSBundle.MainBundle);
        protected DetailsCell(IntPtr handle) : base(handle){}

        public void Configure(ProfileDetail details)
        {
            UILabelExtensions.SetupLabelAppearance(_detailLabel, details.Description, Colors.ProfileGray, 12f);
            UILabelExtensions.SetupLabelAppearance(_detailValueLabel, details.Value, Colors.ProfileGrayDarker, 14f, UIFontWeight.Medium);
            _separatorView.BackgroundColor = Colors.ProfileGrayWhiter;
        }
    }
}
