using System;
using System.Collections.Generic;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Views.TabBar.CallListViewController.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
	public class CallSource : UITableViewSource
    {
        private List<CallModel> _calls;

        public CallSource(UITableView tableView, List<CallModel> calls)
        {
            _calls = calls;

            tableView.RegisterNibForCellReuse(CallCell.Nib, CallCell.Key);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            throw new NotImplementedException();
        }

        public override nint RowsInSection(UITableView tableview, nint section) => _calls.Count;

        public override nint NumberOfSections(UITableView tableView) => 1;
    }
}
