using System;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Division
{
    public partial class DivisionViewController : XViewController<DivisionViewModel>
    {
        public DivisionViewController() : base("DivisionViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}

