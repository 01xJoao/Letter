using System;
using LetterApp.Core.Helpers;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class InputTextViewController : UIViewController
    {
        private string _title;
        private string _inputContent;
        private string _confirmButtonText;
        private string _hint;
        private InputType _inputType;
        private Action<string> _positiveButton;
        private Action _negativeButton;

        public InputTextViewController(string title, string inputContent, string confirmButtonText, string hint, InputType inputType, Action<string> positiveButton, Action negativeButton) : base("InputTextViewController", null)
        {
            _title = title;
            _inputContent = inputContent;
            _confirmButtonText = confirmButtonText;
            _hint = hint;
            _inputType = inputType;
            _positiveButton = positiveButton;
            _negativeButton = negativeButton;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            DismissKeyboardOnBackgroundTap();

            this.View.Alpha = 0.3f;

            switch (_inputType)
            {
                case InputType.Email:
                    _textField.KeyboardType = UIKeyboardType.EmailAddress;
                    break;
                case InputType.Number:
                    _textField.KeyboardType = UIKeyboardType.NumberPad;
                    break;
                case InputType.Phone:
                    _textField.KeyboardType = UIKeyboardType.PhonePad;
                    break;
                default:
                    _textField.KeyboardType = UIKeyboardType.Default;
                    break;
            }

            _textField.AutocorrectionType = UITextAutocorrectionType.No;


            this.View.BackgroundColor = Colors.Black30;

            _backgroundView.Layer.CornerRadius = 2f;
            _buttonView.Layer.CornerRadius = 2f;
            CustomUIExtensions.ViewShadow(_backgroundView);

            UILabelExtensions.SetupLabelAppearance(_titleLabel, _title, Colors.Black, 20);
            UITextFieldExtensions.SetupField(this.View, 0, _hint, _textField, _indicatorView, _textFieldHeightConstraint, _indicatorLabel);
            UIButtonExtensions.SetupButtonAppearance(_confirmButton, Colors.White, 16f, _confirmButtonText);

            if(!string.IsNullOrEmpty(_inputContent))
            {
                _textField.Text = _inputContent;
                _indicatorLabel.TextColor = Colors.GrayIndicator;
                _indicatorLabel.Alpha = 1;
            }

            _closeButton.SetImage(UIImage.FromBundle("close_black"), UIControlState.Normal);
            _closeButton.TintColor = Colors.Black;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;

            _confirmButton.TouchUpInside -= OnConfirmButton_TouchUpInside;
            _confirmButton.TouchUpInside += OnConfirmButton_TouchUpInside;
        }

        private void OnConfirmButton_TouchUpInside(object sender, EventArgs e)
        {
            if(_inputType == InputType.Email)
            {
                _textField.Text = StringUtils.RemoveWhiteSpaces(_textField.Text);

                if(EmailUtils.IsValidEmail(_textField.Text))
                {
                    _positiveButton.Invoke(_textField.Text);
                    Dismiss();
                }
                else
                    _indicatorView.BackgroundColor = Colors.Red;
            }
            else
            {
                _positiveButton.Invoke(_textField.Text);
                Dismiss();
            }
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            _negativeButton.Invoke();
            Dismiss();
        }

        public void Show()
        {
            this.View.Frame = UIApplication.SharedApplication.KeyWindow.Bounds;
            UIApplication.SharedApplication.KeyWindow.AddSubview(this.View);
            UIView.Animate(0.3, () => View.Alpha = 1);
        }

        public void Dismiss()
        {
            UIView.AnimateNotify(0.3, () => View.Alpha = 0, (finished) => CleanFromMemory());
        }

        protected void DismissKeyboardOnBackgroundTap()
        {
            var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
            tap.AddTarget(() => View.EndEditing(true));
            View.AddGestureRecognizer(tap);
        }

        private void CleanFromMemory()
        {
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

