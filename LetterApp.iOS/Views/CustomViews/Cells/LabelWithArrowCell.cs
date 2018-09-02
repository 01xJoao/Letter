using System;
using Foundation;
using LetterApp.Core.Models.Generic;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class LabelWithArrowCell : UITableViewCell
    {
        private DescriptionTypeEventModel _cell;
        public static readonly NSString Key = new NSString("LabelWithArrowCell");
        public static readonly UINib Nib = UINib.FromName("LabelWithArrowCell", NSBundle.MainBundle);
        protected LabelWithArrowCell(IntPtr handle) : base(handle) {}

        public void Configure(DescriptionTypeEventModel cell)
        {
            _cell = cell;
            _imageView.Image?.Dispose();

            if(cell.HasView)
            {
                _imageView.Hidden = false;
                _arrowWithConstraint.Constant = 24f;
            }
            else
            {
                _imageView.Hidden = true;
                _arrowWithConstraint.Constant = 0f;
            }

            UILabelExtensions.SetupLabelAppearance(_label, cell.Description, Colors.Black, 15f);
            _imageView.Image = UIImage.FromBundle("arrow_right");
        }
    }
}
