using System;

using Foundation;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.UserSettings.Cells
{
    public partial class PhoneNumberCell : UITableViewCell
    {
        private XPCommand<string> _changeNumberCommand;
        public static readonly NSString Key = new NSString("PhoneNumberCell");
        public static readonly UINib Nib = UINib.FromName("PhoneNumberCell", NSBundle.MainBundle);
        protected PhoneNumberCell(IntPtr handle) : base(handle) {}

        public void Configure(SettingsPhoneModel phone)
        {
            _changeNumberCommand = phone.ChangeNumber;

            UILabelExtensions.SetupLabelAppearance(_label, phone.PhoneDescription, Colors.Black, 15f);
            UITextFieldExtensions.SetupTextFieldAppearance(_textField, Colors.GrayIndicator, 14f, "#", Colors.GrayIndicator, Colors.GrayIndicator, Colors.White.ColorWithAlpha(0f));
            _textField.Text = phone.PhoneNumber;

            _textField.ShouldChangeCharacters = OnDescriptionField_ShouldChangeCharacters;

            UITextFieldExtensions.AddDoneButtonToNumericKeyboard(_textField);
            _textField.KeyboardType = UIKeyboardType.PhonePad;

            _textField.EditingDidEnd -= OnTextField_EditingDidEnd;
            _textField.EditingDidEnd += OnTextField_EditingDidEnd;
        }

        private void OnTextField_EditingDidEnd(object sender, EventArgs e)
        {
            if (_changeNumberCommand.CanExecute(_textField.Text))
                _changeNumberCommand.Execute(_textField.Text);
                
        }

        private bool OnDescriptionField_ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
        {
            var newLength = textField.Text.Length + replacementString.Length - range.Length;
            return newLength <= 16;
        }
    }
}
