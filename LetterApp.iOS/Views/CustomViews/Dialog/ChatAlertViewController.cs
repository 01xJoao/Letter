using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class ChatAlertViewController : UIViewController
    {
        private UIPanGestureRecognizer gesture = new UIPanGestureRecognizer();
        private string _pictureCopy;
        private string _picture;
        private string _name;
        private string _message;
        private Action<bool> _openChatAction;

        public ChatAlertViewController(string picture, string name, string message, Action<bool> openChatAction) : base("ChatAlertViewController", null)
        {
            _picture = picture;
            _name = name;
            _message = message;
            _openChatAction = openChatAction;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.UserInteractionEnabled = true;

            CustomUIExtensions.ViewShadowForChatPupUp(_backgroundView);

            _pictureImage.Image?.Dispose();

            if (!string.IsNullOrEmpty(_picture))
            {
                _pictureCopy = string.Copy(_picture);

                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, _picture);
                }).ErrorPlaceholder("profile_noimage", ImageSource.CompiledResource).Retry(3, 200).Finish(CleanString).Transform(new CircleTransformation()).Into(_pictureImage);
            }
            else
            {
                _pictureImage.Image = UIImage.FromBundle("profile_noimage");
                CustomUIExtensions.RoundView(_pictureImage);
            }

            UILabelExtensions.SetupLabelAppearance(_nameLabel, _name, Colors.Black, 13f, UIFontWeight.Semibold);
            UILabelExtensions.SetupLabelAppearance(_descriptionLabel, _message, Colors.Black, 12f, UIFontWeight.Regular);

            _openChatButton.TouchUpInside -= OnOpenChatButton_TouchUpInside;
            _openChatButton.TouchUpInside += OnOpenChatButton_TouchUpInside;

            gesture.AddTarget(() => HandleDrag(gesture));
            this.View.AddGestureRecognizer(gesture);
        }

        private void HandleDrag(UIPanGestureRecognizer gesture)
        {
            Dismiss(0);
            _openChatAction.Invoke(false);
        }

        private void OnOpenChatButton_TouchUpInside(object sender, EventArgs e)
        {
            Dismiss(0);
            _openChatAction.Invoke(true);
        }

        private void CleanString(IScheduledWork obj)
        {
            _pictureCopy = null;
        }

        public void Show()
        {
            this.View.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 92);
            UIApplication.SharedApplication.KeyWindow.AddSubview(this.View);
            UIView.Animate(0.3f, () => View.Alpha = 1);
            Animations.SlideVerticaly(this.View, true, true, onFinished: () => Dismiss(5));
        }

        public void Dismiss(float delay)
        {
            if (this != null)
                Animations.SlideVerticaly(this.View, false, true, onFinished: CleanFromMemory, delay: delay);
        }

        private void CleanFromMemory()
        {
            _openChatAction = null;
            _picture = _name = _message = null;
            _openChatButton.TouchUpInside -= OnOpenChatButton_TouchUpInside;
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

