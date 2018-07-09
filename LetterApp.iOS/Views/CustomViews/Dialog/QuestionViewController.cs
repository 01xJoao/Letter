using System;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class QuestionViewController : UIViewController
    {

        private string _text;
        private string _button;
        private Action<bool> _buttonEvent;
        private QuestionType _inputType;

        public QuestionViewController(string text, string button, Action<bool> buttonEvent, QuestionType inputType) : base("QuestionViewController", null) 
        {
            _text = text;
            _button = button;
            _buttonEvent = buttonEvent;
            _inputType = inputType;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.Alpha = 0.3f;
            this.View.BackgroundColor = Colors.Black30;
            _backgroundView.Layer.CornerRadius = 2f;
            CustomUIExtensions.ViewShadow(_backgroundView);
            _viewButton.Layer.CornerRadius = 2f;

            switch (_inputType)
            {
                case QuestionType.Good:
                    _viewButtonSmall.BackgroundColor = Colors.Green;
                    _viewButton.BackgroundColor = Colors.Green;
                    break;
                case QuestionType.Bad:
                    _viewButtonSmall.BackgroundColor = Colors.Red;
                    _viewButton.BackgroundColor = Colors.Red;
                    break;
                case QuestionType.Normal:
                    _viewButtonSmall.BackgroundColor = Colors.MainBlue;
                    _viewButton.BackgroundColor = Colors.MainBlue;
                    break;
                default:
                    _viewButtonSmall.BackgroundColor = Colors.MainBlue;
                    _viewButton.BackgroundColor = Colors.MainBlue;
                    break;
            }

            UILabelExtensions.SetupLabelAppearance(_label, _text, Colors.Black, 20f, UIFontWeight.Medium);
            UIButtonExtensions.SetupButtonAppearance(_submitButton, Colors.White, 17f, _button);

            _closeButton.SetImage(UIImage.FromBundle("close_black"), UIControlState.Normal);
            _closeButton.TintColor = Colors.Black;

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonEvent.Invoke(true);
            Dismiss();
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonEvent.Invoke(false);
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

