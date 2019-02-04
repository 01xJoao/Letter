using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.Division.Cells
{
    public partial class OrganizationDetailCell : UITableViewCell
    {
        private XPCommand _organizationEvent;
        public static readonly NSString Key = new NSString("OrganizationDetailCell");
        public static readonly UINib Nib = UINib.FromName("OrganizationDetailCell", NSBundle.MainBundle);
        protected OrganizationDetailCell(IntPtr handle) : base(handle) {}

        public void Configure(OrganizationInfoModel organizationInfoModel)
        {
            _organizationEvent = organizationInfoModel.OpenOrganizationCommand;

            _imageView.Image?.Dispose();
            ImageService.Instance.LoadStream((token) => {
                return ImageHelper.GetStreamFromImageByte(token, organizationInfoModel.Picture);
            }).ErrorPlaceholder("organization_noimage", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_imageView);

            UILabelExtensions.SetupLabelAppearance(_sectionLabel, organizationInfoModel.Section, Colors.ProfileGray, 12f);
            UILabelExtensions.SetupLabelAppearance(_label, organizationInfoModel.Name, Colors.ProfileGrayDarker, 14f, UIFontWeight.Medium);

            _button.TouchUpInside -= OnButton_TouchUpInside;
            _button.TouchUpInside += OnButton_TouchUpInside;
        }

        private void OnButton_TouchUpInside(object sender, EventArgs e)
        {
            if (_organizationEvent.CanExecute())
                _organizationEvent.Execute();
        }
    }
}
