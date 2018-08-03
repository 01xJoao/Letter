using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.ProfileDivision
{
    public partial class OrganizationDivisionsView : UIView
    {
        public static readonly UINib Nib = UINib.FromName("OrganizationDivisionsView", NSBundle.MainBundle);
        public OrganizationDivisionsView(IntPtr handle) : base(handle) { }
        public static OrganizationDivisionsView Create => Nib.Instantiate(null, null)[0] as OrganizationDivisionsView;

        public void Configure(ProfileOrganizationDetails division)
        {
            _imageView.Image?.Dispose();

            ImageService.Instance.LoadStream((token) => {
                return ImageHelper.GetStreamFromImageByte(token, division.Picture);
            }).ErrorPlaceholder("warning_image", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_imageView);

            UILabelExtensions.SetupLabelAppearance(_nameLabel, division.Name, Colors.ProfileGrayDarker, 14f);
            UILabelExtensions.SetupLabelAppearance(_membersLabel, division.MembersCount, Colors.GrayLabel, 13f);
        }
    }
}
