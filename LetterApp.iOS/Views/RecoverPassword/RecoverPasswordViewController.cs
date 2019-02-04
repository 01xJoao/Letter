using System;
using System.ComponentModel;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.RecoverPassword
{
    public partial class RecoverPasswordViewController : XViewController<RecoverPasswordViewModel>
    {
        public override bool ShowAsPresentView => true;
        public override bool HandlesKeyboardNotifications => true;
        private bool _keyboardViewState;
        private RecoverPasswordSource _source;

        public RecoverPasswordViewController() : base("RecoverPasswordViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupView();
            SetupTableView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.IsSubmiting):
                    Loading();
                    break;
                default:
                    break;
            }
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.SubmitFormCommand.CanExecute())
                ViewModel.SubmitFormCommand.Execute();
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            if(ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        private void SetupView()
        {
            if (PhoneModelExtensions.IsIphoneX())
            {
                _navBarTopConstraint.Constant = LocalConstants.IphoneXNotchHeight;
                _buttonHeightConstraint.Constant += UIApplication.SharedApplication.KeyWindow.SafeAreaInsets.Bottom;
            }

            UIButtonExtensions.SetupButtonAppearance(_submitButton, Colors.White, 16f, ViewModel.SubmitButton);
            UILabelExtensions.SetupLabelAppearance(_titleLabel, ViewModel.NewPassTitle, Colors.Black, 17f, UIFontWeight.Semibold);
            _closeButton.SetImage(UIImage.FromBundle("close_black"), UIControlState.Normal);
            _closeButton.TintColor = Colors.Black;
            _buttonView.BackgroundColor = Colors.MainBlue;
        }

        private void SetupTableView()
        {
            _source = new RecoverPasswordSource(_tableView, _backgroundView, ViewModel.FormModelList);
            _tableView.BackgroundColor = Colors.MainBlue4;
            _tableView.Source = _source;
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _tableView.ReloadData();
        }

        private void Loading()
        {
            UIViewAnimationExtensions.CustomButtomLoadingAnimation("load_white", _submitButton, ViewModel.SubmitButton, ViewModel.IsSubmiting);
        }

        public override void OnKeyboardNotification(UIKeyboardEventArgs keybordEvent, bool changeKeyboardState)
        {
            if (_keyboardViewState != changeKeyboardState && ViewIsVisible)
            {
                _keyboardViewState = changeKeyboardState;

                if (!_keyboardViewState)
                {
                    UIViewAnimationExtensions.AnimateBackgroundView(_backgroundView, 0, _keyboardViewState);
                    _source.IsAnimated = false;
                }
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            _source?.Dispose();
            _source = null;
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
            base.ViewWillDisappear(animated);
        }
    }
}

