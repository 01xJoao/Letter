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
            _source = new RegisterSource(_tableView, _backgroundView, ViewModel.FormModelList, ViewModel.AgreementLabel);
            _tableView.BackgroundColor = Colors.MainBlue4;
            _tableView.Source = _source;
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _tableView.ReloadData();

            _source.AgreementToogleEvent -= OnSource_AgreementToogleEvent;
            _source.AgreementToogleEvent += OnSource_AgreementToogleEvent;
        }

        private void OnSource_AgreementToogleEvent(object sender, bool userAgreed)
        {
            ViewModel.AgreementToogleCommand.Execute(userAgreed);
        }

        private void Loading()
        {
            UIViewAnimationExtensions.CustomButtomLoadingAnimation("load_white", _submitButton, ViewModel.SubmitButton, ViewModel.IsBusy);
        }

        public override void OnKeyboardNotification(UIKeyboardEventArgs keybordEvent, bool changeKeyboardState)
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

            this.Title = ViewModel.Title;
            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = Colors.Black };

            this.NavigationItem.LeftBarButtonItem = UIButtonExtensions.SetupImageBarButton(LocalConstants.TabBarIconSize, "back_black", CloseView);
            this.NavigationController.InteractivePopGestureRecognizer.Delegate = new UIGestureRecognizerDelegate();
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

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            //if (NavigationController?.InteractivePopGestureRecognizer != null)
                //NavigationController.InteractivePopGestureRecognizer.Enabled = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.NavigationController?.SetNavigationBarHidden(true, true);
        }

        public override void ViewDidDisappear(bool animated)
        {
            _source?.Dispose();
            _source = null;
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
            base.ViewDidDisappear(animated);
        }
    }
}

