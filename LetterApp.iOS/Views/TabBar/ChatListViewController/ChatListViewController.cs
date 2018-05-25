using System;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Interfaces;
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
            this.NavigationController.NavigationBar.PrefersLargeTitles = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;
        }
    }
}

