using System;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class InformationViewController : UIViewController
    {
        private string _title;
        private string _text1;
        private string _text2;
        private string _text3;
        private string _confirmButtonText;
        private Action<bool> _buttonEvent;

        public InformationViewController(string title, string text1, string text2, string text3, string confirmButtonText, Action<bool> button) : base("InformationViewController", null) 
        {
            _title = title;
            _text1 = text1;
            _text2 = text2;
            _text3 = text3;
            _confirmButtonText = confirmButtonText;
            _buttonEvent = button;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.Alpha = 0.3f;
            this.View.BackgroundColor = Colors.Black30;
            _backgroundView.Layer.CornerRadius = 2f;
            _buttonView.Layer.CornerRadius = 2f;
            CustomUIExtensions.ViewShadow(_backgroundView);

            UILabelExtensions.SetupLabelAppearance(_titleLabel, _title, Colors.Black, 24f, UIFontWeight.Medium);
            UILabelExtensions.SetupLabelAppearance(_label1, _text1, Colors.GrayLabel, 14f);
            UILabelExtensions.SetupLabelAppearance(_label2, _text2, Colors.GrayLabel, 14f);
            UILabelExtensions.SetupLabelAppearance(_label3, _text3, Colors.GrayLabel, 14f);

            CustomUIExtensions.RoundView(_dotview1);
            CustomUIExtensions.RoundView(_dotview2);
            CustomUIExtensions.RoundView(_dotview3);

            if(string.IsNullOrEmpty(_text2))
            {
                _dotview2.Hidden = true;
                _label2.Hidden = true;
            }

            if (string.IsNullOrEmpty(_text3))
            {
                _dotview3.Hidden = true;
                _label3.Hidden = true;
            }

            UIButtonExtensions.SetupButtonAppearance(_submitButton, Colors.White, 17f, _confirmButtonText);

            _closeButton.SetImage(UIImage.FromBundle("close_black"), UIControlState.Normal);
            _closeButton.TintColor = Colors.Black;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonEvent?.Invoke(true);
            Dismiss();
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonEvent?.Invoke(false);
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

