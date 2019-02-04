using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Loading
{
    public partial class LoadingViewController : XViewController<LoadingViewModel>, IRootView
    {
        private UIView _loadingView = new UIView();

        public LoadingViewController() : base("LoadingViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var userInfo = new NSDictionary("userId", AppSettings.UserAndOrganizationIds);
            NSNotificationCenter.DefaultCenter.PostNotificationName("UserDidLoginNotification", null, userInfo);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            _loadingView.Hidden = true;
            _loadingView.Frame = new CGRect(0, 0, 50, 50);
            var imageLoadingCenter = _loadingView.Center;
            imageLoadingCenter.X = this.View.Bounds.GetMidX();
            imageLoadingCenter.Y = this.View.Frame.Height / 1.25f;
            _loadingView.Center = imageLoadingCenter;

            this.View.AddSubview(_loadingView);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            LoadingAnimation(true);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            LoadingAnimation(false);
        }

        private void LoadingAnimation(bool isLoading)
        {
            UIViewAnimationExtensions.LoadingInView("load_blue", _loadingView, isLoading);
        }
    }
}

