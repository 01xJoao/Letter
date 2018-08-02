using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.UserProfileViewController.Cells
{
    [Register("ProfileHeaderView")]
    public partial class ProfileHeaderView : UIView
    {
        private ProfileHeaderModel _profile;
        public static readonly UINib Nib = UINib.FromName("ProfileHeaderView", NSBundle.MainBundle);
        public static ProfileHeaderView Create() => Nib.Instantiate(null, null)[0] as ProfileHeaderView;
        public ProfileHeaderView(IntPtr handle) : base(handle) {}

        public void Configure(ProfileHeaderModel profile)
        {
            _profile = profile;
            this.BackgroundColor = Colors.MainBlue;
            UILabelExtensions.SetupLabelAppearance(_nameLabel, profile.Name, Colors.White, 22f);
            UITextFieldExtensions.SetupTextFieldAppearance(_descriptionField, Colors.White, 14f, DescriptionField, Colors.White70, Colors.White, Colors.MainBlue);

            _descriptionField.ReturnKeyType = UIReturnKeyType.Done;

            _descriptionField.AdjustsFontSizeToFitWidth = true;
            _descriptionField.MinimumFontSize = 11f;

            _descriptionField.ShouldReturn -= (field) => TextFieldShouldReturn(field);
            _descriptionField.ShouldReturn += (field) => TextFieldShouldReturn(field);

            _settingsImage.Image = UIImage.FromBundle("settings");

            _profileImage.Image?.Dispose();
            ImageService.Instance.LoadStream((token) => {
                return ImageHelper.GetStreamFromImageByte(token, profile.Picture);
            }).ErrorPlaceholder("add_photo", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_profileImage);
            CustomUIExtensions.RoundShadow(_profileImage);

            CustomUIExtensions.LabelShadow(_nameLabel);

            if (!string.IsNullOrEmpty(profile.Description))
                _descriptionField.Text = profile.Description;
            else
                _descriptionField.Font = UIFont.ItalicSystemFontOfSize(14f);

            _descriptionField.ShouldChangeCharacters = OnDescriptionField_ShouldChangeCharacters;

            _descriptionField.EditingDidBegin -= OnDescriptionField_EditingDidBegin;
            _descriptionField.EditingDidBegin += OnDescriptionField_EditingDidBegin;

            _descriptionField.EditingDidEnd -= OnDescriptionField_EditingDidEnd;
            _descriptionField.EditingDidEnd += OnDescriptionField_EditingDidEnd;

            _settingsButton.TouchUpInside -= OnConfigButton_TouchUpInside;
            _settingsButton.TouchUpInside += OnConfigButton_TouchUpInside;

            _profileButton.TouchUpInside -= OnProfileButton_TouchUpInside;
            _profileButton.TouchUpInside += OnProfileButton_TouchUpInside;
        }

        private void OnProfileButton_TouchUpInside(object sender, EventArgs e)
        {
            _profile.OpenGalery?.Invoke(null, EventArgs.Empty);
        }

        private bool TextFieldShouldReturn(UITextField field)
        {
            return field.ResignFirstResponder();
        }

        private void OnDescriptionField_EditingDidBegin(object sender, EventArgs e)
        {
            _descriptionField.Placeholder = "";

            if (string.IsNullOrEmpty(_descriptionField.Text))
                _descriptionField.Font = UIFont.ItalicSystemFontOfSize(14f);
            else
                _descriptionField.Font = UIFont.SystemFontOfSize(14f);
        }

        private void OnConfigButton_TouchUpInside(object sender, EventArgs e)
        {
            _profile.OpenSettingsEvent?.Invoke(null, EventArgs.Empty);
        }

        private void OnDescriptionField_EditingDidEnd(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_descriptionField.Text))
            {
                _descriptionField.Font = UIFont.ItalicSystemFontOfSize(14f);
                _descriptionField.AttributedPlaceholder = new NSAttributedString(DescriptionField, new UIStringAttributes() { ForegroundColor = Colors.White70 });
            }
            else
                _descriptionField.Font = UIFont.SystemFontOfSize(14f);
            
            _profile.UpdateDescriptionEvent?.Invoke(this, _descriptionField.Text);
        }

        private bool OnDescriptionField_ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
        {
            if (!string.IsNullOrEmpty(_descriptionField.Text))
                _descriptionField.Font = UIFont.SystemFontOfSize(14f);
            
            var newLength = textField.Text.Length + replacementString.Length - range.Length;
            return newLength <= 40;
        }

        #region Resources 

        private string DescriptionField => L10N.Localize("UserProfile_DescriptionHint");

        #endregion

    }
}
