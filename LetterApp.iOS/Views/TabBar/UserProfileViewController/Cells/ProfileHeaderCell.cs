using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using Foundation;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.UserProfileViewController.Cells
{
    public partial class ProfileHeaderCell : UITableViewCell
    {
        private ProfileHeaderModel _profile;
        public static readonly NSString Key = new NSString("ProfileHeaderCell");
        public static readonly UINib Nib = UINib.FromName("ProfileHeaderCell", NSBundle.MainBundle);
        protected ProfileHeaderCell(IntPtr handle) : base(handle) { }

        public void Configure(ProfileHeaderModel profile)
        {
            _profile = profile;
            _backgroundView.BackgroundColor = Colors.MainBlue;
            UILabelExtensions.SetupLabelAppearance(_nameLabel, profile.Name, Colors.White, 22f);
            UITextFieldExtensions.SetupTextFieldAppearance(_descriptionField, Colors.White, 14f, DescriptionField, Colors.White70, Colors.White, Colors.MainBlue);
            _configImage.Image = UIImage.FromBundle("settings");

            if (!string.IsNullOrEmpty(profile.Picture))
            {
                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, profile.Picture);
                }).Transform(new CircleTransformation()).Into(_profileImage);
            }
            else
            {
                _profileImage.Image = UIImage.FromBundle("add_picture");
            }

            if (!string.IsNullOrEmpty(profile.Description))
                _descriptionField.Text = profile.Description;

            _descriptionField.ShouldChangeCharacters = OnDescriptionField_ShouldChangeCharacters;

            _descriptionField.EditingDidEnd -= OnDescriptionField_EditingDidEnd;
            _descriptionField.EditingDidEnd += OnDescriptionField_EditingDidEnd;

            _configButton.TouchUpInside -= OnConfigButton_TouchUpInside;
            _configButton.TouchUpInside += OnConfigButton_TouchUpInside;
        }

        private void OnConfigButton_TouchUpInside(object sender, EventArgs e)
        {
            _profile.OpenSettingsEvent?.Invoke(null, EventArgs.Empty);
        }

        private void OnDescriptionField_EditingDidEnd(object sender, EventArgs e)
        {
            _profile.UpdateDescriptionEvent?.Invoke(this, _descriptionField.Text);
        }

        private bool OnDescriptionField_ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
        {
            var newLength = textField.Text.Length + replacementString.Length - range.Length;
            return newLength <= 80;
        }

        #region Resources 

        private string DescriptionField => L10N.Localize("UserProfile_DescriptionHint");

        #endregion
    }
}
