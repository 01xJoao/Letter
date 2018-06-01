using System;
using System.ComponentModel;
using System.Linq;
using Airbnb.Lottie;
using Foundation;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.RecoverPassword
{
    public partial class RecoverPasswordViewController : XViewController<RecoverPasswordViewModel>
    {
        public override bool HandlesKeyboardNotifications => true;
        public override bool ShowAsPresentView => true;
        private LOTAnimationView _lottieAnimation;
        private bool keyboardViewState;

        public RecoverPasswordViewController() : base("RecoverPasswordViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;

            _requestCodeButton.TouchUpInside -= OnRequestCodeButton_TouchUpInside;
            _requestCodeButton.TouchUpInside += OnRequestCodeButton_TouchUpInside;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;

            _passButton.TouchUpInside -= OnPassButton_TouchUpInside;
            _passButton.TouchUpInside += OnPassButton_TouchUpInside;

            _confirmButton.TouchUpInside -= OnConfirmButton_TouchUpInside;
            _confirmButton.TouchUpInside += OnConfirmButton_TouchUpInside;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.IsValidPassword):
                    InvalidPassword();
                    break;
                case nameof(ViewModel.IsSubmiting):
                    Loading();
                    break;
                default:
                    break;
            }
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            var tuple = new Tuple<string, string, string>(_passwordTextField.Text, _confirmPassTextField.Text, _codeTextField.Text);

            if (ViewModel.SubmitFormCommand.CanExecute(tuple))
                ViewModel.SubmitFormCommand.Execute(tuple);
        }

        private void OnRequestCodeButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.ResendCodeCommand.CanExecute())
                ViewModel.ResendCodeCommand.Execute();
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            if(ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        private void OnPassButton_TouchUpInside(object sender, EventArgs e)
        {
            _passwordTextField.SecureTextEntry = !_passwordTextField.SecureTextEntry;

            if(_passwordTextField.SecureTextEntry)
                _passButton.SetTitle(ViewModel.ShowButton, UIControlState.Normal);
            else
                _passButton.SetTitle(ViewModel.HideButton, UIControlState.Normal);
        }

        private void OnConfirmButton_TouchUpInside(object sender, EventArgs e)
        {
            _confirmPassTextField.SecureTextEntry = !_confirmPassTextField.SecureTextEntry;

            if (_confirmPassTextField.SecureTextEntry)
                _confirmButton.SetTitle(ViewModel.ShowButton, UIControlState.Normal);
            else
                _confirmButton.SetTitle(ViewModel.HideButton, UIControlState.Normal);
        }

        private void SetupView()
        {
            UILabelExtensions.SetupLabelAppearance(_titleLabel, ViewModel.NewPassTitle, Colors.Black, 17f, UIFontWeight.Semibold);
            _closeButton.SetImage(UIImage.FromBundle("close_black"), UIControlState.Normal);
            _closeButton.TintColor = Colors.Black;
            _backgroundView.BackgroundColor = Colors.MainBlue4;
            _formView.BackgroundColor = Colors.MainBlue4;

            UIButtonExtensions.SetupButtonAppearance(_requestCodeButton, Colors.MainBlue, 12f, ViewModel.RequestAgainButton);
            UIButtonExtensions.SetupButtonAppearance(_submitButton, Colors.White, 16f, ViewModel.SubmitButton);
            UIButtonExtensions.SetupButtonAppearance(_passButton, Colors.MainBlue, 12f, ViewModel.ShowButton);
            UIButtonExtensions.SetupButtonAppearance(_confirmButton, Colors.MainBlue, 12f, ViewModel.ShowButton);

            ConfigureButton(_requestCodeButton, _codeWidthConstraint);
            ConfigureButton(_passButton, _passWidthConstraint);
            ConfigureButton(_confirmButton, _confirmPassWidthConstraint);

            _passwordTextField.SecureTextEntry = true;
            _confirmPassTextField.SecureTextEntry = true;

            UITextFieldExtensions.SetupField(this.View, 0, ViewModel.EmailAddressLabel, _emailTextField, _emailIndicatorView,_emailHeightConstraint,_emailIndicatorLabel, UIReturnKeyType.Next);
            UITextFieldExtensions.SetupField(this.View, 1, ViewModel.NewPassTitle, _passwordTextField, _passwordIndicatorView, _passHeightConstraint, _passwordIndicatorLabel, UIReturnKeyType.Next);
            UITextFieldExtensions.SetupField(this.View, 2, ViewModel.ConfirmPassLabel, _confirmPassTextField, _confirmPassIndicatorView, _confirmHeightConstraint, _confirmpassIndicatorLabel, UIReturnKeyType.Next);
            UITextFieldExtensions.SetupField(this.View, 3, ViewModel.CodeLabel, _codeTextField, _codeIndicatorView, _codeHeightConstraint, _codeIndicatorLabel, UIReturnKeyType.Default);

            _emailTextField.Enabled = false;
            _emailTextField.Text = ViewModel.Email;
            _emailIndicatorLabel.TextColor = Colors.GrayIndicator;
            _emailIndicatorLabel.Alpha = 1;

            _codeTextField.AutocorrectionType = UITextAutocorrectionType.No;
            _codeTextField.AutocapitalizationType = UITextAutocapitalizationType.AllCharacters;
            _codeTextField.TextContentType = new NSString("");
        }

        private void InvalidPassword()
        {
            _passwordIndicatorLabel.TextColor = Colors.Red;
            _passwordIndicatorView.BackgroundColor = Colors.Red;

            _confirmpassIndicatorLabel.TextColor = Colors.Red;
            _confirmPassIndicatorView.BackgroundColor = Colors.Red;
        }

        private void ConfigureButton(UIButton button, NSLayoutConstraint constraint)
        {
            button.SetNeedsLayout();
            button.LayoutIfNeeded();
            constraint.Constant = (UIScreen.MainScreen.Bounds.Width - 80) - (button.Frame.Width + 7);
        }

        private void Loading()
        {
            _lottieAnimation = UIViewAnimationExtensions.CustomButtomLoadingAnimation(_lottieAnimation, "loading_white", _submitButton, ViewModel.SubmitButton, ViewModel.IsSubmiting);
        }

        public override void OnKeyboardNotification(bool changeKeyboardState)
        {
            if (keyboardViewState != changeKeyboardState && ViewIsVisible)
            {
                keyboardViewState = changeKeyboardState;
                UIViewAnimationExtensions.AnimateBackgroundView(this.View, LocalConstants.RecoverPass_HeightAnimation, keyboardViewState);
                UIApplication.SharedApplication.SetStatusBarHidden(keyboardViewState, true);
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            _lottieAnimation?.Dispose();
            _lottieAnimation = null;
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
            base.ViewWillDisappear(animated);
        }
    }
}

