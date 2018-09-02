using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.ChatListViewController
{
    public partial class ChatListViewController : XViewController<ChatListViewModel>
    {

        public ChatListViewController() : base("ChatListViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
        }

        private void ConfigureView()
        {
            this.Title = "Chats";

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                this.NavigationController.NavigationBar.PrefersLargeTitles = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;
        }
    }
}

