using System;

using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class ChatAlertViewController : UIViewController
    {
        private string _picture;
        private string _name;
        private string _message;
        private Action<bool> _openChatAction;

        public ChatAlertViewController(string picture, string name, string message, Action<bool> openChatAction) : base("ChatAlertViewController", null)
        {
            _picture = picture;
            _name = name;
            _message = message;
            _openChatAction = openChatAction;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

        }
    }
}

