using System;
using CoreGraphics;
using Foundation;
using LetterApp.Core.Helpers;
using LetterApp.Core.Models.Cells;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class TextFieldCell : UITableViewCell
    {
        private NSIndexPath _indexPath;
        private EventHandler<NSIndexPath> _scrollToFieldEvent;
        private FormModel _formModel;
        private UIButton _keyboardButton;

        public static readonly NSString Key = new NSString("TextFieldCell");
        public static readonly UINib Nib = UINib.FromName("TextFieldCell", NSBundle.MainBundle);
        protected TextFieldCell(IntPtr handle) : base(handle) {}

        public void Configure(FormModel formModel, NSIndexPath indexPath, EventHandler<NSIndexPath> scrollToFieldEvent, UIView view)
        {
            _formModel = formModel;
            _scrollToFieldEvent = scrollToFieldEvent;
            _indexPath = indexPath;
            _keyboardButton = null;
            _textField.SecureTextEntry = false;

            if(_formModel.SubmitKeyboardButtonAction != null)
            {
                _keyboardButton = new UIButton();
                UIButtonExtensions.SetupButtonAppearance(_keyboardButton, Colors.White, 16f, _formModel.KeyboardButtonText);
                _keyboardButton.TouchUpInside -= OnKeyboardButton_TouchUpInside;
                _keyboardButton.TouchUpInside += OnKeyboardButton_TouchUpInside;
            }

            switch (_formModel.FieldType)
            {
                case FieldType.Email:
                    _textField.KeyboardType = UIKeyboardType.EmailAddress;
                    break;
                case FieldType.Phone:
                    _textField.KeyboardType = UIKeyboardType.PhonePad;
                    _textField.EditingChanged -= OnTextField_EditingChanged;
                    _textField.EditingChanged += OnTextField_EditingChanged;
                    break;
                case FieldType.Code:
                    _textField.KeyboardType = UIKeyboardType.Default;
                    _textField.AutocapitalizationType = UITextAutocapitalizationType.AllCharacters;
                    break;
                case FieldType.Password:
                    _textField.KeyboardType = UIKeyboardType.Default;
                    _textField.SecureTextEntry = true;
                    break;
                default:
                    break;
            }

            if (_formModel.ButtonText != null)
            {
                string buttonText = _formModel.ButtonText[0];

                if(!_textField.SecureTextEntry && _formModel.FieldType == FieldType.Password)
                    buttonText = _formModel.ButtonText[1];

                UIButtonExtensions.SetupButtonAppearance(_button, Colors.MainBlue, 12f, buttonText);

                TextFieldSize(true);

                _button.TouchUpInside -= OnTextFieldButton_TouchUpInside;
                _button.TouchUpInside += OnTextFieldButton_TouchUpInside;
            }
            else 
                TextFieldSize(false);

            _textField.EditingDidBegin -= OnTextField_EditingDidBegin;
            _textField.EditingDidBegin += OnTextField_EditingDidBegin;

            _textField.EditingDidEnd -= OnTextField_EditingDidEnd;
            _textField.EditingDidEnd += OnTextField_EditingDidEnd;

            UITextFieldExtensions.SetupField(view, indexPath.Row, _formModel.IndicatorText, _textField, _indicatorView, _textFieldHeightConstraint, _indicatorLabel, 
                                             _formModel.ReturnKeyType == ReturnKeyType.Default ? UIReturnKeyType.Default : UIReturnKeyType.Next, _keyboardButton);
            
            _textField.AutocorrectionType = UITextAutocorrectionType.No;
            _textField.TextContentType = new NSString("");

            if (!string.IsNullOrEmpty(_formModel.TextFieldValue))
            {
                _textField.Text = _formModel.TextFieldValue;
                _indicatorLabel.TextColor = Colors.GrayIndicator;
                _indicatorLabel.Alpha = 1;
            }
        }

        private void OnTextFieldButton_TouchUpInside(object sender, EventArgs e)
        {
            if (_formModel.ButtonAction != null)
                _formModel.ButtonAction?.Invoke();
            else
            {
                _textField.SecureTextEntry = !_textField.SecureTextEntry;
                _button.SetTitle(_textField.SecureTextEntry == false ? _formModel.ButtonText[0] : _formModel.ButtonText[1], UIControlState.Normal);
            }
                
        }

        private void TextFieldSize(bool hasPassword)
        {
            if (hasPassword)
            {
                _button.SetNeedsLayout();
                _button.LayoutIfNeeded();
                _textfieldWidthConstraint.Constant = (UIScreen.MainScreen.Bounds.Width - 80) - (_button.Frame.Width + 7);
            }
            else
            {
                _button.SetTitle("", UIControlState.Normal);
                _button.SetNeedsLayout();
                _button.LayoutIfNeeded();
                _textfieldWidthConstraint.Constant = UIScreen.MainScreen.Bounds.Width - 80;
            }
        }

        private void OnTextField_EditingDidBegin(object sender, EventArgs e)
        {
            if (_formModel.FieldType == FieldType.Phone)
                UITextFieldExtensions.AddDoneButtonToNumericKeyboard(_textField);

            _scrollToFieldEvent?.Invoke(this, _indexPath);
        }

        private void OnTextField_EditingChanged(object sender, EventArgs e)
        {
            if (!StringUtils.IsDigitsOnly(_textField.Text))
                _textField.Text = string.Empty;
        }

        private void OnTextField_EditingDidEnd(object sender, EventArgs e)
        {
            _formModel.TextFieldValue = _textField.Text;
        }

        private void OnKeyboardButton_TouchUpInside(object sender, EventArgs e)
        {
            _textField.ResignFirstResponder();
            _formModel?.SubmitKeyboardButtonAction?.Invoke();
        }
    }
}
