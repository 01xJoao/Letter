using System;
using System.Collections.Generic;
using Foundation;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.TabBarView;
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

        public ContactsSource(UITableView tableView, List<GetUsersInDivisionModel> contacts)
        {
            _contacts = contacts;

            tableView.RegisterNibForCellReuse(ContactsCell.Nib, ContactsCell.Key);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var contactCell = tableView.DequeueReusableCell(ContactsCell.Key) as ContactsCell;
            contactCell.Configure(_contacts[indexPath.Row], ContactEvent);
            contactCell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return contactCell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => LocalConstants.Contacts_CellHeight;

        public override nint RowsInSection(UITableView tableview, nint section) => _contacts.Count;
    }
}
