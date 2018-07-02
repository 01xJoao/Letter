using System;
using System.Collections.Generic;
using Foundation;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.Cells;
using LetterApp.iOS.Views.SelectDivision.Cells;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class SelectDivisionsSource : UITableViewSource
    {
        private List<DivisionModel> _divisions;
        Dictionary<string, string> _locationResources;
        public event EventHandler<DivisionModel> OnDivisionSelectedEvent;
        public event EventHandler<string> OnSubmitButton;

        public SelectDivisionsSource(UITableView tableView, List<DivisionModel> divisions, Dictionary<string, string> locationResources)
        {
            _divisions = divisions;
            _locationResources = locationResources;

            tableView.RegisterNibForCellReuse(SubtitleCell.Nib, DivisionCell.Key);
            tableView.RegisterNibForCellReuse(DivisionCell.Nib, DivisionCell.Key);
            tableView.RegisterNibForCellReuse(LineSeparatorCell.Nib, LineSeparatorCell.Key);
            tableView.RegisterNibForCellReuse(InsertDivisionFieldCell.Nib, InsertDivisionFieldCell.Key);
            tableView.RegisterNibForCellReuse(SelectButtonCell.Nib, SelectButtonCell.Key);
            tableView.RegisterNibForCellReuse(LeaveOrganizationCell.Nib, LeaveOrganizationCell.Key);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            //if(section == (int)Sections.Divisions)
            //{
            //    var header = tableView.DequeueReusableCell(SubtitleCell.Key) as SubtitleCell;
            //    header.Configure(_locationResources["Public"]);
            //    return header;
            //}
            //else if (section ==(int)Sections.InsertDivision)
            //{
            //    var header = tableView.DequeueReusableCell(SubtitleCell.Key) as SubtitleCell;
            //    header.Configure(_locationResources["Private"]);
            //    return header;
            //}
            //else
            //{
            //    return null;
            //}
            return null;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = null;

            switch (indexPath.Section)
            {
                case (int)Sections.Divisions:
                    var divisionCell = tableView.DequeueReusableCell(DivisionCell.Key) as DivisionCell;
                    divisionCell.Configure(_divisions[indexPath.Row]);
                    cell = divisionCell;
                    break;

                case (int)Sections.Separator:
                    var separatorCell = tableView.DequeueReusableCell(LineSeparatorCell.Key) as LineSeparatorCell;
                    cell = separatorCell;
                    break;

                case (int)Sections.InsertDivision:
                    var insertCell = tableView.DequeueReusableCell(InsertDivisionFieldCell.Key) as InsertDivisionFieldCell;
                    insertCell.Configure(_locationResources["Insert"], _locationResources["Submit"], OnSubmitButton);
                    cell = insertCell;
                    break;

                case (int)Sections.LeaveOrganization:
                    var leaveCell = tableView.DequeueReusableCell(LeaveOrganizationCell.Key) as LeaveOrganizationCell;
                    leaveCell.Configure(_locationResources["Leave"]);
                    cell = leaveCell;
                    break;
            }
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        public override nint NumberOfSections(UITableView tableView) => (int)Sections.Count;

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            {
                case (int)Sections.Divisions: return _divisions.Count;
                case (int)Sections.Separator: return 1;
                case (int)Sections.InsertDivision: return 1;
                case (int)Sections.LeaveOrganization: return 1;
                default: return 0;
            }
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section) => LocalConstants.SelectDivision_Header;

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case (int)Sections.Divisions: return LocalConstants.SelectDivision_Division;
                case (int)Sections.Separator: return LocalConstants.SelectDivision_Header;
                case (int)Sections.InsertDivision: return LocalConstants.SelectDivision_InsertDivision;
                case (int)Sections.LeaveOrganization: return LocalConstants.SelectDivision_Header;
                default: return 0;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath) => OnDivisionSelectedEvent?.Invoke(this, _divisions[indexPath.Row]);

        private enum Sections
        {
            Divisions,
            Separator,
            InsertDivision,
            LeaveOrganization,
            Count
        }
    }
}
