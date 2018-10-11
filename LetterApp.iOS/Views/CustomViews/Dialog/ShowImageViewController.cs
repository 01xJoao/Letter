using System;
using FFImageLoading;
using FFImageLoading.Work;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class ShowImageViewController : UIViewController
    {
        private string _image;
        private string _saveText;
        private Action<bool> _buttonAction;

        public ShowImageViewController(string image, string saveText, Action<bool> buttonAction) : base("ShowImageViewController", null)
        {
            _image = image;
            _saveText = saveText;
            _buttonAction = buttonAction;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (string.IsNullOrEmpty(_image))
            {
                Dismiss();
                return;
            }

            this.View.Alpha = 0f;

            if (PhoneModelExtensions.IsIphoneX())
                _buttonHeightConstraint.Constant += UIApplication.SharedApplication.KeyWindow.SafeAreaInsets.Bottom;

            ImageService.Instance.LoadUrl(_image).ErrorPlaceholder("connection", ImageSource.CompiledResource).Retry(3, 200).Into(_imageView);

            UIButtonExtensions.SetupButtonAppearance(_saveButton, Colors.Black, 15f, _saveText);
            _saveButton.Hidden = true;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;

            _saveButton.TouchUpInside -= OnSaveButton_TouchUpInside;
            _saveButton.TouchUpInside += OnSaveButton_TouchUpInside;
        }

        private void OnSaveButton_TouchUpInside(object sender, EventArgs e)
        {
            Dismiss();
            _buttonAction?.Invoke(true);
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            Dismiss();
            _buttonAction?.Invoke(false);
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

