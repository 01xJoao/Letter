using System;
using System.ComponentModel;
using Airbnb.Lottie;
using Foundation;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.ActivateAccount
{
    public partial class ActivateAccountViewController : XViewController<ActivateAccountViewModel>
    {
        public override bool HandlesKeyboardNotifications => true;
        public override bool ShowAsPresentView => true;
        private LOTAnimationView _lottieAnimation;

        public ActivateAccountViewController() : base("ActivateAccountViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.IsActivating):
                    Loading();
                    break;
                default:
                    break;
            }
        }

        private void SetupView()
        {
            UIButtonExtensions.SetupButtonAppearance(_button, Colors.White, 16f, ViewModel.SubmitButton);
            UIButtonExtensions.SetupButtonAppearance(_requestCodeButton, Colors.MainBlue, 15f, ViewModel.ResendCodeButton);
            UILabelExtensions.SetupLabelAppearance(_activateLabel, ViewModel.ActivateLabel, Colors.GrayDivider, 14f);
            UILabelExtensions.SetupLabelAppearance(_titleLabel, ViewModel.Title, Colors.Black, 17f, UIFontWeight.Semibold);

            var keyboardButton = new UIButton();
            UIButtonExtensions.SetupButtonAppearance(keyboardButton, Colors.White, 16f, ViewModel.SubmitButton);
            keyboardButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            keyboardButton.TouchUpInside += OnSubmitButton_TouchUpInside;
            UITextFieldExtensions.AddViewToKeyboard(_textField, keyboardButton);

            _textField.AutocorrectionType = UITextAutocorrectionType.No;
            _textField.TextContentType = new NSString("");
            _closeButon.SetImage(UIImage.FromBundle("close_black"), UIControlState.Normal);
            _closeButon.TintColor = Colors.Black;
            _backgroundView.BackgroundColor = Colors.MainBlue4;
            _buttonView.BackgroundColor = Colors.MainBlue;
            _textField.TextColor = Colors.MainBlue;
            _textField.AttributedPlaceholder = new NSAttributedString(ViewModel.PlaceHolder, new UIStringAttributes() { ForegroundColor = Colors.GrayIndicator });
            _textField.BecomeFirstResponder();

            _requestCodeButton.TouchUpInside -= OnRequestCodeButton_TouchUpInside;
            _requestCodeButton.TouchUpInside += OnRequestCodeButton_TouchUpInside;

            _button.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _button.TouchUpInside += OnSubmitButton_TouchUpInside;

            _closeButon.TouchUpInside -= OnCloseButon_TouchUpInside;
            _closeButon.TouchUpInside += OnCloseButon_TouchUpInside;
        }

        private void Loading()
        {
            _lottieAnimation = UIViewAnimationExtensions.CustomButtomLoadingAnimation(_lottieAnimation, "loading_white", _button, ViewModel.SubmitButton, ViewModel.IsActivating);
        }

        private void OnRequestCodeButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.ResendCodeCommand.CanExecute())
                ViewModel.ResendCodeCommand.Execute();
        }

        private void OnCloseButon_TouchUpInside(object sender, EventArgs e)
        {
            if(ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            _textField.ResignFirstResponder();
            if (ViewModel.ActivateAccountCommand.CanExecute(_textField.Text))
                ViewModel.ActivateAccountCommand.Execute(_textField.Text);
        }

        public override void ViewDidDisappear(bool animated)
        {
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
            base.ViewDidDisappear(animated);
        }
    }
}

