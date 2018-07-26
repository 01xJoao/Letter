using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class ContactListViewController : XViewController<ContactListViewModel>, IUIScrollViewDelegate
    {
        private int _currentPageViewIndex;
        private UIPageViewController _pageViewController;
        private UIView _barView;

        private bool _disableUnderline;
        private bool _selectedFromTab;
        private bool _barViewFrameIsSet;
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
            
        }

        private void UpdatePageView()
        {
            if (ViewModel.ContactLists.Contacts.Count == 0)
                return;
            
            var viewControllers = new List<XBoardPageViewController>();

            int divisionView = 0;
            foreach(var divisionList in ViewModel.ContactLists.Contacts)
            {
                var division = new ContactPageViewController(divisionView, divisionList, ContactEvent);
                viewControllers.Add(division);
                divisionView++;
            }

            var tabSelected = ViewModel.ContactTab.Find(x => x.IsSelected);
            var viewControllerVisible = viewControllers[tabSelected.DivisionIndex];

            var pageSource = new PageSource(viewControllers);
            _pageViewController.DataSource = pageSource;
            _pageViewController.SetViewControllers(new UIViewController[]
            {
                viewControllerVisible != null ? viewControllerVisible : viewControllers.FirstOrDefault()
            }, tabSelected.DivisionIndex > _currentPageViewIndex ? UIPageViewControllerNavigationDirection.Forward : UIPageViewControllerNavigationDirection.Reverse, 
                                                   tabSelected.DivisionIndex != _currentPageViewIndex, null);

            _currentPageViewIndex = tabSelected.DivisionIndex;

            _pageViewController.DidFinishAnimating -= OnPageViewController_DidFinishAnimating;
            _pageViewController.DidFinishAnimating += OnPageViewController_DidFinishAnimating;

            foreach(var view in _pageViewController.View.Subviews)
            {
                if(view is UIScrollView)
                {
                    var scrollView = view as UIScrollView;
                    scrollView.Delegate = this;
                }
            }
        }

        private void UpdateTabBar()
        {
            _totalTabs = ViewModel.ContactTab.Count;
            float screenWidth = (float)UIScreen.MainScreen.Bounds.Width;
            var sizeForTab = screenWidth / _totalTabs;

            sizeForTab = sizeForTab < LocalConstants.Contacts_TabMinWidth ? LocalConstants.Contacts_TabMinWidth : sizeForTab;

            if ((int)sizeForTab == (int)LocalConstants.Contacts_TabMinWidth)
                _disableUnderline = true;
            
            int numberTab = 0;
            foreach (var tab in ViewModel.ContactTab)
            {
                var divisionTab = TabBarView.Create;
                divisionTab.Configure(tab);
                divisionTab.Frame = new CGRect((sizeForTab * numberTab), 0, sizeForTab, LocalConstants.Contacts_TabHeight);
                _tabScrollView.AddSubview(divisionTab);

                if (tab.IsSelected)
                {
                    if (_selectedFromTab)
                    {
                        UIView.Animate(0.3f, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
                        {
                            _barView.Frame = new CGRect(divisionTab.Frame.X, _barView.Frame.Y, sizeForTab, _barView.Frame.Height);
                        }, null);
                    }
                    _tabCenter = (float)divisionTab.Center.X;
                }
                
                numberTab++;
            }

            _tabScrollView.ContentInset = new UIEdgeInsets(0, 0, 0, 0);
            _tabScrollView.ContentSize = new CGSize(sizeForTab * _totalTabs, LocalConstants.Contacts_TabHeight);
            _tabScrollView.AutosizesSubviews = false;
            _tabScrollView.LayoutIfNeeded();

            if(!_barViewFrameIsSet)
            {
                _barView.Frame = new CGRect(0, _tabScrollView.Frame.Height - 2, sizeForTab, 2);
                _barViewFrameIsSet = true;
            }
            _barView.Hidden = _disableUnderline;
        }

        private void DidFinishAnimating(bool finished)
        {
            if (finished)
                _selectedFromTab = false;
        }

        private void OnPageViewController_DidFinishAnimating(object sender, UIPageViewFinishedAnimationEventArgs e)
        {
            if (e.Finished)
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
                _barView.Center = new CGPoint((_tabCenter) + move, _barView.Center.Y);
            }
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

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;
        }
    }
}

