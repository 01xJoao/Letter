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
            DismissKeyboardOnBackgroundTap();


            this.EdgesForExtendedLayout = UIRectEdge.None;
        }

        public override void ViewWillAppear(bool animated)
        {
            ViewModel.Appearing();
            ViewIsVisible = true;

            if (HandlesKeyboardNotifications)
                RegisterForKeyboardNotifications(true);

            base.ViewWillAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            ViewModel.Disappearing();
            ViewIsVisible = false;

            if (HandlesKeyboardNotifications)
                RegisterForKeyboardNotifications(false);
            
            base.ViewWillDisappear(animated);
        }

        #region Keyboard

        public virtual bool HandlesKeyboardNotifications => false;
        private NSObject _keyboardWillShow;
        private NSObject _keyboardWillHide;

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
            var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
            tap.AddTarget(() => View.EndEditing(true));
            View.AddGestureRecognizer(tap);
        }

        #endregion
    }
}
