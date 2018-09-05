using System;
using System.ComponentModel;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Models;
using LetterApp.iOS.Views.Base;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Views.SelectPosition
{
    public partial class SelectPositionViewController : XViewController<SelectPositionViewModel>
    {
        private PositionModel _selectedPosition;

        public SelectPositionViewController() : base("SelectPositionViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupView();

            _backgroundView.AddGestureRecognizer(new UITapGestureRecognizer(HidePickerAction));
            _backgroundView.UserInteractionEnabled = true;

            _picker.Hidden = true;

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;
                             
            _buttonPicker.TouchUpInside -= OnButtonPicker_TouchUpInside;
            _buttonPicker.TouchUpInside += OnButtonPicker_TouchUpInside;

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        void HidePickerAction()
        {
            if (_picker.Hidden)
                return;

            var postionSelected = 0;

            if (_selectedPosition != null)
                postionSelected = ViewModel.Positions.FindIndex(x => x.PositionID == _selectedPosition.PositionID);

            _picker.Select(postionSelected, 0, true);
            SelectedPosition(null, _selectedPosition ?? ViewModel.Positions[0]);
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            if (!_picker.Hidden)
                HidePickerAction();

            if (ViewModel.SetUserCommand.CanExecute())
                ViewModel.SetUserCommand.Execute();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.Positions):
                    _picker.Model = new PositionsPickerModel(ViewModel.Positions, SelectedPosition);
                    break;
                case nameof(ViewModel.IsBusy):
                    Loading();
                    break;
            }
        }

        private void Loading()
        {
            UIViewAnimationExtensions.CustomViewLoadingAnimation("loading_white", this.View, _loadingView, ViewModel.IsBusy);
        }

        private void SelectedPosition(object sender, PositionModel position)
        {
            _selectedPosition = position;
            ViewModel.SetPositionCommand.Execute(position.PositionID);
            UILabelExtensions.SetupLabelAppearance(_pickerLabel, position.Name, Colors.Black, 18f);
            AnimatePicker(false);
        }

        private void OnButtonPicker_TouchUpInside(object sender, EventArgs e)
        {
            if(ViewModel.Positions.Count > 0)
                AnimatePicker(_picker.Hidden);
        }

        private void AnimatePicker(bool show)
        {
            if(show)
            {
                _picker.Alpha = 0.3f;
                _picker.Hidden = false;
                UIView.Animate(0.3, () => _picker.Alpha = 1);

            }
            else
            {
                UIView.Animate(0.3, () => _picker.Alpha = 0, () => _picker.Hidden = true);
            }
        }

        private void SetupView()
        {
            _backgroundView.BackgroundColor = Colors.SelectBlue;
            UILabelExtensions.SetupLabelAppearance(_titleLabel, ViewModel.TitleLabel, Colors.White, 24f);
            CustomUIExtensions.LabelShadow(_titleLabel);

            UILabelExtensions.SetupLabelAppearance(_pickerLabel, ViewModel.SelectPositionLabel, Colors.GrayIndicator, 16f, italic: true);
            UIButtonExtensions.SetupButtonAppearance(_submitButton, Colors.White, 18f, ViewModel.SelectButton);
            _pickerImage.Image = UIImage.FromBundle("dropdown_black");

            CustomUIExtensions.RoundShadow(_buttonPickerView);
            CustomUIExtensions.RoundShadow(_picker);
            CustomUIExtensions.RoundShadow(_pickerImage);
            CustomUIExtensions.SelectButton(_buttonView, Colors.White);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;

            if(NavigationController?.InteractivePopGestureRecognizer != null)
                NavigationController.InteractivePopGestureRecognizer.Enabled = false;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (NavigationController?.InteractivePopGestureRecognizer != null)
                NavigationController.InteractivePopGestureRecognizer.Enabled = true;
        }

        public override void ViewDidDisappear(bool animated)
        {
            _picker.Model?.Dispose();
            _picker.Model = null;
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
            base.ViewDidDisappear(animated);
        }
    }
}

