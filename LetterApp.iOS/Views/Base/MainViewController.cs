using UIKit;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.TabBar.CallListViewController;
using LetterApp.iOS.Views.TabBar.ChatListViewController;
using LetterApp.iOS.Views.TabBar.ContactListViewController;
using LetterApp.iOS.Views.TabBar.UserProfileViewController;
using LetterApp.iOS.Helpers;
using LetterApp.Core;
using Foundation;
using System;

namespace LetterApp.iOS.Views.Base
{
    public class MainViewController : XTabBarViewController<MainViewModel>, IRootView
    {
        //private NSObject _willEnterForeGround;
        //private NSObject _didEnterBackGround;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TabBar.Translucent = false;
            TabBar.Layer.MasksToBounds = false;
            TabBar.BackgroundImage = new UIImage();
            TabBar.ShadowImage = new UIImage();

            //_willEnterForeGround = UIApplication.Notifications.ObserveWillEnterForeground(ConnectToService);
            //_didEnterBackGround = UIApplication.Notifications.ObserveDidEnterBackground(DisconnectFromService);

            this.ViewControllers = new[]
            {
                CreateTabBar(new ChatListViewController(), ViewModel.ChatTab, "tabbar_chats", 0),
                CreateTabBar(new CallListViewController(), ViewModel.CallTab, "tabbar_call", 1),
                CreateTabBar(new ContactListViewController(), ViewModel.ContactTab, "tabbar_contacts", 2),
                CreateTabBar(new UserProfileViewController(), ViewModel.ProfileTab, "tabbar_profile", 3)
            };

            this.CustomizableViewControllers = null;
        }

        //private void ConnectToService(object sender, NSNotificationEventArgs e)
        //{
        //    ViewModel.MessengerServiceCommand.Execute(true);
        //}

        //private void DisconnectFromService(object sender, NSNotificationEventArgs e)
        //{
        //    ViewModel.MessengerServiceCommand.Execute(false);
        //}

        public void SetVisibleView(int index)
        {
            this.SelectedViewController = this.ViewControllers[index];
        }

        private UIViewController CreateTabBar(UIViewController vc, string title, string imageName, int position)
        {
            var navigationController = new UINavigationController();
            SetTabBarItem(vc, title, imageName, position);
            navigationController.PushViewController(vc, false);
            return navigationController;
        }

        private void SetTabBarItem(UIViewController vc, string title, string imageName, int position)
        {
            vc.TabBarItem = new UITabBarItem(title, UIImage.FromBundle(imageName), position);
            vc.TabBarItem.Image = UIImage.FromBundle(imageName).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            vc.TabBarItem.SelectedImage = UIImage.FromBundle($"{imageName}_selected").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            vc.TabBarItem.SetTitleTextAttributes(new UITextAttributes { TextColor = Colors.TabBarNotSelected }, UIControlState.Normal);
            vc.TabBarItem.SetTitleTextAttributes(new UITextAttributes { TextColor = Colors.MainBlue }, UIControlState.Selected);

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                vc.TabBarItem.ImageInsets = new UIEdgeInsets(top: -2, left: 0, bottom: 0, right: 0);
                vc.TabBarItem.TitlePositionAdjustment = new UIOffset(0, -2);
            }

            if (position == 0 && AppSettings.BadgeForChat > 0)
                vc.TabBarItem.BadgeValue = AppSettings.BadgeForChat.ToString();

            if (position == 1 && AppSettings.BadgeForCalls > 0)
                vc.TabBarItem.BadgeValue = AppSettings.BadgeForCalls.ToString();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            //if (this.IsMovingFromParentViewController)
            //{
            //    _willEnterForeGround?.Dispose();
            //    _didEnterBackGround?.Dispose();
            //}
        }
    }
}
