using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CoreGraphics;
using FFImageLoading;
using FFImageLoading.Transformations;
using Foundation;
using LetterApp.iOS.Helpers;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class DivisionCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("DivisionCell");
        public static readonly UINib Nib = UINib.FromName("DivisionCell", NSBundle.MainBundle);
        protected DivisionCell(IntPtr handle) : base(handle){}

        public void Configure(DivisionModel division)
        {
            _imageView.Image?.Dispose();

            if(!string.IsNullOrEmpty(division.Picture))
            {
                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, division.Picture);
                }).Transform(new CircleTransformation()).Into(_imageView);
            }
            else
            {
                _imageView.Image = new UIImage();
                _imageView.BackgroundColor = Colors.Black30;
            }
            CustomUIExtensions.LabelShadow(_titleLabel);
            CustomUIExtensions.RoundShadow(_imageView);

            UILabelExtensions.SetupLabelAppearance(_titleLabel, division.Name, Colors.White, 16f, UIFontWeight.Bold);
            UILabelExtensions.SetupLabelAppearance(_subtitleLabel, division.Description, Colors.White, 12f);
        }
    }
}
