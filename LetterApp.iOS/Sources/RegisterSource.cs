using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using LetterApp.Core.Models;
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
        private RegisterFormModel _registerForm;
        private Dictionary<string, string> _locationResources;
        private EventHandler<int> ScrollsToRowEvent;
        private UITableView _tableView;

        public RegisterSource(UITableView tableView, Dictionary<string, string> locationResources, RegisterFormModel registerForm, UIView backgroundView)
        {
            _tableView = tableView;
            _backgroundView = backgroundView;
            _locationResources = locationResources;
            _registerForm = registerForm;

            ScrollsToRowEvent -= ScrollsToRow;
            ScrollsToRowEvent += ScrollsToRow;

            tableView.RegisterNibForCellReuse(HeaderCell.Nib, HeaderCell.Key);
            tableView.RegisterNibForCellReuse(FormCell.Nib, FormCell.Key);
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
                    var formCell = tableView.DequeueReusableCell(FormCell.Key) as FormCell;
                    string dictionaryKey = _locationResources?.ElementAt(indexPath.Row).Key;
                    formCell.Configure(_locationResources[dictionaryKey], _registerForm, _backgroundView, indexPath.Row, ScrollsToRowEvent, _locationResources.Count == indexPath.Row);
                    cell = formCell;
                    break;
                case (int)Sections.Agreement:
                    var agreementCell = tableView.DequeueReusableCell(AgreementCell.Key) as AgreementCell;
                    agreementCell.Configure(_locationResources["agreement"], _registerForm);
                    cell = agreementCell;
                    break;
            }
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        private void ScrollsToRow(object sender, int row)
        {
            _tableView.ScrollToRow(NSIndexPath.FromItemSection(row, 1), UITableViewScrollPosition.Middle, true);

            if((row == 4 || row == 5) && !IsAnimated)
            {
                UIViewAnimationExtensions.AnimateBackgroundView(_backgroundView, LocalConstants.Register_ViewHeight, true);
                IsAnimated = true;
            }
            else if (row != 4 && row != 5 && IsAnimated)
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
                case (int)Sections.Form: return LocalConstants.Register_From;
                case (int)Sections.Agreement: return StringExtensions.StringHeight(_locationResources["agreement"], UIFont.SystemFontOfSize(15f), UIScreen.MainScreen.Bounds.Width - 120) + 50;
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
