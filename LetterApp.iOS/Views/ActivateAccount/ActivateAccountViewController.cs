using System;
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

        public ActivateAccountViewController() : base("ActivateAccountViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupView();
        }

        private void SetupView()
        {
            UIButtonExtensions.SetupButtonAppearance(_button, Colors.White, 16f, ViewModel.SubmitButton);
            UIButtonExtensions.SetupButtonAppearance(_requestCodeButton, Colors.MainBlue, 16f, ViewModel.ResendCodeButton);
            UILabelExtensions.SetupLabelAppearance(_activateLabel, ViewModel.ActivateLabel, Colors.GrayDivider, 14f);
            UILabelExtensions.SetupLabelAppearance(_titleLabel, ViewModel.Title, Colors.Black, 17f, UIFontWeight.Semibold);

            _textField.AutocorrectionType = UITextAutocorrectionType.No;
            _textField.TextContentType = new NSString("");
            _closeButon.SetImage(UIImage.FromBundle("close_black"), UIControlState.Normal);
            _closeButon.TintColor = Colors.Black;
            _backgroundView.BackgroundColor = Colors.MainBlue4;
            _buttonView.BackgroundColor = Colors.MainBlue;
            _textField.TextColor = Colors.MainBlue;
            _textField.AttributedPlaceholder = new NSAttributedString(ViewModel.PlaceHolder, new UIStringAttributes() { ForegroundColor = Colors.GrayIndicator });

            _textField.BecomeFirstResponder();

            _button.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _button.TouchUpInside += OnSubmitButton_TouchUpInside;

            _closeButon.TouchUpInside -= OnCloseButon_TouchUpInside;
            _closeButon.TouchUpInside += OnCloseButon_TouchUpInside;
        }

        private void OnCloseButon_TouchUpInside(object sender, EventArgs e)
        {
            if(ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
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

