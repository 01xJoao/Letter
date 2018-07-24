using System;
using System.ComponentModel;
using CoreGraphics;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using LetterApp.iOS.Views.CustomViews.TabBarView;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.ContactListViewController
{
    public partial class ContactListViewController : XViewController<ContactListViewModel>
    {
        public ContactListViewController() : base("ContactListViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.Title = "Contacts";
            this.NavigationController.NavigationBar.PrefersLargeTitles = true;

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            ConfigurePageView();
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
            ConfigureScrollBar();
        }

        private void ConfigureScrollBar()
        {
            int totalTabs = ViewModel.ContactTab.Count;
            float screenWidth = (float)UIScreen.MainScreen.Bounds.Width;
            float sizeForTab = screenWidth / totalTabs;

            sizeForTab = sizeForTab < 100 ? 100 : sizeForTab;

            int numberTab = 0;
            foreach (var tab in ViewModel.ContactTab)
            {
                var divisionTab = TabBarView.Create;
                divisionTab.Configure(tab);
                divisionTab.Frame = new CGRect((sizeForTab * numberTab), 0, sizeForTab, LocalConstants.Contacts_TabHeight);
                _tabScrollView.AddSubview(divisionTab);
                numberTab++;
            }

            var contentSize = sizeForTab * totalTabs;

            _tabScrollView.ContentInset = new UIEdgeInsets(0, 0, 0, 0);
            _tabScrollView.ContentSize = new CGSize(contentSize, LocalConstants.Profile_DivisionHeight);
            _tabScrollView.AutosizesSubviews = false;
            _tabScrollView.LayoutIfNeeded();
            _tabScrollView.LayoutSubviews();

            _separatorView.BackgroundColor = Colors.GrayDividerContacts;
        }


        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;
        }
    }
}

