using System;
using Airbnb.Lottie;
using CoreGraphics;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.iOS.Interfaces;
using UIKit;

namespace LetterApp.iOS.Views.Base
{
    public abstract class XViewController<TViewModel> : UIViewController, IXiOSView where TViewModel : class, IXViewModel
    {
        public TViewModel ViewModel { get; private set; }
        public object ParameterData { get; set; }
        public virtual bool ShowAsPresentView => false;
        public LOTAnimationView LoadAnimation;

        public XViewController(string nibName, NSBundle bundle) : base(nibName, bundle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel = App.Container.GetInstance<TViewModel>();

            if (ParameterData != null)
                ViewModel.Prepare(ParameterData);
            else
                ViewModel.Prepare();

            if (HandlesKeyboardNotifications)
                RegisterForKeyboardNotifications();

            LoadingAnimation();
            ViewModel.InitializeViewModel();
            DismissKeyboardOnBackgroundTap();


            this.EdgesForExtendedLayout = UIRectEdge.None;
        }

        private void LoadingAnimation()
        {
            LoadAnimation = LOTAnimationView.AnimationNamed("loading_white");
            LoadAnimation.ContentMode = UIViewContentMode.ScaleAspectFit;
            LoadAnimation.LoopAnimation = true;
            LoadAnimation.Hidden = true;
        }

        public override void ViewWillAppear(bool animated)
        {
            ViewModel.Appearing();
            base.ViewWillAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            ViewModel.Disappearing();
            base.ViewWillDisappear(animated);
        }

        #region Keyboard

        public virtual bool HandlesKeyboardNotifications => false;

        protected void RegisterForKeyboardNotifications()
        {
            UIKeyboard.Notifications.ObserveWillHide((sender, e) => OnKeyboardNotification(e));
            UIKeyboard.Notifications.ObserveWillShow((sender, e) => OnKeyboardNotification(e));
        }

        public virtual void OnKeyboardNotification(UIKeyboardEventArgs args)
        {
            if (!this.IsViewLoaded)
                return;
        }

        protected void DismissKeyboardOnBackgroundTap()
        {
            var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
            tap.AddTarget(() => View.EndEditing(true));
            View.AddGestureRecognizer(tap);
        }


        #endregion
    }
}
