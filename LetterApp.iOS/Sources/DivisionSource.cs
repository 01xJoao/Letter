using System;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.Cells;
using LetterApp.iOS.Views.Division.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class DivisionSource : UITableViewSource
    {
        private OrganizationInfoModel _organizationInfo;
        private ProfileDetailsModel _profileDetails;

        public DivisionSource(UITableView tableView, OrganizationInfoModel organizationInfo, ProfileDetailsModel profileDetails)
        {
            _organizationInfo = organizationInfo;
            _profileDetails = profileDetails;
            tableView.RegisterNibForCellReuse(DetailsCell.Nib, DetailsCell.Key);
            tableView.RegisterNibForCellReuse(OrganizationDetailCell.Nib, OrganizationDetailCell.Key);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = new UITableViewCell();

            switch (indexPath.Section)
            {
                case (int)Sections.Details:
                    var detailsCell = tableView.DequeueReusableCell(DetailsCell.Key) as DetailsCell;
                    detailsCell.Configure(_profileDetails.Details[indexPath.Row]);
                    cell = detailsCell;
                    break;

                case (int)Sections.Organization:
                    var organizationDetailCell = tableView.DequeueReusableCell(OrganizationDetailCell.Key) as OrganizationDetailCell;
                    organizationDetailCell.Configure(_organizationInfo);
                    cell = organizationDetailCell;
                    break;
            }

            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case (int)Sections.Details: return LocalConstants.Profile_Details;
                case (int)Sections.Organization: return LocalConstants.Profile_DivisionHeight;
                default: return 0;
            }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            {
                case (int)Sections.Details: return _profileDetails.Details.Count;
                case (int)Sections.Organization: return 1;
                default: return 0;
            }
        }

        public override nint NumberOfSections(UITableView tableView) => (int)Sections.Count;

        private enum Sections 
        {
            Details,
            Organization,
            Count
        }
    }
}
