using System;
using System.Collections.Generic;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class ContactViewController : UIViewController
    {
        private Action<CallingType> _buttonAction;
        private Dictionary<string, string> _locationResources;
        private bool _showPhoneOption;

        public ContactViewController(Dictionary<string, string> locationResources, bool showPhoneOption, Action<CallingType> buttonAction) : base("ContactViewController", null)
        {
            _locationResources = locationResources;
            _buttonAction = buttonAction;
            _showPhoneOption = showPhoneOption;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.Alpha = 0.3f;
            this.View.BackgroundColor = Colors.Black30;
            _backgroundView.Layer.CornerRadius = 2f;
            CustomUIExtensions.ViewShadow(_backgroundView);

            UILabelExtensions.SetupLabelAppearance(_titleLabel, _locationResources["Title"], Colors.Black, 20f, UIFontWeight.Semibold);

            _image1.Image = UIImage.FromBundle("letter_curved");
            UILabelExtensions.SetupLabelAppearance(_title1, _locationResources["TitleLetter"], Colors.Black, 18f, UIFontWeight.Medium);
            UILabelExtensions.SetupLabelAppearance(_description1, _locationResources["DescriptionLetter"], Colors.DescriptionCall, 12f);

            _image2.Image = UIImage.FromBundle("call_cellular");
            UILabelExtensions.SetupLabelAppearance(_title2, _locationResources["TitlePhone"], Colors.Black, 18f, UIFontWeight.Medium);
            UILabelExtensions.SetupLabelAppearance(_description2, _locationResources["DescriptionPhone"], Colors.DescriptionCall, 12f);

            _closeButton.SetImage(UIImage.FromBundle("close_black"), UIControlState.Normal);
            _closeButton.ContentMode = UIViewContentMode.ScaleAspectFit;
            _closeButton.TintColor = Colors.Black;

            _button1.TouchUpInside -= OnButton1_TouchUpInside;
            _button1.TouchUpInside += OnButton1_TouchUpInside;

            if (_showPhoneOption)
            {
                _button2.TouchUpInside -= OnButton2_TouchUpInside;
                _button2.TouchUpInside += OnButton2_TouchUpInside;
            }
            else
            {
                _hideButtonView.Hidden = false;
                _hideButtonView.Alpha = 0.6f;
            }

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;
        }

        private void OnButton1_TouchUpInside(object sender, EventArgs e)
        {
            _buttonAction.Invoke(CallingType.Letter);
            Dismiss();
        }

        private void OnButton2_TouchUpInside(object sender, EventArgs e)
        {
            _buttonAction.Invoke(CallingType.Cellphone);
            Dismiss();
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonAction.Invoke(CallingType.Close);
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
            _buttonAction = null;
            _locationResources = null;
            _button1.TouchUpInside -= OnButton1_TouchUpInside;
            _button2.TouchUpInside -= OnButton2_TouchUpInside;
            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;

            MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

