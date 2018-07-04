using System;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.PendingApproval
{
    public partial class PendingApprovalViewController : XViewController<PendingApprovalViewModel>, IRootView
    {
        public PendingApprovalViewController() : base("PendingApprovalViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}

