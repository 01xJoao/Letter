using System;
using System.ComponentModel;
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

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            SetupTableView();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.UpdateView):
                    SetupTableView();
                    break;
                default:
                    break;
            }
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

            this.NavigationItem.LeftBarButtonItem = UIButtonExtensions.SetupImageBarButton(20, "back_black", CloseView);
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
            base.ViewDidDisappear(animated);

            if(this.IsMovingFromParentViewController)
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

