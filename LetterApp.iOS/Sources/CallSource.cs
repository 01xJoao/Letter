using System;
using System.Collections.Generic;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.TabBar.CallListViewController.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class CallSource : UITableViewSource
    {
        private string _info;
        private string _delete;
        private List<CallHistoryModel> _calls;

        public event EventHandler<int> OpenCallerProfileEvent;
        public event EventHandler<int> CallEvent;
        public event EventHandler<int> DeleteCallEvent;
        public event EventHandler<int> CallStackEvent;

        public CallSource(UITableView tableView, List<CallHistoryModel> calls, string delete, string info)
        {
            _info = info;
            _delete = delete;
            _calls = calls;
            tableView.RegisterNibForCellReuse(CallHistoryCell.Nib, CallHistoryCell.Key);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var callCell = tableView.DequeueReusableCell(CallHistoryCell.Key) as CallHistoryCell;
            callCell.Configure(_calls[indexPath.Row], OpenCallerProfileEvent);
            return callCell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            CallEvent?.Invoke(this, _calls[indexPath.Row].CallerId);
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction deleteButton = UITableViewRowAction.Create(
                UITableViewRowActionStyle.Destructive,
                _delete,
                delegate {
                    tableView.BeginUpdates();
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Bottom);
                    DeleteCallEvent?.Invoke(this, indexPath.Row);
                    tableView.EndUpdates();            
                    tableView.ReloadData();
                });

            deleteButton.BackgroundColor = Colors.Red;

            UITableViewRowAction infoButton = UITableViewRowAction.Create(
                UITableViewRowActionStyle.Normal,
                _info,
                delegate {
                    CallStackEvent?.Invoke(this, indexPath.Row);
                });

            infoButton.BackgroundColor = Colors.Orange;

            return new UITableViewRowAction[] { deleteButton, infoButton };
        }
              
        public override UISwipeActionsConfiguration GetLeadingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.SetEditing(false, true);
            return null;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section) => new UIView();

        public override nfloat GetHeightForHeader(UITableView tableView, nint section) => 8;

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => LocalConstants.CallHistory_Height;

        public override nint RowsInSection(UITableView tableview, nint section) => _calls.Count;

        public override nint NumberOfSections(UITableView tableView) => 1;
    }
}
