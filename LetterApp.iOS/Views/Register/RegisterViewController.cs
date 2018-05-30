using System;
using LetterApp.Core.Models;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Register
{
    public partial class RegisterViewController : XViewController<RegisterViewModel>
    {
        public RegisterViewController() : base("RegisterViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
          
            SetupView();
            SetupTableView();

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;
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
            var source = new RegisterSource(_tableView, ViewModel.LocationResources, ViewModel.RegisterForm, this.View);
            _tableView.BackgroundColor = Colors.MainBlue4;
            _tableView.Source = source;
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _tableView.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.Title = ViewModel.Title;
            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = Colors.Black };
            this.NavigationController.NavigationBar.TopItem.Title = string.Empty;
            this.NavigationController.NavigationBar.TintColor = Colors.Black;
            this.NavigationController.NavigationBar.BarTintColor = Colors.White;
            this.NavigationController.NavigationBar.Translucent = false;
            this.NavigationController.SetNavigationBarHidden(false, true);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.NavigationController.SetNavigationBarHidden(true, true);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            MemoryUtility.ReleaseUITableViewCell(_tableView);
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

