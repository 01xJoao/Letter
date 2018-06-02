using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using LetterApp.Core.Models.Cells;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class RecoverPasswordSource : UITableViewSource
    {
        public bool IsAnimated;
        private UIView _backgroundView;
        private EventHandler<NSIndexPath> _scrollsToRowEvent;
        private List<FormModel> _formModels;
        private UITableView _tableView;

        public RecoverPasswordSource(UITableView tableView, UIView backgroundView, List<FormModel> formModels)
        {
            _tableView = tableView;
            _backgroundView = backgroundView;
            _formModels = formModels;

            _scrollsToRowEvent -= ScrollsToRow;
            _scrollsToRowEvent += ScrollsToRow;

            tableView.RegisterNibForCellReuse(HeaderCell.Nib, HeaderCell.Key);
            tableView.RegisterNibForCellReuse(TextFieldCell.Nib, TextFieldCell.Key);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            if (section == (int)Sections.Code)
                return new UIView();
            
            return null;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (section == (int)Sections.Code)
                return LocalConstants.RecoverPass_CodeHeaderHeight;

            return 0;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = null;

            switch (indexPath.Section)
            {
                case (int)Sections.Header:
                    var headeCell = tableView.DequeueReusableCell(HeaderCell.Key) as HeaderCell;
                    cell = headeCell;
                    break;
                case (int)Sections.Form:
                    var formCell = tableView.DequeueReusableCell(TextFieldCell.Key) as TextFieldCell;
                    formCell.Configure(_formModels[indexPath.Row], indexPath, _scrollsToRowEvent, _backgroundView);
                    cell = formCell;
                    break;
                case (int)Sections.Code:
                    var formCodeCell = tableView.DequeueReusableCell(TextFieldCell.Key) as TextFieldCell;
                    formCodeCell.Configure(_formModels.Last(), indexPath, _scrollsToRowEvent, _backgroundView);
                    cell = formCodeCell;
                    break;
            }
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        private void ScrollsToRow(object sender, NSIndexPath index)
        {
            _tableView.ScrollToRow(index, UITableViewScrollPosition.Middle, true);

            if ((index.Row == 0 && index.Section == 2) && !IsAnimated)
            {
                IsAnimated = true;
                UIViewAnimationExtensions.AnimateBackgroundView(_backgroundView, LocalConstants.RecoverPass_Height, true);
            }
            else
            {
                IsAnimated = false;
                UIViewAnimationExtensions.AnimateBackgroundView(_backgroundView, 0, false);
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case (int)Sections.Header: return LocalConstants.Register_HeaderHeight;
                case (int)Sections.Form: return LocalConstants.Register_Form;
                case (int)Sections.Code: return LocalConstants.Register_Form;
                default: return 0;
            }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            {
                case (int)Sections.Header: return 1;
                case (int)Sections.Form: return (int)Form.Count;
                case (int)Sections.Code: return 1;
                default: return 0;
            }
        }

        public override nint NumberOfSections(UITableView tableView) => (int)Sections.Count;

        private enum Sections
        {
            Header,
            Form,
            Code,
            Count
        }

        private enum Form
        {
            Email,
            Password,
            VerifyPassword,
            Count
        }
    }
}
