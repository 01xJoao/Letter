﻿using System;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.iOS.Interfaces;
using UIKit;

namespace LetterApp.iOS.Views.Base
{
    public abstract class XViewController<TViewModel> : UIViewController, IXiOSView where TViewModel : class, IXViewModel
    {
        public bool ViewIsVisible { get; set; }
        public TViewModel ViewModel { get; private set; }
        public object ParameterData { get; set; }
        public virtual bool ShowAsPresentView => false;

        public XViewController(string nibName, NSBundle bundle) : base(nibName, bundle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel = App.Container.GetInstance<TViewModel>();

            if (ParameterData != null)
                ViewModel.Prepare(ParameterData);
            else
                ViewModel.Prepare();

            ViewModel.InitializeViewModel();
           
            this.EdgesForExtendedLayout = UIRectEdge.None;
        }

        public override void ViewWillAppear(bool animated)
        {
            ViewModel.Appearing();
            ViewIsVisible = true;

            if (HandlesKeyboardNotifications)
                RegisterForKeyboardNotifications(true);

            DismissKeyboardOnBackgroundTap();

            base.ViewWillAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            ViewModel.Disappearing();
            ViewIsVisible = false;

            if (HandlesKeyboardNotifications)
                RegisterForKeyboardNotifications(false);

            ReleaseKeyboardTap();
            
            base.ViewWillDisappear(animated);
        }

        #region Keyboard

        public virtual bool HandlesKeyboardNotifications => false;
        private NSObject _keyboardWillShow;
        private NSObject _keyboardWillHide;
        private UITapGestureRecognizer _keyboardTapGesture;

        protected void RegisterForKeyboardNotifications(bool shouldRegister)
        {
            if(shouldRegister)
            {
                _keyboardWillShow = UIKeyboard.Notifications.ObserveWillShow((sender, e) => OnKeyboardNotification(true));
                _keyboardWillHide = UIKeyboard.Notifications.ObserveWillHide((sender, e) => OnKeyboardNotification(false));
            }
            else
            {
                _keyboardWillShow?.Dispose();
                _keyboardWillHide?.Dispose();
            }
        }

        public virtual void OnKeyboardNotification(bool changeKeyboardState) {}

        protected void DismissKeyboardOnBackgroundTap()
        {
            _keyboardTapGesture = new UITapGestureRecognizer { CancelsTouchesInView = false };
            _keyboardTapGesture.AddTarget(() => View.EndEditing(true));
            View.AddGestureRecognizer(_keyboardTapGesture);
        }

        private void ReleaseKeyboardTap()
        {
            _keyboardTapGesture.Dispose();
            _keyboardTapGesture = null;
        }

        #endregion
    }
}
