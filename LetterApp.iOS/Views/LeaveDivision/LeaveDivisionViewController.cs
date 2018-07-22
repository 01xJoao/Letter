using System;
using System.ComponentModel;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Views.LeaveDivision
{
    public partial class LeaveDivisionViewController : XViewController<LeaveDivisionViewModel>
    {
        public LeaveDivisionViewController() : base("LeaveDivisionViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            SetupTableView();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.UpdateView):
                    SetupTableView();
                    break;
                default:
                    break;
            }
        }

        private void SetupTableView()
        {
            var source = new LeaveDivisionSource(_tableView, ViewModel.Divisions, ViewModel.LeaveButton);
            _tableView.Source = source;
            _tableView.ReloadData();

            source.LeaveDivisionEvent -= OnSource_LeaveDivisionEvent;
            source.LeaveDivisionEvent += OnSource_LeaveDivisionEvent;
        }

        private void OnSource_LeaveDivisionEvent(object sender, DivisionModel division)
        {
            if (ViewModel.LeaveDivisionCommand.CanExecute(division))
                ViewModel.LeaveDivisionCommand.Execute(division);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.Title = ViewModel.Title;
            NavigationController.NavigationBar.TintColor = Colors.White;
            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = Colors.Black };

            var backButton = UIButtonExtensions.SetupImageBarButton(20, "back_black", CloseView);
            this.NavigationItem.LeftBarButtonItem = backButton;
            NavigationController.InteractivePopGestureRecognizer.Delegate = new UIGestureRecognizerDelegate();

            this.NavigationController.NavigationBar.BarTintColor = Colors.White;
            this.NavigationController.NavigationBar.Translucent = false;
            this.NavigationController.SetNavigationBarHidden(false, true);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
        }

        private void CloseView(object sender, EventArgs e)
        {
            if (ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (this.IsMovingFromParentViewController)
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

