using System;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.CallListViewController
{
    public partial class CallListViewController : XViewController<CallListViewModel>
    {
        private UIImageView _noRecentCallsImage = new UIImageView(UIImage.FromBundle("recent_calls"));
        private UILabel _noRecentCallsLabel = new UILabel();

        public CallListViewController() : base("CallListViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.CallHistory):
                    SetupTableView();
                    break;
                case nameof(ViewModel.NoCalls):
                    HasHistoryCalls(false);
                    break;
                default:
                    break;
            }
        }

        private void ConfigureView()
        {
            this.Title = ViewModel.Title;
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                this.NavigationController.NavigationBar.PrefersLargeTitles = true;
            this.NavigationItem.RightBarButtonItem = UIButtonExtensions.SetupImageBarButton(LocalConstants.TabBarIconSize, "new_call", OpenContacts, false);
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            _noRecentCallsImage.ContentMode = UIViewContentMode.ScaleAspectFit;
            _noRecentCallsImage.SizeThatFits(_noRecentCallsImage.Image.Size);
            var imageViewCenter = _noRecentCallsImage.Center;
            imageViewCenter.X = _tableView.Bounds.GetMidX();
            imageViewCenter.Y = _tableView.Bounds.GetMidY() - 40;
            _noRecentCallsImage.Center = imageViewCenter;

            _noRecentCallsLabel.Center = this.View.Center;
            var labelY = _noRecentCallsImage.Frame.Y + _noRecentCallsImage.Frame.Height + 40;
            _noRecentCallsLabel.Frame = new CGRect(0, labelY, this.View.Frame.Width, 20);
            UILabelExtensions.SetupLabelAppearance(_noRecentCallsLabel, ViewModel.NoRecentCalls, Colors.GrayDivider, 17f, UIFontWeight.Medium);
            _noRecentCallsLabel.TextAlignment = UITextAlignment.Center;

            this.View.AddSubview(_noRecentCallsImage);
            this.View.AddSubview(_noRecentCallsLabel);

            HasHistoryCalls(ViewModel.CallHistory?.Count > 0);
        }

        private void SetupTableView()
        {
            HasHistoryCalls(true);

            var source = new CallSource(_tableView, ViewModel.CallHistory, ViewModel.Delete, ViewModel.CallActionInfo);

            _tableView.Source = source;

            source.CallEvent -= OnSource_CallEvent;
            source.CallEvent += OnSource_CallEvent;

            source.OpenCallerProfileEvent -= OnSource_OpenCallerProfileEvent;
            source.OpenCallerProfileEvent += OnSource_OpenCallerProfileEvent;

            source.DeleteCallEvent -= OnSource_DeleteCallEvent;
            source.DeleteCallEvent += OnSource_DeleteCallEvent;

            source.CallStackEvent -= OnSource_CallStackEvent;
            source.CallStackEvent += OnSource_CallStackEvent;

            _tableView.ReloadData();
        }

        private void OnSource_CallStackEvent(object sender, int call)
        {
            ViewModel.CallStackCommand.Execute(call);
        }

        public void HasHistoryCalls(bool value)
        {
            _tableView.Hidden = !value;
            _noRecentCallsImage.Hidden = value;
            _noRecentCallsLabel.Hidden = value;
        }

        private void OnSource_DeleteCallEvent(object sender, int indexRow)
        {
			ViewModel.DeleteCallCommand.Execute(indexRow);
        }

        private void OnSource_OpenCallerProfileEvent(object sender, int callerId)
        {
            ViewModel.OpenCallerProfileCommand.Execute(callerId);
        }

        private void OnSource_CallEvent(object sender, int callerId)
        {
            ViewModel.CallCommand.Execute(callerId);
        }

        private void OpenContacts(object sender, EventArgs e)
        {
            ViewModel.OpenCallListCommand.Execute();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (AppSettings.BadgeForCalls > 0)
            {
                AppSettings.BadgeForCalls = 0;

                using (var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate)
                {
                    if (appDelegate.RootController?.CurrentViewController is MainViewController)
                    {
                        using (var view = appDelegate.RootController.CurrentViewController as MainViewController)
                        {
                            if (view.TabBar.Items.Any())
                                view.TabBar.Items[1].BadgeValue = null;
                        }
                    }
                }
            }

            _navigationGesture = this.NavigationController.InteractivePopGestureRecognizer.Delegate;
            this.NavigationController.InteractivePopGestureRecognizer.Delegate = null;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;

            this.NavigationController.InteractivePopGestureRecognizer.Delegate = _navigationGesture;
        }

        IUIGestureRecognizerDelegate _navigationGesture;

        [Export("gestureRecognizerShouldBegin:")]
        public bool ShouldBegin(UIGestureRecognizer recognizer)
        {
            return false;
        }
    }
}

