﻿using System;
using System.ComponentModel;
using System.Linq;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Login
{
    public partial class LoginViewController : XViewController<LoginViewModel>, IRootView
    {
        public override bool HandlesKeyboardNotifications => true;
        private bool keyboardViewState;

        public LoginViewController() : base("LoginViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            _forgotPassButton.TouchUpInside -= OnforgotPassButton_TouchUpInside;
            _forgotPassButton.TouchUpInside += OnforgotPassButton_TouchUpInside;

            _signUpButton.TouchUpInside -= OnSignUpButton_TouchUpInside;
            _signUpButton.TouchUpInside += OnSignUpButton_TouchUpInside;

            _signInButton.TouchUpInside -= OnSignInButton_TouchUpInside;
            _signInButton.TouchUpInside += OnSignInButton_TouchUpInside;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.IsValidEmail):
                    InvalidMail();
                    break;
                case nameof(ViewModel.IsSigningIn):
                    Loading();
                    break;
                default:
                    break;
            }
        }

        private void OnforgotPassButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.ForgotPassCommand.CanExecute(null))
                ViewModel.ForgotPassCommand.Execute(_emailTextField.Text);
        }

        private void OnSignUpButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.OpenRegisterViewCommand.CanExecute())
                ViewModel.OpenRegisterViewCommand.Execute();
        }

        private void OnSignInButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.SignInCommand.CanExecute(new Tuple<string, string>(_emailTextField.Text, _passwordTextField.Text)))
                ViewModel.SignInCommand.Execute(new Tuple<string, string>(_emailTextField.Text, _passwordTextField.Text));
            
            this.View.EndEditing(true);
        }

        private void SetupView()
        {
            if (PhoneModelExtensions.IsIphoneX())
                _buttonHeightConstraint.Constant += UIApplication.SharedApplication.KeyWindow.SafeAreaInsets.Bottom;

            UIButtonExtensions.SetupButtonAppearance(_signUpButton, Colors.MainBlue, 14f, ViewModel.SignUpButton);
            UIButtonExtensions.SetupButtonAppearance(_signInButton, Colors.White, 16f, ViewModel.SignInButton);
            UIButtonExtensions.SetupButtonAppearance(_forgotPassButton, Colors.MainBlue, 12f, ViewModel.ForgotPasswordButton);
            _forgotPassButton.LayoutIfNeeded();

            UIButton keyboardButton = new UIButton();
            UIButtonExtensions.SetupButtonAppearance(keyboardButton, Colors.White, 16f, ViewModel.SignInButton);
            keyboardButton.TouchUpInside -= OnSignInButton_TouchUpInside;
            keyboardButton.TouchUpInside += OnSignInButton_TouchUpInside;

            _emailTextField.KeyboardType = UIKeyboardType.EmailAddress;
            UITextFieldExtensions.SetupField(this.View, 0, ViewModel.EmailLabel, _emailTextField, _emailLineView, _emailHeightConstraint, _emailLabel, UIReturnKeyType.Next, keyboardButton);
            
            _passwordTextField.SecureTextEntry = true;
            _passwordWithConstraint.Constant = (ScreenWidth - 80) - (_forgotPassButton.Frame.Width + 7);
            UITextFieldExtensions.SetupField(this.View, 1, ViewModel.PasswordLabel, _passwordTextField, _passwordLineView, _passwordHeightConstraint, _passwordLabel, UIReturnKeyType.Default, keyboardButton);

            _emailTextField.KeyboardType = UIKeyboardType.EmailAddress;
            _emailTextField.AutocorrectionType = UITextAutocorrectionType.No;

            if(!string.IsNullOrEmpty(ViewModel.UserEmail))
            {
                _emailTextField.Text = ViewModel.UserEmail;
                _emailLabel.Alpha = 1;
            }
        }

        private void Loading()
        {
             UIViewAnimationExtensions.CustomButtomLoadingAnimation("load_white", _signInButton, ViewModel.SignInButton, ViewModel.IsSigningIn);
        }

        private void InvalidMail()
        {
            _emailLabel.TextColor = Colors.Red;  
            _emailLineView.BackgroundColor = Colors.Red;  
        } 

        public override void OnKeyboardNotification(UIKeyboardEventArgs keybordEvent, bool changeKeyboardState)
        {
            if (ShouldAnimateView() && keyboardViewState != changeKeyboardState)
            {
                keyboardViewState = changeKeyboardState;
                UIViewAnimationExtensions.AnimateView(this.View, LocalConstants.Login_HeightAnimation, keyboardViewState);
            }
        }

        private bool ShouldAnimateView()
        {
            var viewsInScreen = UIApplication.SharedApplication.KeyWindow.Subviews;

            return (viewsInScreen.Length == 1 || viewsInScreen.Last().Frame != this.View.Frame) && ViewIsVisible;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (NavigationController?.InteractivePopGestureRecognizer != null)
                NavigationController.InteractivePopGestureRecognizer.Enabled = false;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            _passwordLabel.Alpha = 0;
            _passwordTextField.Text = string.Empty;

            if (NavigationController?.InteractivePopGestureRecognizer != null)
                NavigationController.InteractivePopGestureRecognizer.Enabled = true;
        }
    }
}

