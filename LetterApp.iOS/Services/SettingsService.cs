using System;
using LetterApp.Core.Services.Interfaces;
using UIKit;

namespace LetterApp.iOS.Services
{
    public class SettingsService : ISettingsService
    {
        public void Logout()
        {
            using (var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate)
            {
                appDelegate.UnregisterTokens();
            }
        }
    }
}
