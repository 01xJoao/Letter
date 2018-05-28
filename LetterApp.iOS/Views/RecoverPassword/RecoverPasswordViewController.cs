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
        }

        private void SetupView()
        {
            UILabelExtensions.SetupLabelAppearance(_titleLabel, ViewModel.ChangePassTitle, Colors.Black, 17f, UIFontWeight.Semibold);
            _closeButton.SetImage(UIImage.FromBundle("close_black_big"), UIControlState.Normal);
            _closeButton.TintColor = Colors.Black;
            _tableView.BackgroundColor = Colors.MainBlue3;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ViewWillDisappear(bool animated)
        {
            if ((NavigationController == null && IsMovingFromParentViewController) || (ParentViewController != null && ParentViewController.IsBeingDismissed))
            {
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
                MemoryUtility.ReleaseUITableViewCell(_tableView);
            }
            base.ViewWillDisappear(animated);
        }
    }
}

