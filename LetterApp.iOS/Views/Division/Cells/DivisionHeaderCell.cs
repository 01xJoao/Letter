using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

//NOT USED
namespace LetterApp.iOS.Views.Division.Cells
{
    public partial class DivisionHeaderCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("DivisionHeaderCell");
        public static readonly UINib Nib = UINib.FromName("DivisionHeaderCell", NSBundle.MainBundle);
        protected DivisionHeaderCell(IntPtr handle) : base(handle) {}

        public void Configure(DivisionHeaderModel division)
        {

            this.BackgroundColor = Colors.MainBlue;
            UILabelExtensions.SetupLabelAppearance(_nameLabel, division.Name, Colors.White, 17f);
            UILabelExtensions.SetupLabelAppearance(_membersLabel, division.MembersCount, Colors.White, 15f);
            UILabelExtensions.SetupLabelAppearance(_descriptionLabel, division.Description, Colors.White, 15f);

            _backImage.Image = UIImage.FromBundle("back_white");
            _membersImage.Image = UIImage.FromBundle("members");

            _profileImage.Image?.Dispose();
            ImageService.Instance.LoadStream((token) => {
                return ImageHelper.GetStreamFromImageByte(token, division.Picture);
            }).LoadingPlaceholder("warning_image", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_profileImage);
            CustomUIExtensions.RoundShadow(_profileImage);


        }
    }
}
