using System;
using System.Collections.Generic;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class CallStackSource : UITableViewSource
    {
        private readonly List<CallStackModel> _calls;
        private const string CellId = "Call";

        public CallStackSource(List<CallStackModel> calls)
        {
            _calls = calls;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var call = _calls[indexPath.Row];

            UITableViewCell cell = tableView.DequeueReusableCell(CellId);

            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellId);

            UILabelExtensions.SetupLabelAppearance(cell.TextLabel, call.CallType, call.Successful ? Colors.Green : Colors.Red, 13f, UIFontWeight.Regular);
            UILabelExtensions.SetupLabelAppearance(cell.DetailTextLabel, call.Date, Colors.ProfileGray, 11f, UIFontWeight.Regular);

            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section) => _calls.Count;
    }
}
