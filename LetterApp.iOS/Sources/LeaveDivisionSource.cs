using System;
using System.Collections.Generic;
using Foundation;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.LeaveDivision.Cells;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class LeaveDivisionSource : UITableViewSource
    {
        private UITableView _tableView;
        private string _leaveDivision;
        private List<DivisionModel> _divisions;
        public event EventHandler<DivisionModel> LeaveDivisionEvent;

        public LeaveDivisionSource(UITableView tableView, List<DivisionModel> divisions, string leaveDivision)
        {
            _tableView = tableView;
            _divisions = divisions;
            _leaveDivision = leaveDivision;
            tableView.RegisterNibForCellReuse(LeaveDivisionCell.Nib, LeaveDivisionCell.Key);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(LeaveDivisionCell.Key) as LeaveDivisionCell;
            cell.Configure(_divisions[indexPath.Row], _leaveDivision, DivisionLeftEvent, indexPath);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => LocalConstants.LeaveDivision_CellHeight;

        public override nint RowsInSection(UITableView tableview, nint section) => _divisions.Count;

        private void DivisionLeftEvent(object sender, Tuple<NSIndexPath, DivisionModel> division)
        {
            _tableView.BeginUpdates();
            _tableView.DeleteRows(new NSIndexPath[] { division.Item1 }, UITableViewRowAnimation.Bottom);
            _divisions.RemoveAt(division.Item1.Row);
            _tableView.EndUpdates();
            _tableView.ReloadData();

            LeaveDivisionEvent?.Invoke(sender, division.Item2);
        }
    }
}
