using System;
using CoreGraphics;
using LetterApp.Core;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class AlertViewController : UIViewController
    {
        private IDialogService _dialogService;

        private string _title;
        private AlertType _alertType;

        public AlertViewController(string title, AlertType alertType) : base("AlertViewController", null) 
        {
            _title = title;
            _alertType = alertType;
            _dialogService = App.Container.GetInstance<IDialogService>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            switch (_alertType)
            {
                case AlertType.Success:
                    _backgroundView.BackgroundColor = Colors.Green;
                    break;
                case AlertType.Error:
                    _backgroundView.BackgroundColor = Colors.Red;
                    break;
                case AlertType.Info:
                    _backgroundView.BackgroundColor = Colors.Orange;
                    break;
                default:
                    _backgroundView.BackgroundColor = Colors.MainBlue;
                    break;
            }

            UILabelExtensions.SetupLabelAppearance(_titleLabel, _title, Colors.White, 15f);
            _closeButton.SetImage(UIImage.FromBundle("close_white"), UIControlState.Normal);
            _closeButton.TintColor = Colors.White;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e) => Dismiss(0);

        public void Show()
        {
            View.Frame = new CGRect(0,0,UIApplication.SharedApplication.KeyWindow.Bounds.Width, LocalConstants.AlertDialogSize);
            UIApplication.SharedApplication.KeyWindow.AddSubview(View);
            Animations.SlideVerticaly(this.View, true, true, 0.3f, onFinished: () => Dismiss(3));
        }

        public void Dismiss(int delay)
        {
            if(this.View != null && this.ViewIfLoaded?.Window != null)
                Animations.SlideVerticaly(this.View, false, true, 0.3f, delay: delay);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if ((NavigationController == null && IsMovingFromParentViewController) || (ParentViewController != null && ParentViewController.IsBeingDismissed))
            {
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
            }
        }
    }
}

