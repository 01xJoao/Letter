using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using LetterApp.iOS.Views.CustomViews.TabBarView;
using LetterApp.iOS.Views.TabBar.ContactListViewController.PageViewController;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.ContactListViewController
{
    public partial class ContactListViewController : XViewController<ContactListViewModel>
    {
        private UIPageViewController _pageViewController;

        public ContactListViewController() : base("ContactListViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.Title = "Contacts";
            this.NavigationController.NavigationBar.PrefersLargeTitles = true;

            _pageViewController = new UIPageViewController(UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation.Horizontal);

            ConfigurePageView();

            this.AddChildViewController(_pageViewController);
            _pageView.AddSubview(_pageViewController.View);

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.UpdateView):
                    ConfigurePageView();
                    break;
                default:
                    break;
            }
        }

        private void ConfigurePageView()
        {
            if (ViewModel.ContactLists.Contacts.Count == 0)
                return;

            ConfigureScrollBar();

            var viewControllers = new List<XBoardPageViewController>();

            int divisionView = 0;
            foreach(var divisionList in ViewModel.ContactLists.Contacts)
            {
                var division = new ContactPageViewController(divisionView, divisionList, ContactEvent);
                viewControllers.Add(division);
                divisionView++;
            }

            var pageSource = new PageSource(viewControllers);
            _pageViewController.DataSource = pageSource;
            _pageViewController.SetViewControllers(new UIViewController[] { viewControllers.FirstOrDefault() }, UIPageViewControllerNavigationDirection.Forward, false, null);
        }

        private void ContactEvent(object sender, Tuple<ContactListViewModel.ContactEventType, int> contact)
        {
            if (ViewModel.ContactCommand.CanExecute(contact))
                ViewModel.ContactCommand.Execute(contact);
        }

        private void ConfigureScrollBar()
        {
            int totalTabs = ViewModel.ContactTab.Count;
            float screenWidth = (float)UIScreen.MainScreen.Bounds.Width;
            float sizeForTab = screenWidth / totalTabs;

            sizeForTab = sizeForTab < LocalConstants.Contacts_TabMinWidth ? LocalConstants.Contacts_TabMinWidth : sizeForTab;

            int numberTab = 0;
            foreach (var tab in ViewModel.ContactTab)
            {
                var divisionTab = TabBarView.Create;
                divisionTab.Configure(tab);
                divisionTab.Frame = new CGRect((sizeForTab * numberTab), 0, sizeForTab, LocalConstants.Contacts_TabHeight);
                _tabScrollView.AddSubview(divisionTab);
                numberTab++;
            }

            _tabScrollView.ContentInset = new UIEdgeInsets(0, 0, 0, 0);
            _tabScrollView.ContentSize = new CGSize(sizeForTab * totalTabs, LocalConstants.Profile_DivisionHeight);
            _tabScrollView.AutosizesSubviews = false;
            _tabScrollView.LayoutIfNeeded();

            _separatorView.BackgroundColor = Colors.GrayDividerContacts;
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

