﻿using System;
using System.ComponentModel;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.SelectOrganization
{
    public partial class SelectOrganizationViewController : XViewController<SelectOrganizationViewModel>, IRootView
    {
        public SelectOrganizationViewController() : base("SelectOrganizationViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;
                
            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;

            _createOrgButton.TouchUpInside -= OnCreateOrgButton_TouchUpInside;
            _createOrgButton.TouchUpInside += OnCreateOrgButton_TouchUpInside;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.IsBusy):
                    Loading();
                    break;
                case nameof(ViewModel.RegisterUser):
                    var userInfo = new NSDictionary("userId", AppSettings.UserAndOrganizationIds);
                    NSNotificationCenter.DefaultCenter.PostNotificationName("UserDidLoginNotification", null, userInfo);
                    break;
                default:
                    break;
            }
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.AccessOrgCommand.CanExecute(_textField.Text))
                ViewModel.AccessOrgCommand.Execute(_textField.Text);
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        private void Loading()
        {
            UIViewAnimationExtensions.CustomButtomLoadingAnimation("load_blue", _submitButton, ViewModel.AccessButton, ViewModel.IsBusy);
        }

        private void SetupView()
        {
            if (PhoneModelExtensions.IsIphoneX())
                _buttonHeightConstraint.Constant += UIApplication.SharedApplication.KeyWindow.SafeAreaInsets.Bottom;

            _closeButton.SetImage(UIImage.FromBundle("close_white"), UIControlState.Normal);
            _closeButton.TintColor = Colors.White;
            _backgroundView.BackgroundColor = Colors.SelectBlue;
            _buttonBackgroundView.BackgroundColor = Colors.White;

            UIButton keyboardButton = new UIButton();
            UIButtonExtensions.SetupButtonAppearance(keyboardButton, Colors.SelectBlue, 16f, ViewModel.AccessButton);
            keyboardButton.TouchUpInside -= OnSignInButton_TouchUpInside;
            keyboardButton.TouchUpInside += OnSignInButton_TouchUpInside;

            UILabelExtensions.SetupLabelAppearance(_label, ViewModel.TitleLabel, Colors.White, 24f, UIFontWeight.Thin);
            UIButtonExtensions.SetupButtonAppearance(_submitButton, Colors.SelectBlue, 16f, ViewModel.AccessButton);
            UITextFieldExtensions.SetupTextFieldAppearance(_textField, Colors.White, 26f, ViewModel.OrganizationHint, Colors.White70, Colors.White, Colors.SelectBlue, UIFontWeight.Medium);
            UITextFieldExtensions.AddViewToKeyboard(_textField, keyboardButton, Colors.White);
            _textField.AutocorrectionType = UITextAutocorrectionType.No;
            _textField.TextContentType = new NSString("");

            UIButtonExtensions.SetupButtonUnderlineAppearance(_createOrgButton, Colors.White, 15f, ViewModel.CreateOrganization);

            if (!string.IsNullOrEmpty(ViewModel.EmailDomain))
                _textField.Text = ViewModel.EmailDomain.ToUpper();
        }

        private void OnSignInButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.AccessOrgCommand.CanExecute(_textField.Text))
            {
                ViewModel.AccessOrgCommand.Execute(_textField.Text);
                _textField.ResignFirstResponder();
            }
        }

        private void OnCreateOrgButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.CreateOrganizationCommand.Execute();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (NavigationController?.InteractivePopGestureRecognizer != null)
                NavigationController.InteractivePopGestureRecognizer.Enabled = false;
        }
    }
}

