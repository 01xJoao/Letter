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
            this.Title = ViewModel.Title;
            this.NavigationController.NavigationBar.PrefersLargeTitles = true;
            this.NavigationItem.RightBarButtonItem = UIButtonExtensions.SetupImageBarButton(20f, "new_call", OpenContacts);
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
        }

        private void OpenContacts(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;
        }
    }
}

