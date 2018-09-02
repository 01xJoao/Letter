using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Foundation;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using LetterApp.iOS.Views.CustomViews.TabBarView;
using LetterApp.iOS.Views.TabBar.ContactListViewController.PageViewController;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.ContactListViewController
{
    public partial class ContactListViewController : XViewController<ContactListViewModel>, IUIScrollViewDelegate, IUISearchResultsUpdating
    {
        public override bool ShowAsPresentView => true;

        private List<UITableView> _tableViews;
        private UIView _barView;
        private UIPageViewController _pageViewController;
        private List<XBoardPageViewController> _viewControllers;
        private UIPanGestureRecognizer gesture = new UIPanGestureRecognizer();
        private UISearchController _search;
        private UIViewController _visibleViewController;
        private UITextField _textFieldInsideSearchBar;

        private int _heightForAnimationTab = PhoneModelExtensions.IsIphoneX() ? 43 : 20;
        private bool _isSearchActive;
        private int _currentPageViewIndex;
        private bool _disableUnderline;
        private bool _selectedFromTab;
        private float _tabCenter;
        private float _viewSize;
        private int _totalTabs;
        private bool _isKeyboardVisible;

        public ContactListViewController() : base("ContactListViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewSize = (float)UIScreen.MainScreen.Bounds.Width;
            _barView = new UIView();
            _barView.BackgroundColor = Colors.MainBlue;
            _separatorView.BackgroundColor = Colors.GrayDividerContacts;
            _separatorView.Hidden = true;

            if (!ViewModel.IsPresentViewForCalls)
            {
                this.Title = ViewModel.Title;

                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    this.NavigationController.NavigationBar.PrefersLargeTitles = false;
                    this.NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;
                }

                this.NavigationItem.RightBarButtonItem = UIButtonExtensions.SetupImageBarButton(20f, "contacts_filter", OpenFilter);

                _presentView.Hidden = true;
                _presentViewHeightConstraint.Constant = 0;
            }
            else
            {
                UILabelExtensions.SetupLabelAppearance(_titleLabel, ViewModel.NewCallTitle, Colors.Black, 17f, UIFontWeight.Semibold);
                UIButtonExtensions.SetupButtonAppearance(_cancelButton, _cancelButton.TintColor, 17f, ViewModel.Cancel);

                _cancelButton.TitleLabel.Lines = 1;
                _cancelButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
                _cancelButton.TitleLabel.LineBreakMode = UILineBreakMode.Clip;
                _cancelButton.TouchUpInside -= OnCancelButton_TouchUpInside;
                _cancelButton.TouchUpInside += OnCancelButton_TouchUpInside;

                if(PhoneModelExtensions.IsIphoneX())
                    _titleLabelHeightConstraint.Constant = _titleLabelHeightConstraint.Constant + 10;
            }

            _pageViewController = new UIPageViewController(UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation.Horizontal);

            this.AddChildViewController(_pageViewController);
            _pageView.AddSubview(_pageViewController.View);

            gesture.AddTarget(() => HandleDrag(gesture));
                     
            this.View.AddSubview(_barView);
            this.View.BringSubviewToFront(_barView);

            ConfigureView();

            _search = new UISearchController(searchResultsController: null) { DimsBackgroundDuringPresentation = false };
            _search.SearchBar.TintColor = Colors.White;
            _search.SearchBar.BarStyle = UIBarStyle.Black;

            _textFieldInsideSearchBar = _search.SearchBar.ValueForKey(new NSString("searchField")) as UITextField;
            _textFieldInsideSearchBar.ReturnKeyType = UIReturnKeyType.Done;
            _textFieldInsideSearchBar.ClearButtonMode = UITextFieldViewMode.Never;

            _search.SearchBar.OnEditingStarted -= OnSearchBar_OnEditingStarted;
            _search.SearchBar.OnEditingStarted += OnSearchBar_OnEditingStarted;

            _search.SearchBar.OnEditingStopped -= OnSearchBar_OnEditingStopped;
            _search.SearchBar.OnEditingStopped += OnSearchBar_OnEditingStopped;

            _search.SearchBar.CancelButtonClicked -= OnSearchBar_CancelButtonClicked;
            _search.SearchBar.CancelButtonClicked += OnSearchBar_CancelButtonClicked;

            _search.SearchResultsUpdater = this;

            if(UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                _textFieldInsideSearchBar.Text = ViewModel.SearchLabel;
                _textFieldInsideSearchBar.Subviews[0].Alpha = 0f;

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

                if(_tableViews?.Count > 0)
                    _tableViews.FirstOrDefault().TableHeaderView = _search.SearchBar;
            }

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void OnCancelButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        private void OpenFilter(object sender, EventArgs e)
        {
            ViewModel.FilterCommand.Execute();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.ConfigureView):
                    ConfigureView();
                    break;
                case nameof(ViewModel.UpdateView):
                    _selectedFromTab = true;
                    UpdatePageView();
                    UpdateTabBar();
                    break;
                case nameof(ViewModel.UpdateTabBar):
                    UpdateTabBar();
                    break;
                case nameof(ViewModel.IsSearching):
                    SearchContacts();
                    break;
                default:
                    break;
            }
        }

        private void SearchContacts()
        {
            int page = 0;
            foreach(var pageController in _viewControllers)
            {
                var controller = pageController as UIViewController;    
                var viewController = controller as ContactPageViewController;
                viewController.SetupTableView(ViewModel.ContactLists.Contacts[page]);
                page++;
            }
        }

        private void ConfigureView()
        {
            if (ViewModel.ContactLists.Contacts.Count == 0)
                return;

            _viewControllers = new List<XBoardPageViewController>();

            int divisionView = 0;
            foreach (var divisionList in ViewModel.ContactLists.Contacts)
            {
                var division = new ContactPageViewController(divisionView, divisionList, ContactEvent, ViewModel.IsPresentViewForCalls);
                _viewControllers.Add(division);
                divisionView++;
            }

            _tableViews = new List<UITableView>();
            foreach (var viewController in _viewControllers)
            {
                foreach (var view in viewController)
                {
                    if (view is UITableView)
                    {
                        _tableViews.Add(view as UITableView);
                        break;
                    }
                }
            }

            var pageSource = new PageSource(_viewControllers);
            _pageViewController.DataSource = pageSource;
            _pageViewController.DidFinishAnimating -= OnPageViewController_DidFinishAnimating;
            _pageViewController.DidFinishAnimating += OnPageViewController_DidFinishAnimating;

            foreach (var view in _pageViewController.View.Subviews)
            {
                if (view is UIScrollView)
                {
                    var scrollView = view as UIScrollView;
                    scrollView.Delegate = this;
                }
            }

            foreach (var viewController in _viewControllers) {
                _pageViewController.SetViewControllers(new UIViewController[]{ viewController as UIViewController}, UIPageViewControllerNavigationDirection.Forward, false, null);    
            }

            UpdatePageView();

            _totalTabs = ViewModel.ContactTab.Count;
            float screenWidth = (float)UIScreen.MainScreen.Bounds.Width;
            var sizeForTab = screenWidth / _totalTabs;
            sizeForTab = sizeForTab < LocalConstants.Contacts_TabMinWidth ? LocalConstants.Contacts_TabMinWidth : sizeForTab;

            if ((int)sizeForTab == (int)LocalConstants.Contacts_TabMinWidth)
                _disableUnderline = true;

            _tabScrollView.ContentInset = new UIEdgeInsets(0, 0, 0, 0);
            _tabScrollView.ContentSize = new CGSize(sizeForTab * _totalTabs, _totalTabs <= 1 ? LocalConstants.Contacts_TabMinHeight : LocalConstants.Contacts_TabHeight);
            _tabScrollView.AutosizesSubviews = false;
            _tabScrollView.LayoutIfNeeded();

            int numberTab = 0;
            foreach (var tab in ViewModel.ContactTab)
            {
                var divisionTab = TabBarView.Create;
                divisionTab.Configure(tab,_disableUnderline);
                divisionTab.Frame = new CGRect((sizeForTab * numberTab), 0, sizeForTab, _totalTabs <= 1 ? LocalConstants.Contacts_TabMinHeight : LocalConstants.Contacts_TabHeight);
                _tabScrollView.AddSubview(divisionTab);

                if (tab.IsSelected)
                {
                    _barView.Frame = new CGRect(divisionTab.Frame.X, _barView.Frame.Y, _barView.Frame.Width, _barView.Frame.Height);
                    _tabCenter = (float)divisionTab.Center.X;
                }

                numberTab++;
            }

            _tabBarViewHeightConstraint.Constant = _totalTabs <= 1 ? LocalConstants.Contacts_TabMinHeight : LocalConstants.Contacts_TabHeight;

            if(!ViewModel.IsPresentViewForCalls)
                _barView.Frame = new CGRect(0, _isSearchActive ? _heightForAnimationTab + _tabScrollView.Frame.Height - 2 : _tabBarViewHeightConstraint.Constant - 2, sizeForTab, 2);
            else
                _barView.Frame = new CGRect(0, _presentViewHeightConstraint.Constant + _tabScrollView.Frame.Height - 2, sizeForTab, 2);

            _barView.Hidden = _disableUnderline || _totalTabs <= 1;
            _separatorView.Hidden = false;
        }

        private void UpdatePageView()
        {
            var tabSelectedIndex = ViewModel.ContactTab.Find(x => x.IsSelected).DivisionIndex;

            int visibleTab = _viewControllers.Count < tabSelectedIndex + 1 ? 0 : tabSelectedIndex;

            var viewControllerVisible = _viewControllers[visibleTab];

            _pageViewController.SetViewControllers(new UIViewController[]
            {
                viewControllerVisible ??_viewControllers.FirstOrDefault()
            }, visibleTab > _currentPageViewIndex ? UIPageViewControllerNavigationDirection.Forward : UIPageViewControllerNavigationDirection.Reverse, 
                                                   visibleTab != _currentPageViewIndex, DidFinishAnimating);
            
            _currentPageViewIndex = visibleTab;

            if(_isKeyboardVisible)
                SetGestureRecognizer();
            
            _visibleViewController = _pageViewController.ViewControllers[0];
        }

        private void UpdateTabBar()
        {
            var tabIndexSelected = ViewModel.ContactTab.Find(x => x.IsSelected).DivisionIndex;

            foreach(var subview in _tabScrollView.Subviews)
            {
                if (subview is TabBarView tabView)
                {
                    if (tabView.Division.IsSelected == true)
                        tabView.Division.IsSelected = false;

                    if (tabView.Division.DivisionIndex == tabIndexSelected)
                    {
                        tabView.Division.IsSelected = true;

                        if (_selectedFromTab)
                        {
                            UIView.Animate(0.3f, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
                            {
                                _barView.Frame = new CGRect(tabView.Frame.X, _barView.Frame.Y, _barView.Frame.Width, _barView.Frame.Height);
                            }, null);
                        }
                        _tabCenter = (float)tabView.Center.X;

                        if (_disableUnderline)
                            _tabScrollView.SetContentOffset(new CGPoint(tabView.Center.X - _viewSize / 2, 0), true);
                    }

                    tabView.Configure(tabView.Division, _disableUnderline);
                }
            }
        }

        private void DidFinishAnimating(bool finished)
        {
            if (finished)
                _selectedFromTab = false;
        }

        private void OnPageViewController_DidFinishAnimating(object sender, UIPageViewFinishedAnimationEventArgs e)
        {
            if (e.Completed)
            {
                var viewController = _pageViewController.ViewControllers[0] as XBoardPageViewController;
                _currentPageViewIndex = viewController.Index;
                ViewModel.SwitchDivisionCommand.Execute(viewController.Index);
                _visibleViewController = viewController;
            }
        }

        private void HandleDrag(UIPanGestureRecognizer gesture)
        {
            if (_isSearchActive) {
                _search.SearchBar.ResignFirstResponder();
            }

            _tableViews[_currentPageViewIndex].RemoveGestureRecognizer(gesture);
        }

        private void SetGestureRecognizer(bool shouldRemove = false)
        {
            var alreadyHasGesture = false;
            foreach (var ges in _tableViews[_currentPageViewIndex].GestureRecognizers)
            {
                if (ges == gesture)
                {
                    alreadyHasGesture = true;
                    break;
                }
            }

            if (!alreadyHasGesture)
                _tableViews[_currentPageViewIndex].AddGestureRecognizer(gesture);
            else if (alreadyHasGesture && shouldRemove)
                _tableViews[_currentPageViewIndex].RemoveGestureRecognizer(gesture);
        }

        [Export("scrollViewDidScroll:")]
        public virtual void Scrolled(UIScrollView scrollView)
        {
            if (!_selectedFromTab && !_disableUnderline) 
            {
                var scrollOffSet = scrollView.ContentOffset.X - _viewSize;
                var move = 1.0f / _totalTabs * scrollOffSet;
                _barView.Center = new CGPoint(_tabCenter + move, _barView.Center.Y);
            }
        }

        private void SetSearchView()
        {
            if(_isKeyboardVisible && _isSearchActive)
            {
                if(_textFieldInsideSearchBar.Text == ViewModel.SearchLabel)
                {
                    _textFieldInsideSearchBar.Text = string.Empty;
                    _textFieldInsideSearchBar.ClearButtonMode = UITextFieldViewMode.Always;
                }
            }

            if(!_isSearchActive && !_isKeyboardVisible)
            {
                _textFieldInsideSearchBar.Text = ViewModel.SearchLabel;
                _textFieldInsideSearchBar.ClearButtonMode = UITextFieldViewMode.Never;
            }
        }

        private void OnSearchBar_OnEditingStarted(object sender, EventArgs e)
        {
            if(!_isSearchActive && UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                _tabScrollTopConstraint.Constant = _heightForAnimationTab;
                _pageView.SetNeedsLayout();

                UIView.Animate(0.3f, 0, UIViewAnimationOptions.CurveEaseInOut,
                   () => {
                        _barView.Frame = new CGRect(_barView.Frame.X, _barView.Frame.Y + _heightForAnimationTab, _barView.Frame.Width, _barView.Frame.Height);
                        this.View.LayoutIfNeeded();
                   }, null
               );
            }

            SetGestureRecognizer();
            _isSearchActive = true;
            _isKeyboardVisible = true;
            SetSearchView();
        }

        private void OnSearchBar_OnEditingStopped(object sender, EventArgs e)
        {
            _isKeyboardVisible = false;
            SetGestureRecognizer(true);
        }

        private void OnSearchBar_CancelButtonClicked(object sender, EventArgs e)
        {
            if (_isSearchActive)
            {
                _isKeyboardVisible = false;
                _isSearchActive = false;

                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    _tabScrollTopConstraint.Constant = 0;
                    UIView.Animate(0.3f, 0, UIViewAnimationOptions.CurveEaseInOut,
                       () =>
                       {
                           _barView.Frame = new CGRect(_barView.Frame.X, _barView.Frame.Y - _heightForAnimationTab, _barView.Frame.Width, _barView.Frame.Height);
                           this.View.LayoutIfNeeded();
                       }, SetSearchView
                   );
                }
            }
        }

        public void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            ViewModel.SearchCommand.Execute(searchController.SearchBar.Text);
        }

        private void ContactEvent(object sender, Tuple<ContactListViewModel.ContactEventType, int> contact)
        {
            if (ViewModel.ContactCommand.CanExecute(contact))
                ViewModel.ContactCommand.Execute(contact);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            _pageViewController.View.Frame = _pageView.Bounds;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                NavigationItem.HidesSearchBarWhenScrolling = false;

            if (ViewModel.IsPresentViewForCalls)
                UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                this.NavigationItem.HidesSearchBarWhenScrolling = true;
                this.NavigationItem.SearchController.SearchBar.EndEditing(true);
            }

            if (this.IsMovingFromParentViewController && ViewModel.IsPresentViewForCalls)
            {
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
            }
        }
    }
}

