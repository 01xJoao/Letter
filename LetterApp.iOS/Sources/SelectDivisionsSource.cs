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
        private UITableView _tableView;
        private List<DivisionModel> _divisions;
        private Dictionary<string, string> _locationResources;
        private EventHandler<bool> _scrollsToRowEvent;
        private bool _showLeaveOrganization;

        public event EventHandler<DivisionModel> DivisionSelectedEvent;
        public event EventHandler<string> SubmitButtonEvent;
        public event EventHandler LeaveOrganizationEvent;

        public SelectDivisionsSource(UITableView tableView, List<DivisionModel> divisions, Dictionary<string, string> locationResources, bool showLeaveOrganization)
        {
            _divisions = divisions;
            _locationResources = locationResources;
            _tableView = tableView;
            _showLeaveOrganization = showLeaveOrganization;

            _scrollsToRowEvent -= ScrollsToRow;
            _scrollsToRowEvent += ScrollsToRow;

            tableView.RegisterNibForCellReuse(TitleHeader.Nib, TitleHeader.Key);
            tableView.RegisterNibForCellReuse(DivisionCell.Nib, DivisionCell.Key);
            tableView.RegisterNibForCellReuse(LineSeparatorCell.Nib, LineSeparatorCell.Key);
            tableView.RegisterNibForCellReuse(InsertDivisionFieldCell.Nib, InsertDivisionFieldCell.Key);
            tableView.RegisterNibForCellReuse(LeaveOrganizationCell.Nib, LeaveOrganizationCell.Key);
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (section == (int)Sections.Divisions || section == (int)Sections.InsertDivision)
                return LocalConstants.SelectDivision_Header;
                    
            return 0;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            if(section == (int)Sections.Divisions)
            {
                var header = tableView.DequeueReusableCell(TitleHeader.Key) as TitleHeader;
                header.Configure(_locationResources["Public"]);
                return header;
            }
            else if (section ==(int)Sections.InsertDivision)
            {
                var header = tableView.DequeueReusableCell(TitleHeader.Key) as TitleHeader;
                header.Configure(_locationResources["Private"]);
                return header;
            }
            else
            {
                return null;
            }
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
                    cell.SelectedBackgroundView = new UIView { BackgroundColor = Colors.MainBlue.ColorWithAlpha(0.5f) };
                    break;

                case (int)Sections.Separator:
                    var separatorCell = tableView.DequeueReusableCell(LineSeparatorCell.Key) as LineSeparatorCell;
                    cell = separatorCell;
                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                    break;

                case (int)Sections.InsertDivision:
                    var insertCell = tableView.DequeueReusableCell(InsertDivisionFieldCell.Key) as InsertDivisionFieldCell;
                    insertCell.Configure(_locationResources["Insert"], _locationResources["Submit"], SubmitButtonEvent, _scrollsToRowEvent);
                    cell = insertCell;
                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                    break;

                case (int)Sections.LeaveOrganization:
                    var leaveCell = tableView.DequeueReusableCell(LeaveOrganizationCell.Key) as LeaveOrganizationCell;
                    leaveCell.Configure(_locationResources["Leave"], LeaveOrganizationEvent);
                    cell = leaveCell;
                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                    break;
            }
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
                case (int)Sections.LeaveOrganization: return _showLeaveOrganization ? 1 : 0;
                default: return 0;
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case (int)Sections.Divisions: return LocalConstants.SelectDivision_Division;
                case (int)Sections.Separator: return LocalConstants.SelectDivision_Separator;
                case (int)Sections.InsertDivision: return LocalConstants.SelectDivision_InsertDivision;
                case (int)Sections.LeaveOrganization: return LocalConstants.SelectDivision_Header;
                default: return 0;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if(indexPath.Section == (int)Sections.Divisions)
            {
                DivisionSelectedEvent?.Invoke(this, _divisions[indexPath.Row]);
                tableView.DeselectRow(indexPath, true);
            }
        }

        private void ScrollsToRow(object sender, bool shouldAnimate)
        {
            _tableView.ScrollToRow(NSIndexPath.FromItemSection(0, 2), UITableViewScrollPosition.Top, true);
            UIViewAnimationExtensions.AnimateView(_tableView, LocalConstants.SelectDivision_ViewHeight, shouldAnimate);
            _tableView.ScrollEnabled = !shouldAnimate;
        }

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
