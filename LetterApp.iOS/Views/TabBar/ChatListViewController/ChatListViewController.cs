using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using CoreGraphics;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.ChatListViewController
{
    public partial class ChatListViewController : XViewController<ChatListViewModel>, IUIScrollViewDelegate, IUISearchResultsUpdating, IUIGestureRecognizerDelegate
    {
        private UIView _loadingView = new UIView();
        private UIImageView _noRecentChatImage = new UIImageView(UIImage.FromBundle("nochats"));
        private UILabel _noRecenChatLabel = new UILabel();
        private UISearchController _search; 
        private UITextField _textFieldInsideSearchBar;

        public ChatListViewController() : base("ChatListViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

           UIApplication.Notifications.ObserveWillEnterForeground(ShowNotifications);
           UIApplication.Notifications.ObserveDidEnterBackground(RemoveNotifications);
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.ChatList):
                    SetupTableView();
                    break;
                case nameof(ViewModel.UpdateTableView):
                    if(this.ViewIsVisible)
                        SetupTableView();
                    break;
                case nameof(ViewModel.NoChats):
                    HasChats(false);
                    break;
                case nameof(ViewModel.IsLoading):
                    LoadingAnimation(ViewModel.IsLoading);
                    break;
                case nameof(ViewModel.BadgeForChat):
                    BadgeForChat(ViewModel.BadgeForChat);
                    break;
            }
        }

        private void LoadingAnimation(bool isLoading)
        {
            UIViewAnimationExtensions.LoadingInView("paperplane_loading", _loadingView, isLoading);
        }

        private void ConfigureView()
        {
            this.Title = ViewModel.Title;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                this.NavigationController.NavigationBar.PrefersLargeTitles = true;
                this.NavigationItem.HidesSearchBarWhenScrolling = true;
            }

            this.NavigationItem.RightBarButtonItem = UIButtonExtensions.SetupImageBarButton(LocalConstants.TabBarIconSize, "newchat", OpenContacts, false);
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            _search = new UISearchController(searchResultsController: null) { DimsBackgroundDuringPresentation = false };
            _search.SearchBar.TintColor = Colors.White;
            _search.SearchBar.BarStyle = UIBarStyle.Black;

            _textFieldInsideSearchBar = _search.SearchBar.ValueForKey(new NSString("searchField")) as UITextField;
            _textFieldInsideSearchBar.ReturnKeyType = UIReturnKeyType.Done;
            _textFieldInsideSearchBar.ClearButtonMode = UITextFieldViewMode.Never;

            _search.SearchResultsUpdater = this;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                _textFieldInsideSearchBar.Subviews[0].Alpha = 0.5f;

                _search.SearchBar.SetImageforSearchBarIcon(UIImage.FromBundle("search"), UISearchBarIcon.Search, UIControlState.Normal);
                _search.SearchBar.SetImageforSearchBarIcon(UIImage.FromBundle("clear"), UISearchBarIcon.Clear, UIControlState.Normal);

                this.DefinesPresentationContext = true;
                this.NavigationItem.SearchController = _search;
            }
            else
            {
                this.DefinesPresentationContext = false;
                _search.HidesNavigationBarDuringPresentation = false;
                _search.SearchBar.BarStyle = UIBarStyle.Default;
                _tableView.TableHeaderView = _search.SearchBar;
            }

            _search.SearchBar.OnEditingStarted -= OnSearchBar_OnEditingStarted;
            _search.SearchBar.OnEditingStarted += OnSearchBar_OnEditingStarted;

            _search.SearchBar.CancelButtonClicked -= OnSearchBar_CancelButtonClicked;
            _search.SearchBar.CancelButtonClicked += OnSearchBar_CancelButtonClicked;
        }

        private void SetupTableView()
        {
            Debug.WriteLine("PropertyChanged: ChatListCountINTableView: " + ViewModel.ChatList.Count);

            HasChats(true);

            var source = new ChatListSource(_tableView, ViewModel.ChatList, ViewModel.Actions);

            _tableView.Source = source;
            _tableView.ReloadData();

            source.ChatListActionsEvent -= OnSource_ChatListActionsEvent;
            source.ChatListActionsEvent += OnSource_ChatListActionsEvent;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            _noRecentChatImage.ContentMode = UIViewContentMode.ScaleAspectFit;
            _noRecentChatImage.SizeThatFits(_noRecentChatImage.Image.Size);
            var imageViewCenter = _noRecentChatImage.Center;
            imageViewCenter.X = _tableView.Bounds.GetMidX();
            imageViewCenter.Y = _tableView.Bounds.GetMidY() - 40;
            _noRecentChatImage.Center = imageViewCenter;

            _noRecenChatLabel.Center = this.View.Center;
            var labelY = _noRecentChatImage.Frame.Y + _noRecentChatImage.Frame.Height + 40;
            _noRecenChatLabel.Frame = new CGRect(0, labelY, this.View.Frame.Width, 20);
            UILabelExtensions.SetupLabelAppearance(_noRecenChatLabel, ViewModel.NoRecentChat, Colors.GrayDivider, 17f, UIFontWeight.Medium);
            _noRecenChatLabel.TextAlignment = UITextAlignment.Center;

            _loadingView.Hidden = true;
            _loadingView.Frame = new CGRect(0, 0, 80, 80);
            var imageLoadingCenter = _noRecentChatImage.Center;
            imageLoadingCenter.X = _tableView.Bounds.GetMidX();
            imageLoadingCenter.Y = _tableView.Bounds.Height - 40;
            _loadingView.Center = imageLoadingCenter;

            this.View.AddSubview(_loadingView);
            this.View.AddSubview(_noRecentChatImage);
            this.View.AddSubview(_noRecenChatLabel);
        }

        public void HasChats(bool value)
        {
            _tableView.Hidden = !value;
            _noRecentChatImage.Hidden = value;
            _noRecenChatLabel.Hidden = value;
        }

        private void ShowNotifications(object sender, NSNotificationEventArgs e)
        {
            ViewModel.ShowNotificationsCommand.Execute(true);
        }

        private void RemoveNotifications(object sender, NSNotificationEventArgs e)
        {
            ViewModel.ShowNotificationsCommand.Execute(false);
        }

        private void OnSource_ChatListActionsEvent(object sender, Tuple<ChatListViewModel.ChatEventType, int> action)
        {
            ViewModel.RowActionCommand.Execute(action);
        }

        private void OpenContacts(object sender, EventArgs e)
        {
            ViewModel.OpenContactsCommand.Execute();
        }

        private void OnSearchBar_OnEditingStarted(object sender, EventArgs e)
        {
            UIView.Animate(0.3f, 0, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    _tableViewTopConstraint.Constant = -30;
                    this.View.LayoutIfNeeded();
                },
                null
            );
        }

        public void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            ViewModel.SearchChatCommand.Execute(searchController.SearchBar.Text?.ToLower());
        }

        private void OnSearchBar_CancelButtonClicked(object sender, EventArgs e)
        {
            UIView.Animate(0.3f, 0, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    _tableViewTopConstraint.Constant = 0;
                    this.View.LayoutIfNeeded();
                },
                null
            );

            ViewModel.CloseSearchCommand.Execute();
        }

        private void BadgeForChat(bool clean)
        {
            if (!this.ViewIsVisible && clean)
            {
                AppSettings.BadgeForChat++;

                using (var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate)
                {
                    if (appDelegate.RootController?.CurrentViewController is MainViewController view)
                    {
                        if (view.TabBar.Items.Any())
                            view.TabBar.Items[0].BadgeValue = AppSettings.BadgeForChat.ToString();
                    }
                }
            }
            else if(!clean)
            {
                CleanBadge();
            }
        }

        private void CleanBadge()
        {
            AppSettings.BadgeForChat = 0;

            using (var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate)
            {
                if (appDelegate.RootController?.CurrentViewController is MainViewController)
                {
                    using (var view = appDelegate.RootController.CurrentViewController as MainViewController)
                    {
                        if (view.TabBar.Items.Any())
                            view.TabBar.Items[0].BadgeValue = null;
                    }
                }
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (AppSettings.BadgeForChat > 0)
            {
                CleanBadge();
            }

            HasChats(ViewModel?.ChatList?.Count > 0);

            if (this.NavigationController?.InteractivePopGestureRecognizer != null)
            {
                _navigationGesture = this.NavigationController.InteractivePopGestureRecognizer.Delegate;
                this.NavigationController.InteractivePopGestureRecognizer.Delegate = null;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;

            if (_navigationGesture != null)
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

