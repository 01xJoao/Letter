using System;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class MemberSource : UITableViewSource
    {
        private ProfileDetailsModel _memberDetails;

        public MemberSource(UITableView tableView, ProfileDetailsModel memberDetails)
        {
            _memberDetails = memberDetails;
            tableView.RegisterNibForCellReuse(DetailsCell.Nib, DetailsCell.Key);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var detailsCell = tableView.DequeueReusableCell(DetailsCell.Key) as DetailsCell;
            detailsCell.Configure(_memberDetails.Details[indexPath.Row]);
            detailsCell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return detailsCell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => LocalConstants.Profile_Details;

        public override nint NumberOfSections(UITableView tableView) => 1;

        public override nint RowsInSection(UITableView tableview, nint section) => _memberDetails.Details.Count;
    }
}
