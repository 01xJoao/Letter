using System;
using System.ComponentModel;
using Foundation;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision
{
    public partial class SelectDivisionViewController : XViewController<SelectDivisionViewModel>, IUIGestureRecognizerDelegate
    {
        public override bool HandlesKeyboardNotifications => true;
        private SelectDivisionsSource _source;

        public SelectDivisionViewController() : base("SelectDivisionViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView();
            _tableView.Hidden = true;

            //NavigationController.SetNavigationBarHidden(true, true);

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.Divisions):
                    SetupTableView();
                    break;
            }
        }

        private void SetupTableView()
        {
            _tableView.Hidden = false;
            _source = new SelectDivisionsSource(_tableView, ViewModel.Divisions, ViewModel.LocationResources, ViewModel.NewUser);
            _tableView.BackgroundColor = Colors.SelectBlue;
            _tableView.Source = _source;
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _tableView.ReloadData();

            _source.DivisionSelectedEvent -= OnSource_DivisionSelectedEvent;
            _source.DivisionSelectedEvent += OnSource_DivisionSelectedEvent;

            _source.SubmitButtonEvent -= OnSource_SubmitButtonEvent;
            _source.SubmitButtonEvent += OnSource_SubmitButtonEvent;

            _source.LeaveOrganizationEvent -= OnSource_LeaveOrganizationEvent;
            _source.LeaveOrganizationEvent += OnSource_LeaveOrganizationEvent;
        }

        private void OnSource_SubmitButtonEvent(object sender, string code)
        {
            if (ViewModel.VerifyDivisionCodeCommand.CanExecute(code))
                ViewModel.VerifyDivisionCodeCommand.Execute(code);
        }

        private void OnSource_DivisionSelectedEvent(object sender, DivisionModel division)
        {
            if (ViewModel.ShowDivisionInformationCommand.CanExecute(division))
                ViewModel.ShowDivisionInformationCommand.Execute(division);
        }

        private void OnSource_LeaveOrganizationEvent(object sender, EventArgs e)
        {
            if (ViewModel.LeaveOrganizationCommand.CanExecute())
                ViewModel.LeaveOrganizationCommand.Execute();
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        private void SetupView()
        {
            _closeButton.SetImage(UIImage.FromBundle("close_white"), UIControlState.Normal);
            _closeButton.TintColor = Colors.White;
            _backgroundView.BackgroundColor = Colors.SelectBlue;
            UILabelExtensions.SetupLabelAppearance(_titleLabel, ViewModel.NewUser ? ViewModel.TitleMainLabel : ViewModel.TitleLabel, Colors.White, 24f);
            CustomUIExtensions.LabelShadow(_titleLabel);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;

            NavigationController.SetNavigationBarHidden(true, true);

            _navigationGesture = this.NavigationController.InteractivePopGestureRecognizer.Delegate;
            this.NavigationController.InteractivePopGestureRecognizer.Delegate = null;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (NavigationController?.InteractivePopGestureRecognizer != null)
                NavigationController.InteractivePopGestureRecognizer.Enabled = false;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.NavigationController.InteractivePopGestureRecognizer.Delegate = _navigationGesture;

            if (NavigationController?.InteractivePopGestureRecognizer != null)
                NavigationController.InteractivePopGestureRecognizer.Enabled = true;

        }

        public override void ViewDidDisappear(bool animated)
        {
            if (this.IsMovingFromParentViewController)
            {
                _source?.Dispose();
                _source = null;
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
                base.ViewDidDisappear(animated);
            }
        }

        IUIGestureRecognizerDelegate _navigationGesture;

        [Export("gestureRecognizerShouldBegin:")]
        public bool ShouldBegin(UIGestureRecognizer recognizer)
        {
            return false;
        }
    }
}

