using System;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.UserProfileViewController
{
    public partial class UserProfileViewController : XViewController<UserProfileViewModel>
    {
        public UserProfileViewController() : base("UserProfileViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
        }

        private void ConfigureView()
        {
            this.NavigationController.NavigationBarHidden = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;
        }
    }
}

