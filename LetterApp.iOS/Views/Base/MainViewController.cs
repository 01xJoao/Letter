using UIKit;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.TabBar.CallListViewController;
using LetterApp.iOS.Views.TabBar.ChatListViewController;
using LetterApp.iOS.Views.TabBar.ContactListViewController;
using LetterApp.iOS.Views.TabBar.UserProfileViewController;
using LetterApp.iOS.Helpers;

namespace LetterApp.iOS.Views.Base
{
    public class MainViewController : XTabBarViewController<MainViewModel>, IRootView
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TabBar.Translucent = false;
            TabBar.Layer.MasksToBounds = false;
            TabBar.BackgroundImage = new UIImage();
            TabBar.ShadowImage = new UIImage();

            this.ViewControllers = new[]
            {
                CreateTabBar(new ChatListViewController(), ViewModel.ChatTab, "tabbar_chats", 0),
                CreateTabBar(new CallListViewController(), ViewModel.CallTab, "tabbar_call", 1),
                CreateTabBar(new ContactListViewController(), ViewModel.ContactTab, "tabbar_contacts", 2),
                CreateTabBar(new UserProfileViewController(), ViewModel.ProfileTab, "tabbar_profile", 3)
            };

            this.CustomizableViewControllers = null;
            this.SelectedViewController = ViewControllers[2];
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
            vc.TabBarItem.ImageInsets = new UIEdgeInsets(top: -2, left: 0, bottom: 0, right: 0);
            vc.TabBarItem.TitlePositionAdjustment = new UIOffset(0, -2);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
        }
    }
}
