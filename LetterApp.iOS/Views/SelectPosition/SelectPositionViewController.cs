﻿using System;
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
        public SelectPositionViewController() : base("SelectPositionViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupView();

            _picker.Hidden = true;

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;
                             
            _buttonPicker.TouchUpInside -= OnButtonPicker_TouchUpInside;
            _buttonPicker.TouchUpInside += OnButtonPicker_TouchUpInside;

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
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
                UIView.Animate(0.3, () => _picker.Alpha = 0, HidePicker);
            }
        }

        private void HidePicker() => _picker.Hidden = true;

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

