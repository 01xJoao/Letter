using System;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Views.CustomViews.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class MemberSource : UITableViewSource
    {
        private MemberDetailsModel _memberDetails;

        public MemberSource(UITableView tableView, MemberDetailsModel memberDetails)
        {
            _memberDetails = memberDetails;
            tableView.RegisterNibForCellReuse(DetailsCell.Nib, DetailsCell.Key);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            throw new NotImplementedException();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            throw new NotImplementedException();
        }
    }
}
