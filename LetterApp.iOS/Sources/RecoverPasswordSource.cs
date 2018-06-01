//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Foundation;
//using LetterApp.iOS.Helpers;
//using LetterApp.iOS.Views.CustomViews.Cells;
//using LetterApp.Models.DTO.RequestModels;
//using UIKit;

//namespace LetterApp.iOS.Sources
//{
//    public class RecoverPasswordSource : UITableViewSource
//    {
//        public bool IsAnimated;
//        private UIView _backgroundView;
//        private Dictionary<string, string> _locationResources;
//        private EventHandler<Tuple<int, int>> ScrollsToRowEvent;
//        private PasswordChangeRequestModel _passwordModel;
//        [Weak] private UITableView _tableView;

//        protected RecoverPasswordSource(UITableView tableView, Dictionary<string, string> locationResources, UIView backgroundView, PasswordChangeRequestModel passwordModel)
//        {
//            _locationResources = locationResources;
//            _tableView = tableView;
//            _backgroundView = backgroundView;
//            _passwordModel = passwordModel;

//            ScrollsToRowEvent -= ScrollsToRow;
//            ScrollsToRowEvent += ScrollsToRow;

//            tableView.RegisterNibForCellReuse(HeaderCell.Nib, HeaderCell.Key);
//            //tableView.RegisterNibForCellReuse(FormCell.Nib, FormCell.Key);
//        }

//        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
//        {
//            UITableViewCell cell = null;

//            switch (indexPath.Section)
//            {
//                case (int)Sections.Header:
//                    var headeCell = tableView.DequeueReusableCell(HeaderCell.Key) as HeaderCell;
//                    cell = headeCell;
//                    break;

//                case (int)Sections.PasswordForm:
//                    var formCell = tableView.DequeueReusableCell(FormCell.Key) as FormCell;
//                    string dictionaryKey = _locationResources?.ElementAt(indexPath.Row).Key;
//                    formCell.Configure(_locationResources[dictionaryKey], _passwordModel, _backgroundView, indexPath.Row, ScrollsToRowEvent, _locationResources.Count == indexPath.Row);
//                    cell = formCell;
//                    break;
//                case (int)Sections.Code:
//                    var agreementCell = tableView.DequeueReusableCell(AgreementCell.Key) as AgreementCell;
//                    agreementCell.Configure(_locationResources["agreement"], _registerForm);
//                    cell = agreementCell;
//                    break;
//            }
//            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
//            return cell;
//        }

//        private void ScrollsToRow(object sender, Tuple<int, int> index)
//        {
//            _tableView.ScrollToRow(NSIndexPath.FromItemSection(index.Item1, index.Item2), UITableViewScrollPosition.Middle, true);

//            if ((index.Item1 == 0 && index.Item2 == 2) && !IsAnimated)
//            {
//                IsAnimated = true;
//                UIViewAnimationExtensions.AnimateBackgroundView(_backgroundView, LocalConstants.RecoverPass_Height, true);
//            }
//            else
//            {
//                IsAnimated = false;
//                UIViewAnimationExtensions.AnimateBackgroundView(_backgroundView, 0, false);
//            }
//        }

//        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
//        {
//            switch (indexPath.Section)
//            {
//                case (int)Sections.Header: return LocalConstants.Register_HeaderHeight;
//                case (int)Sections.PasswordForm: return LocalConstants.Register_From;
//                case (int)Sections.Code: return LocalConstants.Register_From;
//                default: return 0;
//            }
//        }

//        public override nint RowsInSection(UITableView tableview, nint section)
//        {
//            switch (section)
//            {
//                case (int)Sections.Header: return 1;
//                case (int)Sections.PasswordForm: return (int)PasswordForm.Count;
//                case (int)Sections.Code: return 1;
//                default: return 0;
//            }
//        }

//        private enum Sections
//        {
//            Header,
//            PasswordForm,
//            Code,
//            Count
//        }

//        private enum PasswordForm
//        {
//            Password,
//            VerifyPassword,
//            Count
//        }
//    }
//}
