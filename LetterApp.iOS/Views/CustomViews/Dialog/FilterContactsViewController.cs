using System;
using LetterApp.Core;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class FilterContactsViewController : UIViewController
    {
        private bool _isActive;
        private string _title;
        private string _switchText;
        private string _descriptionText;
        private string _buttonText;
        private Action<bool> _buttonEvent;

        public FilterContactsViewController(string title, string switchText, string descriptionText, string buttonText, bool isActive , Action<bool> button) : base("FilterContactsViewController", null)
        {
            _isActive = isActive;
            _title = title;
            _switchText = switchText;
            _descriptionText = descriptionText;
            _buttonText = buttonText;
            _buttonEvent = button;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.Alpha = 0.3f;

            this.View.BackgroundColor = Colors.Black30;
            _backGroundView.Layer.CornerRadius = 2f;
            _buttonColorBig.Layer.CornerRadius = 2f;
            CustomUIExtensions.ViewShadow(_backGroundView);

            UILabelExtensions.SetupLabelAppearance(_titleLabel, _title, Colors.Black, 24f, UIFontWeight.Medium);
            UILabelExtensions.SetupLabelAppearance(_switchLabel, _switchText, Colors.Black, 14f);
            UILabelExtensions.SetupLabelAppearance(_descriptionLabel, _descriptionText, Colors.GrayLabel, 12f);

            _buttonColorBig.BackgroundColor = Colors.MainBlue;
            _buttonColorSmall.BackgroundColor = Colors.MainBlue;
            UIButtonExtensions.SetupButtonAppearance(_button, Colors.White, 17f, _buttonText);

            _closeButton.SetImage(UIImage.FromBundle("close_black"), UIControlState.Normal);
            _closeButton.ContentMode = UIViewContentMode.ScaleAspectFit;
            _closeButton.TintColor = Colors.Black;


            _switch.On = _isActive;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;

            _button.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _button.TouchUpInside += OnSubmitButton_TouchUpInside;
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonEvent?.Invoke(_switch.On);
            Dismiss();
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonEvent?.Invoke(_isActive);
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

