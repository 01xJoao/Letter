using System;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Login
{
    public partial class LoginViewController : XViewController<LoginViewModel>
    {
        public LoginViewController() : base("LoginViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
}

