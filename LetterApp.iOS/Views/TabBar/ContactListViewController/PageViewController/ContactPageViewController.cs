using System;
using System.Collections.Generic;
using LetterApp.Core.Models;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.ContactListViewController.PageViewController
{
    public partial class ContactPageViewController : XBoardPageViewController
    {
        private int _index;
        private EventHandler<Tuple<ContactListViewModel.ContactEventType, int>> _contactEvent;
        private List<GetUsersInDivisionModel> _contactPage;

        public ContactPageViewController(int index, List<GetUsersInDivisionModel> contactPage, EventHandler<Tuple<ContactListViewModel.ContactEventType, int>> contactEvent) 
            : base(index, "ContactPageViewController", null)
        {
            _index = index;
            _contactPage = contactPage;
            _contactEvent = contactEvent;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupTableView();
        }

        public void SetupTableView(List<GetUsersInDivisionModel> contactPage = null)
        {
            var contact = contactPage == null ? _contactPage : contactPage;

            var source = new ContactsSource(_tableView, contact);

            source.ContactEvent -= OnSource_ContactEvent;
            source.ContactEvent += OnSource_ContactEvent;

            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _tableView.Source = source;
            _tableView.ReloadData();
        }

        private void OnSource_ContactEvent(object sender, Tuple<ContactListViewModel.ContactEventType, int> contact)
        {
            _contactEvent?.Invoke(this, contact);
        }
    }
}

