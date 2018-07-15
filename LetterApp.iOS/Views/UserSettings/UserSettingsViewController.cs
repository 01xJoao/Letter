using System;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.UserSettings
{
    public partial class UserSettingsViewController : XViewController<UserSettingsViewModel>
    {
        public UserSettingsViewController() : base("UserSettingsViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}

