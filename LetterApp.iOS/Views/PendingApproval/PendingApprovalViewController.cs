using System;
using System.ComponentModel;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.PendingApproval
{
    public partial class PendingApprovalViewController : XViewController<PendingApprovalViewModel>, IRootView
    {
        public PendingApprovalViewController() : base("PendingApprovalViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _backgroundView.BackgroundColor = Colors.WhiteBlue;
            _imageView.Image = UIImage.FromBundle("letter_curved");
            _reloadImage.Image = UIImage.FromBundle("update");

            UILabelExtensions.SetupLabelAppearance(_navTitleLabel, ViewModel.Title, Colors.Black, 22f);
            UILabelExtensions.SetupLabelAppearance(_subtitleLabel, ViewModel.Subtitle, Colors.Black, 24f, UIFontWeight.Light);
            UILabelExtensions.SetupLabelAppearance(_descriptionLabel, ViewModel.Description, Colors.Black, 16f, UIFontWeight.Light);

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.CanContinue):
                    SetupView(ViewModel.CanContinue);
                    break;
                case nameof(ViewModel.IsLoading):
                    Loading(ViewModel.IsLoading);
                    break;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            Loading(true);
        }

        private void Loading(bool shouldAnimate)
        {
            UIViewAnimationExtensions.CustomViewLoadingAnimation("loading", this.View, _loadingView, shouldAnimate);
        }

        private void SetupView(bool canContinue)
        {

            _descriptionLabel.Hidden = false;

            if(canContinue)
            {
                UIButtonExtensions.SetupButtonAppearance(_continueButton, Colors.Black, 18f, ViewModel.SubmitButton, UIFontWeight.Light);
                CustomUIExtensions.SelectButton(_continueView, Colors.Black);

                _continueButton.TouchUpInside -= OnContinueButton_TouchUpInside;
                _continueButton.TouchUpInside += OnContinueButton_TouchUpInside;

                ShouldContinue(true);
            }
            else
            {
                UIButtonExtensions.SetupButtonUnderlineAppearance(_leaveDivisionButton, Colors.Black, 14f, ViewModel.LeaveButton, UIFontWeight.Light);
                UIButtonExtensions.SetupButtonUnderlineAppearance(_logoutButton, Colors.Black, 14f, ViewModel.LogoutButton, UIFontWeight.Light);
                UILabelExtensions.SetupLabelAppearance(_helpLabel, ViewModel.HelpLabel, Colors.Black, 14f, UIFontWeight.Light);
                UIButtonExtensions.SetupButtonUnderlineAppearance(_helpButton, Colors.SelectBlue, 14f, ViewModel.Division.Email);

                ShouldContinue(false);

                _reloadButton.TouchUpInside -= OnReloadButton_TouchUpInside;
                _reloadButton.TouchUpInside += OnReloadButton_TouchUpInside;

                _leaveDivisionButton.TouchUpInside -= OnLeaveDivisionButton_TouchUpInside;
                _leaveDivisionButton.TouchUpInside += OnLeaveDivisionButton_TouchUpInside;

                _logoutButton.TouchUpInside -= OnLogoutButton_TouchUpInside;
                _logoutButton.TouchUpInside += OnLogoutButton_TouchUpInside;

                _helpButton.TouchUpInside -= OnHelpButton_TouchUpInside;
                _helpButton.TouchUpInside += OnHelpButton_TouchUpInside;
            }
        }

        private void ShouldContinue(bool hide)
        {
            _continueButton.Hidden = !hide;
            _continueView.Hidden = !hide;

            _reloadImage.Hidden = hide;
            _reloadButton.Hidden = hide;
            _helpLabel.Hidden = hide;
            _helpButton.Hidden = hide;
            _leaveDivisionButton.Hidden = hide;
            _logoutButton.Hidden = hide;
        }

        private void HideAll()
        {
            _descriptionLabel.Hidden = true;
            _reloadImage.Hidden = true;
            _reloadButton.Hidden = true;
            _continueButton.Hidden = true;
            _continueView.Hidden = true;
            _helpLabel.Hidden = true;
            _helpButton.Hidden = true;
            _leaveDivisionButton.Hidden = true;
            _logoutButton.Hidden = true;
        }

        private void OnReloadButton_TouchUpInside(object sender, EventArgs e)
        {
            _reloadImage.Alpha = 0.8f;
            Animations.Fade(_reloadImage, true, 0.5f);

            if (ViewModel.UpdateCommand.CanExecute())
                ViewModel.UpdateCommand.Execute();
        }

        private void OnContinueButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.NavigateToMainCommand.CanExecute())
                ViewModel.NavigateToMainCommand.Execute();
        }

        private void OnHelpButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.OpenEmailCommand.CanExecute())
                ViewModel.OpenEmailCommand.Execute();
        }

        private void OnLogoutButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.LogoutCommand.CanExecute())
                ViewModel.LogoutCommand.Execute();
        }

        private void OnLeaveDivisionButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.LeaveDivisionCommand.CanExecute())
                ViewModel.LeaveDivisionCommand.Execute();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
            HideAll();
        }
    }
}

