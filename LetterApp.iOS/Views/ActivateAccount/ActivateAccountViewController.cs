using System;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.ActivateAccount
{
    public partial class ActivateAccountViewController : XViewController<ActivateAccountViewModel>
    {
        public override bool HandlesKeyboardNotifications => true;
        public override bool ShowAsPresentView => true;

        public ActivateAccountViewController() : base("ActivateAccountViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewDidDisappear(bool animated)
        {
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
            base.ViewDidDisappear(animated);
        }
    }
}

