using System;
using CoreGraphics;
using Foundation;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class AlertView : UIView
    {
        private float _duration;
        public static readonly UINib Nib = UINib.FromName("AlertView", NSBundle.MainBundle);
        public AlertView(IntPtr handle) : base(handle) {}
        public static AlertView Create() => Nib.Instantiate(null, null)[0] as AlertView;

        public void Configure(string title, AlertType alertType, float duration)
        {
            this.ClipsToBounds = true;

            _duration = duration;

            UILabelExtensions.SetupLabelAppearance(_titleLabel, title, Colors.White, 14f, UIFontWeight.Medium);

            switch (alertType)
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

            _closeButton.SetImage(UIImage.FromBundle("close_white"), UIControlState.Normal);
            _closeButton.TintColor = Colors.White;
            _closeButton.Hidden = true;

            Show();
        }

        public void Show()
        {
            this.Frame = new CGRect(0, UIApplication.SharedApplication.StatusBarFrame.Size.Height, UIApplication.SharedApplication.KeyWindow.Bounds.Width, LocalConstants.AlertDialogSize);
            UIApplication.SharedApplication.KeyWindow.Add(this);
            Animations.SlideVerticaly(this, true, true, onFinished: () => Dismiss(_duration));
        }

        public void Dismiss(float delay)
        {
            if (this != null)
                Animations.SlideVerticaly(this, false, true, onFinished: CleanFromMemory, delay: delay);
        }

        private void CleanFromMemory()
        {
            MemoryUtility.ReleaseUIViewWithChildren(this);
        }
    }
}
