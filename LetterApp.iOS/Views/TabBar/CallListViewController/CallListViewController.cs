using System;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.CallListViewController
{
    public partial class CallListViewController : XViewController<CallListViewModel>
    {
        public CallListViewController() : base("CallListViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
        }

        private void ConfigureView()
        {
            this.Title = "Calls";
            this.NavigationController.NavigationBar.PrefersLargeTitles = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;
        }
    }
}

