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
        [Weak] private UIView _viewControllerView;
        private RegisterFormModel registerForm = new RegisterFormModel();
        private Dictionary<string, string> _locationResources = new Dictionary<string, string>();
        public event EventHandler<RegisterFormModel> OnSubmitClickEvent;

        public RegisterSource(UITableView tableView, Dictionary<string, string> locationResources, UIView viewControllerView)
        {
            _viewControllerView = viewControllerView;
            _locationResources = locationResources;

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
                    formCell.Configure(_locationResources[dictionaryKey], registerForm, _viewControllerView, indexPath.Row, (int)_locationResources.Count == indexPath.Row);
                    cell = formCell;
                    break;
                case (int)Sections.Agreement:
                    var agreementCell = tableView.DequeueReusableCell(AgreementCell.Key) as AgreementCell;
                    agreementCell.Configure(_locationResources["agreement"], registerForm);
                    cell = agreementCell;
                    break;
            }
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
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
            Number,
            Password,
            VerifyPassword,
            Count
        }
    }
}
