using System;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Login
{
    public partial class LoginViewController : XViewController<LoginViewModel>, IRootView
    {
        public LoginViewController() : base("LoginViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}

