using System;
using Foundation;
using LetterApp.Core.Models.Generic;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class LabelWithArrowCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("LabelWithArrowCell");
        public static readonly UINib Nib = UINib.FromName("LabelWithArrowCell", NSBundle.MainBundle);
        protected LabelWithArrowCell(IntPtr handle) : base(handle) {}

        public void Configure(DescriptionTypeEventModel cell)
        {
            _imageView.Image?.Dispose();
            _imageView.Image = UIImage.FromBundle("arrow_right");

            if (cell.HasView)
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
        }

        public void ConfigureForChat(string text, UITextAlignment textAlignment = UITextAlignment.Left, bool hasArrow = false)
        {
            _imageView.Image?.Dispose();
            _imageView.Image = UIImage.FromBundle("arrow_right");

            if (hasArrow)
            {
                _imageView.Hidden = false;
                _arrowWithConstraint.Constant = 24f;
            }
            else
            {
                _imageView.Hidden = true;
                _arrowWithConstraint.Constant = 0f;
            }

            UILabelExtensions.SetupLabelAppearance(_label, text, textAlignment == UITextAlignment.Left ? Colors.Black : Colors.GrayIndicator, 15f);
            _label.TextAlignment = textAlignment;
        }
    }
}
