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
        private EventHandler<int>  _scrollToEvent;
        private RegisterFormModel _registerForm;
        private UIView _view;

        public static readonly NSString Key = new NSString("FormCell");
        public static readonly UINib Nib = UINib.FromName("FormCell", NSBundle.MainBundle);
        protected FormCell(IntPtr handle) : base(handle) {}

        public void Configure(string text, RegisterFormModel registerForm, UIView view, int row, EventHandler<int> scrollToEvent, bool isLast)
        {
            _view = view;
            _row = row;
            _registerForm = registerForm;
            _scrollToEvent = scrollToEvent;
            UITextFieldExtensions.SetupField(view, _row, text, _textField, _indicatorView, _textFieldHeightConstraint, _indicatorLabel, isLast ? UIReturnKeyType.Default : UIReturnKeyType.Next);

            string value = _registerForm.ReturnValue(_row);

            if(!string.IsNullOrEmpty(value))
            {
                _textField.Text = value;
                _indicatorLabel.TextColor = Colors.GrayIndicator;
                _indicatorView.BackgroundColor = Colors.GrayDivider;
                _indicatorLabel.Alpha = 1;
            }

            switch (_row)
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
                    break;
                default:
                    _textField.KeyboardType = UIKeyboardType.Default;
                    break;
            }

            _textField.AutocorrectionType = UITextAutocorrectionType.No;
            _textField.TextContentType = new NSString("");

            _textField.EditingDidBegin -= OnTextField_EditingDidBegin;
            _textField.EditingDidBegin += OnTextField_EditingDidBegin;

            _textField.EditingDidEnd -= OnTextField_EditingDidEnd;
            _textField.EditingDidEnd += OnTextField_EditingDidEnd;
        }

        private void OnTextField_EditingDidBegin(object sender, EventArgs e)
        {
            if (_row == 5)
                UITextFieldExtensions.AddDoneButtonToNumericKeyboard(_textField);
            
            _scrollToEvent?.Invoke(this, _row);
        }

        private void OnTextField_EditingChanged(object sender, EventArgs e)
        {
            if (!StringUtils.IsDigitsOnly(_textField.Text))
            {
                _textField.Text = string.Empty;
                _registerForm.SetValue(_row, _textField.Text);
            }
        }

        private void OnTextField_EditingDidEnd(object sender, EventArgs e)
        {
            _registerForm.SetValue(_row, _textField.Text);
        }

    }
}
