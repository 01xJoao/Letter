using System;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.ChangePassword
{
    public partial class ChangePasswordViewController : XViewController<ChangePasswordViewModel>
    {
        public ChangePasswordViewController() : base("ChangePasswordViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.Title = "Change Password";
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

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (this.IsMovingFromParentViewController)
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

