using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.Core.Models.Cells;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.Cells;
using LetterApp.iOS.Views.Register.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class RegisterSource : UITableViewSource
    {
        public bool IsAnimated;
        private UIView _backgroundView;
        private EventHandler<NSIndexPath> _scrollsToRowEvent;
        private UITableView _tableView;
        private List<FormModel> _formModels;
        private string _agreement;
        public EventHandler<bool> AgreementToogleEvent;

        public RegisterSource(UITableView tableView, UIView backgroundView, List<FormModel> formModels, string agreement)
        {
            _tableView = tableView;
            _backgroundView = backgroundView;
            _formModels = formModels;
            _agreement = agreement;

            _scrollsToRowEvent -= ScrollsToRow;
            _scrollsToRowEvent += ScrollsToRow;

            tableView.RegisterNibForCellReuse(HeaderCell.Nib, HeaderCell.Key);
            tableView.RegisterNibForCellReuse(TextFieldCell.Nib, TextFieldCell.Key);
            tableView.RegisterNibForCellReuse(AgreementCell.Nib, AgreementCell.Key);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = null;

            switch(indexPath.Section)
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

                case (int)Sections.Agreement:
                    var agreementCell = tableView.DequeueReusableCell(AgreementCell.Key) as AgreementCell;
                    agreementCell.Configure(_agreement, AgreementToogleEvent);
                    cell = agreementCell;
                    break;
            }
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        private void ScrollsToRow(object sender, NSIndexPath indexPath)
        {
            _tableView.ScrollToRow(NSIndexPath.FromItemSection(indexPath.Row, indexPath.Section), UITableViewScrollPosition.Middle, true);

            if((indexPath.Row == 4 || indexPath.Row == 5) && !IsAnimated)
            {
                UIViewAnimationExtensions.AnimateBackgroundView(_backgroundView, LocalConstants.Register_ViewHeight, true);
                IsAnimated = true;
            }
            else if (indexPath.Row != 4 && indexPath.Row != 5 && IsAnimated)
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
                case (int)Sections.Agreement: return StringExtensions.StringHeight(_agreement, UIFont.SystemFontOfSize(15f), UIScreen.MainScreen.Bounds.Width - 120) + 50;
                default: return 0;
            }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            {
                case (int)Sections.Header: return 1;
                case (int)Sections.Form: return (int)Form.Count;
                case (int)Sections.Agreement: return 1;
                default: return 0;
            }
        }

        public override nint NumberOfSections(UITableView tableView) => (int)Sections.Count;

        private enum Sections
        {
            Header,
            Form,
            Agreement,
            Count
        }

        private enum Form
        {
            Firstname,
            LastName,
            Email,
            Password,
            VerifyPassword,
            Number,
            Count
        }
    }
}
