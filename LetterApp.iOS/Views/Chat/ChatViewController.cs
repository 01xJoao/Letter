using System;
using System.ComponentModel;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Chat
{
    public partial class ChatViewController : XViewController<ChatViewModel>
    {
        public ChatViewController() : base("ChatViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ConfigureView();
            ConfigureTableView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                default:
                    break;
            }
        }

        private void ConfigureView()
        {
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
        }

        private void ConfigureTableView()
        {

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var titleViewMaxSize = UIScreen.MainScreen.Bounds.Width - 165; //165 is the size and icons space

            this.NavigationItem.TitleView = CustomUIExtensions.SetupNavigationBarWithSubtitle("João Palma", "Instituto Politecnico de Viana do Castelo - Escola Superior TG", titleViewMaxSize);
            this.NavigationController.NavigationBar.TintColor = Colors.MainBlue;
            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = Colors.White };
            this.NavigationItem.LeftBarButtonItem = UIButtonExtensions.SetupImageBarButton(44, "back_white", CloseView);

            var optionsItem = UIButtonExtensions.SetupImageBarButton(44, "options", Options, false);
            var callItem = UIButtonExtensions.SetupImageBarSecondButton(44, "call_white_medium", CallUser);
            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[] { optionsItem, callItem };

            this.NavigationController.InteractivePopGestureRecognizer.Delegate = new UIGestureRecognizerDelegate();
            this.NavigationController.NavigationBar.BarTintColor = Colors.MainBlue;
            this.NavigationController.NavigationBar.Translucent = false;
            this.NavigationController.SetNavigationBarHidden(false, true);
            this.NavigationController.NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            this.NavigationController.NavigationBar.ShadowImage = new UIImage();
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.BlackOpaque;
            _navBarView.BackgroundColor = Colors.MainBlue;
        }

        private void Options(object sender, EventArgs e)
        {
        }

        private void CloseView(object sender, EventArgs e)
        {
            ViewModel.CloseViewCommand.Execute();
        }

        private void CallUser(object sender, EventArgs e)
        {
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (this.IsMovingFromParentViewController)
                this.NavigationController.SetNavigationBarHidden(true, true);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (this.IsMovingFromParentViewController)
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

