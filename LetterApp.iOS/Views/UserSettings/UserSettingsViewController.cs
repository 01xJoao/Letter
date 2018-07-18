using System;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.UserSettings
{
    public partial class UserSettingsViewController : XViewController<UserSettingsViewModel>
    {
        public UserSettingsViewController() : base("UserSettingsViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupTableView();
        }

        private void SetupTableView()
        {
            _tableView.BackgroundColor = Colors.MainBlue4;
            _tableView.Source = new UserSettingsSource(_tableView, ViewModel.PhoneModel, ViewModel.AllowCallsModel, ViewModel.TypeModelPassword, ViewModel.SwitchModel, ViewModel.TypeModelInformation,
                                                       ViewModel.TypeModelDanger, ViewModel.LocationResources);
            _tableView.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.Title = ViewModel.SettingsTitle;
            NavigationController.NavigationBar.TintColor = Colors.White;
            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = Colors.Black };

            var backButton = UIButtonExtensions.SetupImageBarButton(20, "back_black", CloseView);
            this.NavigationItem.LeftBarButtonItem = backButton;
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

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if(this.IsMovingFromParentViewController)
            {
                this.NavigationController.SetNavigationBarHidden(true, true);
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            if(this.IsMovingFromParentViewController)
            {
                base.ViewDidDisappear(animated);
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
            }
        }
    }
}

