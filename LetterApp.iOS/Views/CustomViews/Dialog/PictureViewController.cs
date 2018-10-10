using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class PictureViewController : UIViewController
    {
        private string _image;
        private string _sendText;
        private string _cancelText;
        private Action<bool> _buttonAction;

        public PictureViewController(string image, string sendText, string cancelText, Action<bool> buttonAction) : base("PictureViewController", null)
        {
            _image = image;
            _sendText = sendText;
            _cancelText = cancelText;
            _buttonAction = buttonAction;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.Alpha = 0.3f;
            this.View.BackgroundColor = Colors.Black30;
            _sendButton.BackgroundColor = Colors.MainBlue;

            ImageService.Instance.LoadStream((token) => {
                return ImageHelper.GetStreamFromImageByte(token, _image);
            }).ErrorPlaceholder("letter_round_big", ImageSource.CompiledResource).Transform(new RoundedTransformation(5f)).Into(_imageView);

            UIButtonExtensions.SetupButtonAppearance(_sendButton, Colors.White, 14f, _sendText);
            UIButtonExtensions.SetupButtonAppearance(_cancelButton, Colors.Black, 14f, _cancelText);

            _sendButton.Layer.CornerRadius = 2f;
            _cancelButton.Layer.CornerRadius = 2f;

            _sendButton.TouchUpInside -= OnSendButton_TouchUpInside;
            _sendButton.TouchUpInside += OnSendButton_TouchUpInside;

            _cancelButton.TouchUpInside -= OnCancelButton_TouchUpInside;
            _cancelButton.TouchUpInside += OnCancelButton_TouchUpInside;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            var blurView = new UIVisualEffectView(UIBlurEffect.FromStyle(UIBlurEffectStyle.Regular)) { Frame = _blurView.Frame };
            _blurView.Add(blurView);
        }

        private void OnSendButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonAction?.Invoke(true);
            Dismiss();
        }

        private void OnCancelButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonAction?.Invoke(false);
            Dismiss();
        }

        public void Show()
        {
            this.View.Frame = UIApplication.SharedApplication.KeyWindow.Bounds;
            UIApplication.SharedApplication.KeyWindow.AddSubview(this.View);
            UIView.Animate(0.3, () => View.Alpha = 1);
        }

        public void Dismiss()
        {
            UIView.AnimateNotify(0.3, () => View.Alpha = 0, (finished) => CleanFromMemory());
        }

        private void CleanFromMemory()
        {
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

