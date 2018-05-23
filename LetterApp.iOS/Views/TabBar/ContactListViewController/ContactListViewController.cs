using System;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.ContactListViewController
{
    public partial class ContactListViewController : XViewController<ContactListViewModel>
    {
        public ContactListViewController() : base("ContactListViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
        }

        private void ConfigureView()
        {
            this.Title = "Contacts";
            this.NavigationController.NavigationBar.PrefersLargeTitles = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;
        }
    }
}

