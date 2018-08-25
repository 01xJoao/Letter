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
        public event EventHandler<int> OpenCallerProfileEvent;
        public event EventHandler<int> CallEvent;
        private List<CallHistoryModel> _calls;

        public CallSource(UITableView tableView, List<CallHistoryModel> calls)
        {
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

        public override UIView GetViewForHeader(UITableView tableView, nint section) => new UIView();

        public override nfloat GetHeightForHeader(UITableView tableView, nint section) => 8;

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => LocalConstants.CallHistory_Height;

        public override nint RowsInSection(UITableView tableview, nint section) => _calls.Count;

        public override nint NumberOfSections(UITableView tableView) => 1;
    }
}
