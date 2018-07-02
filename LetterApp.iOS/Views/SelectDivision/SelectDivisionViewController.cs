using System;
using System.ComponentModel;
using Airbnb.Lottie;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision
{
    public partial class SelectDivisionViewController : XViewController<SelectDivisionViewModel>
    {
        public override bool HandlesKeyboardNotifications => true;
        private SelectDivisionsSource _source;

        public SelectDivisionViewController() : base("SelectDivisionViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView();
            _tableView.Hidden = true;

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.Divisions):
                    SetupTableView();
                    break;
                case nameof(ViewModel.IsLoading):
                    Loading(ViewModel.IsLoading);
                    break;
                default:
                    break;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            Loading(ViewModel.IsLoading);
        }

        private void Loading(bool shouldAnimate)
        {
            UIViewAnimationExtensions.CustomViewLoadingAnimation("loading_white", this.View, _loadingView, shouldAnimate);
        }

        private void SetupTableView()
        {
            _tableView.Hidden = false;

            _source = new SelectDivisionsSource(_tableView, ViewModel.Divisions, ViewModel.LocationResources);
            _tableView.BackgroundColor = Colors.SelectBlue;
            _tableView.Source = _source;
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _tableView.ReloadData();

        }

        private void SetupView()
        {
            _closeButton.SetImage(UIImage.FromBundle("close_white"), UIControlState.Normal);
            _closeButton.TintColor = Colors.White;
            _backgroundView.BackgroundColor = Colors.SelectBlue;
            UILabelExtensions.SetupLabelAppearance(_titleLabel, ViewModel.TitleLabel, Colors.White, 24f);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
        }

        public override void ViewDidDisappear(bool animated)
        {
            _source?.Dispose();
            _source = null;
            MemoryUtility.ReleaseUITableViewCell(_tableView);
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
            base.ViewDidDisappear(animated);
        }
    }
}

