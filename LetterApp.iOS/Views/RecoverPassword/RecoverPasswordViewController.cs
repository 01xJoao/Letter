using System;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.RecoverPassword
{
    public partial class RecoverPasswordViewController : XViewController<RecoverPasswordViewModel>
    {
        public override bool ShowAsPresentView => true;

        public RecoverPasswordViewController() : base("RecoverPasswordViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupView();

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;

            _requestCodeButton.TouchUpInside -= OnRequestCodeButton_TouchUpInside;
            _requestCodeButton.TouchUpInside += OnRequestCodeButton_TouchUpInside;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnRequestCodeButton_TouchUpInside(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.CloseViewCommand.Execute();
        }

        private void SetupView()
        {
            UILabelExtensions.SetupLabelAppearance(_titleLabel, ViewModel.NewPassTitle, Colors.Black, 17f, UIFontWeight.Semibold);
            _closeButton.SetImage(UIImage.FromBundle("close_black_big"), UIControlState.Normal);
            _closeButton.TintColor = Colors.Black;
            _backgroundView.BackgroundColor = Colors.MainBlue4;
            _formView.BackgroundColor = Colors.MainBlue4;

            //UITextFieldExtensions.SetupField(this.View, 0, );

        }

        public override void ViewWillDisappear(bool animated)
        {
            if ((NavigationController == null && IsMovingFromParentViewController) || (ParentViewController != null && ParentViewController.IsBeingDismissed))
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
            
            base.ViewWillDisappear(animated);
        }
    }
}

