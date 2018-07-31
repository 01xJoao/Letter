using System;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Member
{
    public partial class MemberViewController : XViewController<MemberViewModel>
    {
        public MemberViewController() : base("MemberViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }
    }
}

