using System;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Chat
{
    public partial class ChatViewController : XViewController<ChatViewModel>
    {
        public ChatViewController() : base("ChatViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}

