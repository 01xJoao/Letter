using System;
using System.ComponentModel;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.ChangePassword
{
    public partial class ChangePasswordViewController : XViewController<ChangePasswordViewModel>
    {
        public override bool HandlesKeyboardNotifications => true;
        public ChangePasswordViewController() : base("ChangePasswordViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.IsBusy): 
                    Loading();
                    break;
                case nameof(ViewModel.CleanPassword):
                    CleanCurrentPassword();
                    break;
                default:
                    break;
            }
        }

        private void CleanCurrentPassword() 
        {
            _currentPassword.Text = string.Empty;
            this.View.EndEditing(false);
            _currentPassword.BecomeFirstResponder();
        }
        private void Loading()
        {
            if(ViewModel.IsBusy)
                this.View.EndEditing(true);
            
            UIViewAnimationExtensions.CustomButtomLoadingAnimation("loading_white", _submitButton, ViewModel.ChangePassword, ViewModel.IsBusy);
        }

        private void SetupView()
        {
            _view1.BackgroundColor = Colors.ProfileGrayWhiter;
            _view2.BackgroundColor = Colors.ProfileGrayWhiter;
            _view3.BackgroundColor = Colors.ProfileGrayWhiter;

            UITextFieldExtensions.SetupTextFieldAppearance(_currentPassword, Colors.Black, 15f, ViewModel.CurrentPass, Colors.GrayIndicator, Colors.MainBlue, Colors.White, returnKeyType: UIReturnKeyType.Next, view: this.View, tag: 0);
            UITextFieldExtensions.SetupTextFieldAppearance(_newPassword, Colors.Black, 15f, ViewModel.NewPassTitle, Colors.GrayIndicator, Colors.MainBlue, Colors.White, returnKeyType: UIReturnKeyType.Next, view: this.View, tag: 1);
            UITextFieldExtensions.SetupTextFieldAppearance(_confirmPassword, Colors.Black, 15f, ViewModel.NewPassAgainTitle, Colors.GrayIndicator, Colors.MainBlue, Colors.White, view: this.View, tag: 2);

            _currentPassword.SecureTextEntry = true;
            _newPassword.SecureTextEntry = true;
            _confirmPassword.SecureTextEntry = true;

            UIButtonExtensions.SetupButtonAppearance(_submitButton, Colors.White, 14f, ViewModel.ChangePassword);
            _backgroundButton.BackgroundColor = Colors.MainBlue;

            var keyboardButton1 = new UIButton();
            UIButtonExtensions.SetupButtonAppearance(keyboardButton1, Colors.White, 16f, ViewModel.ChangePassword);
            keyboardButton1.TouchUpInside -= OnSubmitButton_TouchUpInside;
            keyboardButton1.TouchUpInside += OnSubmitButton_TouchUpInside;

            var keyboardButton2 = new UIButton();
            UIButtonExtensions.SetupButtonAppearance(keyboardButton2, Colors.White, 16f, ViewModel.ChangePassword);
            keyboardButton2.TouchUpInside -= OnSubmitButton_TouchUpInside;
            keyboardButton2.TouchUpInside += OnSubmitButton_TouchUpInside;

            var keyboardButton3 = new UIButton();
            UIButtonExtensions.SetupButtonAppearance(keyboardButton3, Colors.White, 16f, ViewModel.ChangePassword);
            keyboardButton3.TouchUpInside -= OnSubmitButton_TouchUpInside;
            keyboardButton3.TouchUpInside += OnSubmitButton_TouchUpInside;

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;

            UITextFieldExtensions.AddViewToKeyboard(_currentPassword, keyboardButton1, Colors.MainBlue);
            UITextFieldExtensions.AddViewToKeyboard(_newPassword, keyboardButton2, Colors.MainBlue);
            UITextFieldExtensions.AddViewToKeyboard(_confirmPassword, keyboardButton3, Colors.MainBlue);
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.ChangePassCommand.CanExecute(null))
                ViewModel.ChangePassCommand.Execute(new Tuple<string, string, string>(_currentPassword.Text, _newPassword.Text, _confirmPassword.Text));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.Title = ViewModel.Title;
            NavigationController.NavigationBar.TintColor = Colors.White;
            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = Colors.Black };

            this.NavigationItem.LeftBarButtonItem = UIButtonExtensions.SetupImageBarButton(44, "back_black", CloseView);
            NavigationController.InteractivePopGestureRecognizer.Delegate = new UIGestureRecognizerDelegate();

            this.NavigationController.NavigationBar.BarTintColor = Colors.White;
            this.NavigationController.NavigationBar.Translucent = false;
            this.NavigationController.SetNavigationBarHidden(false, true);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
        }

        private void CloseView(object sender, EventArgs e)
        {
            if (ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (this.IsMovingFromParentViewController)
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

