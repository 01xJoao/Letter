﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.TabBar.ContactListViewController.Cells;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;
using static LetterApp.Core.ViewModels.TabBarViewModels.ContactListViewModel;

namespace LetterApp.iOS.Sources
{
    public class ContactsSource : UITableViewSource
    {
        public event EventHandler<Tuple<ContactEventType, int>> ContactEvent;
        private List<GetUsersInDivisionModel> _contacts;
        private IGrouping<string, GetUsersInDivisionModel>[] _grouping;
        private List<string> _indices = new List<string>();
        private ContactsType _showContactType;

        public ContactsSource(UITableView tableView, List<GetUsersInDivisionModel> contacts, ContactsType showContactType, bool filterByName)
        {
            _contacts = contacts;
            tableView.RegisterNibForCellReuse(ContactsCell.Nib, ContactsCell.Key);


            if (filterByName)
            {
                _grouping = (from c in contacts
                             orderby c.FirstName[0] ascending
                             group c by c.FirstName[0].ToString() into g
                             select g).ToArray();

                _indices = (from c in contacts
                            orderby c.FirstName ascending
                            group c by c.FirstName[0] into i
                            select i.Key.ToString()).ToList();
            }
            else
            {
                _grouping = (from c in contacts
                             orderby c.Position[0] ascending
                             group c by c.Position into g
                             select g).ToArray();

                _indices = (from c in contacts
                            orderby c.Position ascending
                            group c by c.Position[0] into i
                            select i.Key.ToString()).ToList();
            }

            _indices.Insert(0, UITableView.IndexSearch);

            _showContactType = showContactType;
        }

        public override string TitleForHeader(UITableView tableView, nint section) => _grouping[section].Key.ToString();

        public override nfloat GetHeightForHeader(UITableView tableView, nint section) => LocalConstants.Contacts_TitleHeight;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (_contacts.ElementAtOrDefault(indexPath.Row) == null)
                return new UITableViewCell();

            var contactCell = tableView.DequeueReusableCell(ContactsCell.Key) as ContactsCell;
            contactCell.Configure(_grouping[indexPath.Section].ElementAt(indexPath.Row), ContactEvent, _showContactType);


            return contactCell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            ContactEvent?.Invoke(this, new Tuple<ContactEventType, int>(ContactEventType.OpenProfile, _grouping[indexPath.Section].ElementAt(indexPath.Row).UserId));
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => LocalConstants.Contacts_CellHeight;

        public override nint RowsInSection(UITableView tableview, nint section) => _grouping[section].Count();

        public override nint NumberOfSections(UITableView tableView) => _grouping.Length;

        public override string[] SectionIndexTitles(UITableView tableView) => _indices.ToArray();
    }
}
