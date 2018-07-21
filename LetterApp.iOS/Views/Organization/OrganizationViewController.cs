using System;
using System.ComponentModel;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Organization
{
    public partial class OrganizationViewController : XViewController<OrganizationViewModel>
    {
        public OrganizationViewController() : base("OrganizationViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

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

        private void SetupView()
        {
            
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = Colors.White };
            var backButton = UIButtonExtensions.SetupImageBarButton(20, "back_white", CloseView);
            this.NavigationItem.LeftBarButtonItem = backButton;
            this.NavigationController.InteractivePopGestureRecognizer.Delegate = new UIGestureRecognizerDelegate();
            this.NavigationController.NavigationBar.BarTintColor = Colors.MainBlue;
            this.NavigationController.NavigationBar.Translucent = false;
            this.NavigationController.SetNavigationBarHidden(false, true);
            this.NavigationController.NavigationBar.ShadowImage = new UIImage();

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
        }

        private void CloseView(object sender, EventArgs e)
        {
            if (ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        public override void ViewDidDisappear(bool animated)
        {
            if (this.IsMovingFromParentViewController)
            {
                base.ViewDidDisappear(animated);
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
            }
        }
    }
}

