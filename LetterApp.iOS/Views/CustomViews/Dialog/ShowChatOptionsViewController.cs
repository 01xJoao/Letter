using System;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class ShowChatOptionsViewController : UIViewController
    {
        private string _name;
        private string _photo;
        private string _email;
        private bool _muted;
        private string[] _resources;
        private Action<Tuple<ChatOptions, bool>> _buttonAction;

        public ShowChatOptionsViewController(string name, string photo, string email, bool muted, string[] resources, Action<Tuple<ChatOptions, bool>> buttonAction) 
            : base("ShowChatOptionsViewController", null)
        {
            _name = name;
            _photo = string.Copy(photo);
            _email = email;
            _muted = muted;
            _resources = resources;
            _buttonAction = buttonAction;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public void Show()
        {
            this.View.Frame = UIApplication.SharedApplication.KeyWindow.Bounds;
            UIApplication.SharedApplication.KeyWindow.AddSubview(this.View);
            UIView.Animate(0.3, () => View.Alpha = 1);
        }

        public void Dismiss()
        {
            UIView.AnimateNotify(0.3, () => View.Alpha = 0, (finished) => CleanFromMemory());
        }

        private void CleanFromMemory()
        {
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

