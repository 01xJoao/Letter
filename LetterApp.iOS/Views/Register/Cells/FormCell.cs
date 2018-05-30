using System;

using Foundation;
using LetterApp.Core.Helpers;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.Register.Cells
{
    public partial class FormCell : UITableViewCell
    {
        private int _row;
        private RegisterFormModel _registerForm;
        public static readonly NSString Key = new NSString("FormCell");
        public static readonly UINib Nib = UINib.FromName("FormCell", NSBundle.MainBundle);
        protected FormCell(IntPtr handle) : base(handle) {}

        public void Configure(string text, RegisterFormModel registerForm, UIView view, int row, bool isLast)
        {
            _row = row;
            _registerForm = registerForm;
            UITextFieldExtensions.SetupField(view, row, text, _textField, _indicatorView, _textFieldHeightConstraint, _indicatorLabel, isLast ? UIReturnKeyType.Default : UIReturnKeyType.Next);

            string value = _registerForm.ReturnValue(row);

            if(!string.IsNullOrEmpty(value))
            {
                _textField.Text = value;
                _indicatorView.BackgroundColor = Colors.GrayDivider;
                _indicatorLabel.TextColor = Colors.GrayIndicator;
                _indicatorLabel.Alpha = 1;
            }

            switch (row)
            {
                case 2:
                    _textField.KeyboardType = UIKeyboardType.EmailAddress;
                    break;
                case 3:
                case 4:
                    _textField.SecureTextEntry = true;
                    _textField.KeyboardType = UIKeyboardType.Default;
                    break;
                case 5:
                    _textField.KeyboardType = UIKeyboardType.NumberPad;
                    _textField.EditingChanged -= OnTextField_EditingChanged;
                    _textField.EditingChanged += OnTextField_EditingChanged;

                    _textField.EditingDidBegin -= OnTextField_EditingDidBegin;
                    _textField.EditingDidBegin += OnTextField_EditingDidBegin;
                    break;
                default:
                    _textField.KeyboardType = UIKeyboardType.Default;
                    break;
            }

            _textField.AutocorrectionType = UITextAutocorrectionType.No;

            _textField.EditingDidEnd -= OnTextField_EditingDidEnd;
            _textField.EditingDidEnd += OnTextField_EditingDidEnd;
        }

        private void OnTextField_EditingDidBegin(object sender, EventArgs e)
        {
            UITextFieldExtensions.AddDoneButtonToNumericKeyboard(_textField);
        }

        private void OnTextField_EditingChanged(object sender, EventArgs e)
        {
            if (!StringUtils.IsDigitsOnly(_textField.Text))
                _textField.Text = string.Empty;
        }

        private void OnTextField_EditingDidEnd(object sender, EventArgs e)
        {
            _registerForm.SetValue(_row, _textField.Text);
        }

    }
}
