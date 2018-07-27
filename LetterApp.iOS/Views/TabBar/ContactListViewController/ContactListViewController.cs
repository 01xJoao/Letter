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
        private UIView _barView;
        private UIPageViewController _pageViewController;
        private List<XBoardPageViewController> _viewControllers;

        private UISearchController _serach;
        private bool _isSearchActive;

        private int _currentPageViewIndex;
        private bool _disableUnderline;
        private bool _selectedFromTab;
        private float _tabCenter;
        private float _viewSize;
        private int _totalTabs;

        public ContactListViewController() : base("ContactListViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewSize = (float)UIScreen.MainScreen.Bounds.Width;
            _barView = new UIView();
            _barView.BackgroundColor = Colors.MainBlue;
            _separatorView.BackgroundColor = Colors.GrayDividerContacts;

            this.Title = ViewModel.Title;
            this.NavigationController.NavigationBar.PrefersLargeTitles = true;
            this.NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Automatic;

            _pageViewController = new UIPageViewController(UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation.Horizontal);

            this.AddChildViewController(_pageViewController);
            _pageView.AddSubview(_pageViewController.View);

            this.View.AddSubview(_barView);
            this.View.BringSubviewToFront(_barView);

            ConfigureView();

            _serach = new UISearchController(searchResultsController: null) 
            {
                DimsBackgroundDuringPresentation = false,    
            };

            _serach.SearchBar.TintColor = Colors.White;
            _serach.SearchBar.BarStyle = UIBarStyle.Black;

            var textFieldInsideSearchBar = _serach.SearchBar.ValueForKey(new NSString("searchField")) as UITextField;
            textFieldInsideSearchBar.AttributedPlaceholder = new NSAttributedString("Search", foregroundColor: UIColor.White);
            var backgroundField = textFieldInsideSearchBar.Subviews[0];
            backgroundField.Alpha = 0f;

            _serach.SearchBar.SetImageforSearchBarIcon(UIImage.FromBundle("search"), UISearchBarIcon.Search, UIControlState.Normal);
            _serach.SearchBar.SetImageforSearchBarIcon(UIImage.FromBundle("clear"), UISearchBarIcon.Clear, UIControlState.Normal);

            _serach.SearchBar.OnEditingStarted -= OnSearchBar_OnEditingStarted;
            _serach.SearchBar.OnEditingStarted += OnSearchBar_OnEditingStarted;

            _serach.SearchBar.OnEditingStopped -= OnSearchBar_OnEditingStopped;
            _serach.SearchBar.OnEditingStopped += OnSearchBar_OnEditingStopped;

            _serach.SearchBar.CancelButtonClicked -= OnSearchBar_CancelButtonClicked;
            _serach.SearchBar.CancelButtonClicked += OnSearchBar_CancelButtonClicked;

            _serach.SearchResultsUpdater = this;
            this.DefinesPresentationContext = true;
            this.NavigationItem.SearchController = _serach;

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
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
                default:
                    break;
            }
        }

        private void ConfigureView()
        {
            //Page
            if (ViewModel.ContactLists.Contacts.Count == 0)
                return;

            _viewControllers = new List<XBoardPageViewController>();

            int divisionView = 0;
            foreach (var divisionList in ViewModel.ContactLists.Contacts)
            {
                var division = new ContactPageViewController(divisionView, divisionList, ContactEvent);
                _viewControllers.Add(division);
                divisionView++;
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
                    break;
                }
            }

            UpdatePageView();

            //Tab

            _totalTabs = ViewModel.ContactTab.Count;
            float screenWidth = (float)UIScreen.MainScreen.Bounds.Width;
            var sizeForTab = screenWidth / _totalTabs;
            sizeForTab = sizeForTab < LocalConstants.Contacts_TabMinWidth ? LocalConstants.Contacts_TabMinWidth : sizeForTab;

            if ((int)sizeForTab == (int)LocalConstants.Contacts_TabMinWidth)
                _disableUnderline = true;


            _tabScrollView.ContentInset = new UIEdgeInsets(0, 0, 0, 0);
            _tabScrollView.ContentSize = new CGSize(sizeForTab * _totalTabs, LocalConstants.Contacts_TabHeight);
            _tabScrollView.AutosizesSubviews = false;
            _tabScrollView.LayoutIfNeeded();

            _barView.Frame = new CGRect(0, _tabScrollView.Frame.Height - 2, sizeForTab, 2);
            _barView.Hidden = _disableUnderline;

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

            _barView.Hidden = _totalTabs <= 1;
            _tabBarViewHeightConstraint.Constant = _totalTabs <= 1 ? LocalConstants.Contacts_TabMinHeight : LocalConstants.Contacts_TabHeight;
        }

        private void UpdatePageView()
        {
            var tabSelected = ViewModel.ContactTab.Find(x => x.IsSelected);
            var viewControllerVisible = _viewControllers[tabSelected.DivisionIndex];

            _pageViewController.SetViewControllers(new UIViewController[]
            {
                viewControllerVisible != null ? viewControllerVisible : _viewControllers.FirstOrDefault()
            }, tabSelected.DivisionIndex > _currentPageViewIndex ? UIPageViewControllerNavigationDirection.Forward : UIPageViewControllerNavigationDirection.Reverse, 
                                                   tabSelected.DivisionIndex != _currentPageViewIndex, DidFinishAnimating);

            _currentPageViewIndex = tabSelected.DivisionIndex;
        }

        private void UpdateTabBar()
        {
            var tabIndexSelected = ViewModel.ContactTab.Find(x => x.IsSelected).DivisionIndex;

            foreach(var subview in _tabScrollView.Subviews)
            {
                var tabView = subview as TabBarView;

                if (tabView != null)
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
                            _tabScrollView.SetContentOffset(new CGPoint(tabView.Center.X - _viewSize/2, 0), true);
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
            }
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

            if(_isSearchActive)
            {
                _serach.SearchBar.ResignFirstResponder();
            }
        }

        private void OnSearchBar_OnEditingStarted(object sender, EventArgs e)
        {
            if(!_isSearchActive)
            {
                _tabScrollTopConstraint.Constant = 20;
                UIView.Animate(0.3f, 0, UIViewAnimationOptions.CurveEaseInOut,
                   () => {
                       _barView.Frame = new CGRect(_barView.Frame.X, _barView.Frame.Y + 20, _barView.Frame.Width, _barView.Frame.Height);
                        this.View.LayoutIfNeeded();
                   }, null
               );
            }
            
            _isSearchActive = true;
        }

        private void OnSearchBar_OnEditingStopped(object sender, EventArgs e)
        {
        }

        private void OnSearchBar_CancelButtonClicked(object sender, EventArgs e)
        {
            if (_isSearchActive)
            {
                _tabScrollTopConstraint.Constant = 0;
                UIView.Animate(0.3f, 0, UIViewAnimationOptions.CurveEaseInOut,
                   () =>
                   {
                       _barView.Frame = new CGRect(_barView.Frame.X, _barView.Frame.Y - 20, _barView.Frame.Width, _barView.Frame.Height);
                       this.View.LayoutIfNeeded();
                   }, null
               );
            }

            _isSearchActive = false;
        }

        public void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
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
            NavigationItem.HidesSearchBarWhenScrolling = false;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;
            this.NavigationItem.HidesSearchBarWhenScrolling = true;
            this.NavigationItem.SearchController.SearchBar.EndEditing(true);
        }
    }
}

