using System;
using System.Linq;
using Airbnb.Lottie;
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

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnRequestCodeButton_TouchUpInside(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
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

            UIButtonExtensions.SetupButtonAppearance(_requestCodeButton, Colors.MainBlue, 13f, ViewModel.RequestAgainButton);
            UIButtonExtensions.SetupButtonAppearance(_submitButton, Colors.White, 16f, ViewModel.SubmitButton);
            UIButtonExtensions.SetupButtonAppearance(_passButton, Colors.MainBlue, 13f, ViewModel.ShowButton);
            UIButtonExtensions.SetupButtonAppearance(_confirmButton, Colors.MainBlue, 13f, ViewModel.ShowButton);

            ConfigureButton(_requestCodeButton, _codeWidthConstraint);
            ConfigureButton(_passButton, _passWidthConstraint);
            ConfigureButton(_confirmButton, _confirmPassWidthConstraint);

            _passwordTextField.SecureTextEntry = true;
            _confirmPassTextField.SecureTextEntry = true;

            UITextFieldExtensions.SetupField(this.View, 0, ViewModel.EmailAddressLabel, _emailTextField, _emailIndicatorView,_emailHeightConstraint,_emailIndicatorLabel, UIReturnKeyType.Next);
            UITextFieldExtensions.SetupField(this.View, 1, ViewModel.NewPassTitle, _passwordTextField, _passwordIndicatorView, _passHeightConstraint, _passwordIndicatorLabel, UIReturnKeyType.Next);
            UITextFieldExtensions.SetupField(this.View, 2, ViewModel.ConfirmPassLabel, _confirmPassTextField, _confirmPassIndicatorView, _confirmHeightConstraint, _confirmpassIndicatorLabel, UIReturnKeyType.Next);
            UITextFieldExtensions.SetupField(this.View, 3, ViewModel.CodeLabel, _codeTextField, _codeIndicatorView, _codeHeightConstraint, _codeIndicatorLabel, UIReturnKeyType.Default);

            _emailTextField.AutocorrectionType = UITextAutocorrectionType.No;
            _codeTextField.AutocorrectionType = UITextAutocorrectionType.No;
            _codeTextField.SmartDashesType = UITextSmartDashesType.No;
            _codeTextField.SmartQuotesType = UITextSmartQuotesType.No;
            _codeTextField.SpellCheckingType = UITextSpellCheckingType.No;
            _codeTextField.AutocapitalizationType = UITextAutocapitalizationType.AllCharacters;
        }

        private void ConfigureButton(UIButton button, NSLayoutConstraint constraint)
        {
            constraint.Constant = (UIScreen.MainScreen.Bounds.Width - 80) - (button.Frame.Width + 7);
            button.SetNeedsLayout();
            button.LayoutIfNeeded();
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

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }
    }
}

