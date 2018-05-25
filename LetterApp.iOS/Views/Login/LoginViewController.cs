﻿using System;
using CoreGraphics;
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

        public LoginViewController() : base("LoginViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView();

            _forgotPassButton.TouchUpInside -= OnforgotPassButton_TouchUpInside;
            _forgotPassButton.TouchUpInside += OnforgotPassButton_TouchUpInside;

            _signUpButton.TouchUpInside -= OnSignUpButton_TouchUpInside;
            _signUpButton.TouchUpInside += OnSignUpButton_TouchUpInside;

            _signInButton.TouchUpInside -= OnSignInButton_TouchUpInside;
            _signInButton.TouchUpInside += OnSignInButton_TouchUpInside;
        }

        private void OnforgotPassButton_TouchUpInside(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnSignUpButton_TouchUpInside(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnSignInButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.SignInCommand.CanExecute(new Tuple<string, string>(_emailTextField.Text, _passwordTextField.Text)))
                ViewModel.SignInCommand.Execute(new Tuple<string, string>(_emailTextField.Text, _passwordTextField.Text));
            
            View.EndEditing(true);
        }

        private void SetupView()
        {
            UIButtonExtensions.SetupButtonAppearance(_signUpButton, Colors.MainBlue, 14f, ViewModel.SignUpButton);
            UIButtonExtensions.SetupButtonAppearance(_signInButton, Colors.White, 16f, ViewModel.SignInButton);
            UIButtonExtensions.SetupButtonAppearance(_forgotPassButton, Colors.MainBlue, 13f, ViewModel.ForgotPasswordButton);

            UIButton keyboardButton = new UIButton();
            UIButtonExtensions.SetupButtonAppearance(keyboardButton, Colors.White, 16f, ViewModel.SignInButton);
            keyboardButton.TouchUpInside -= OnSignInButton_TouchUpInside;
            keyboardButton.TouchUpInside += OnSignInButton_TouchUpInside;

            _emailTextField.AutocorrectionType = UITextAutocorrectionType.No;
            UITextFieldExtensions.SetupField(this.View, 0, ViewModel.EmailLabel, _emailTextField, _emailLineView, _emailHeightConstraint, _emailLabel,
                                             UIReturnKeyType.Next, keyboardButton);
            
            _forgotPassButton.SetNeedsLayout();
            _forgotPassButton.LayoutIfNeeded();

            _passwordTextField.SecureTextEntry = true;
            _passwordWithConstraint.Constant = (UIScreen.MainScreen.Bounds.Width - 80) - (_forgotPassButton.Frame.Width + 7);
            UITextFieldExtensions.SetupField(this.View, 1, ViewModel.PasswordLabel, _passwordTextField, _passwordLineView, _passwordHeightConstraint, _passwordLabel, 
                                             UIReturnKeyType.Default, keyboardButton);
        }

        public override void OnKeyboardNotification(UIKeyboardEventArgs args)
        {
            base.OnKeyboardNotification(args);

            if (args.FrameEnd.Y < args.FrameBegin.Y)
                Animations.AnimateBackground(this.View, LocalConstants.Login_HeightAnimation);
            else if ( args.FrameEnd.Y > args.FrameBegin.Y)
                Animations.AnimateBackground(this.View, -LocalConstants.Login_HeightAnimation);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
        }
    }
}
