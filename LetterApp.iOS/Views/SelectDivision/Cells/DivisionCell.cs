using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CoreGraphics;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
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

            ImageService.Instance.LoadStream((token) => {
                return ImageHelper.GetStreamFromImageByte(token, division.Picture);
            }).LoadingPlaceholder("warning_image", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_imageView);
            CustomUIExtensions.RoundShadow(_imageView);

            CustomUIExtensions.LabelShadow(_titleLabel);

            UILabelExtensions.SetupLabelAppearance(_titleLabel, division.Name, Colors.White, 16f, UIFontWeight.Bold);
            UILabelExtensions.SetupLabelAppearance(_subtitleLabel, division.Description, Colors.White, 12f);
        }
    }
}
