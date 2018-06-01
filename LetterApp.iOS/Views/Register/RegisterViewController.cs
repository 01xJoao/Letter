using System;
using System.ComponentModel;
using Airbnb.Lottie;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using ObjCRuntime;
using UIKit;

namespace LetterApp.iOS.Views.Register
{
    public partial class RegisterViewController : XViewController<RegisterViewModel>
    {
        public override bool HandlesKeyboardNotifications => true;
        private LOTAnimationView _lottieAnimation;
        private bool _keyboardViewState;
        private RegisterSource _source;

        public RegisterViewController() : base("RegisterViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
          
            SetupView();
            SetupTableView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.IsBusy):
                    Loading();
                    break;
                default:
                    break;
            }
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.CreateAccountCommand.CanExecute())
                ViewModel.CreateAccountCommand.Execute();
        }

        private void SetupView()
        {
            _buttonView.BackgroundColor = Colors.MainBlue;
            UIButtonExtensions.SetupButtonAppearance(_submitButton, Colors.White, 16f, ViewModel.SubmitButton);
        }

        private void SetupTableView()
        {
            _source = new RegisterSource(_tableView, ViewModel.LocationResources, ViewModel.RegisterForm, _backgroundView);
            _tableView.BackgroundColor = Colors.MainBlue4;
            _tableView.Source = _source;
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _tableView.ReloadData();
        }

        private void Loading()
        {
            _lottieAnimation = UIViewAnimationExtensions.CustomButtomLoadingAnimation(_lottieAnimation, "loading_white", _submitButton, ViewModel.SubmitButton, ViewModel.IsBusy);
        }

        public override void OnKeyboardNotification(bool changeKeyboardState)
        {
            if (_keyboardViewState != changeKeyboardState && ViewIsVisible)
            {
                _keyboardViewState = changeKeyboardState;

                if(!_keyboardViewState)
                {
                    UIViewAnimationExtensions.AnimateBackgroundView(_backgroundView, 0, _keyboardViewState);
                    _source.IsAnimated = false;
                }
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //TODO: Maybe Change this to BaseViewController

            this.Title = ViewModel.Title;
            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = Colors.Black };

            var backButton = UIButtonExtensions.SetupImageBarButton(20, "back_black", CloseView);
            this.NavigationItem.LeftBarButtonItem = backButton;
            NavigationController.InteractivePopGestureRecognizer.Delegate = new UIGestureRecognizerDelegate();

            //this.NavigationController.NavigationBar.TintColor = Colors.Black;
            this.NavigationController.NavigationBar.BarTintColor = Colors.White;
            this.NavigationController.NavigationBar.Translucent = false;
            this.NavigationController.SetNavigationBarHidden(false, true);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
        }

        private void CloseView(object sender, EventArgs e)
        {
            if(ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.NavigationController.SetNavigationBarHidden(true, true);
        }

        public override void ViewDidDisappear(bool animated)
        {
            _lottieAnimation?.Dispose();
            _lottieAnimation = null;
            MemoryUtility.ReleaseUITableViewCell(_tableView);
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
            base.ViewDidDisappear(animated);
        }
    }
}

