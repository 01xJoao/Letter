using System;
using System.ComponentModel;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.Base;

namespace LetterApp.iOS.Views.Loading
{
    public partial class LoadingViewController : XViewController<LoadingViewModel>, IRootView
    {
        public LoadingViewController() : base("LoadingViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var userInfo = new NSDictionary("userId", AppSettings.UserId.ToString());
            NSNotificationCenter.DefaultCenter.PostNotificationName("UserDidLoginNotification", null, userInfo);
        }
    }
}

