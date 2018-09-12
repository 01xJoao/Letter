using System;
using Foundation;
using LetterApp.Core.Models;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.ChatListViewController.Cells
{
    public partial class ChatCell : UITableViewCell
    {
        private ChatListUserCellModel _chatUser;

        public static readonly NSString Key = new NSString("ChatCell");
        public static readonly UINib Nib = UINib.FromName("ChatCell", NSBundle.MainBundle);

        protected ChatCell(IntPtr handle) : base(handle){}

        public void Configure(ChatListUserCellModel chatUser)
        {
            _chatUser = chatUser;
        }
    }
}
