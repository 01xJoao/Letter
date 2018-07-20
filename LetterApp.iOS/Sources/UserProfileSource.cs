using System;
using CoreGraphics;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.Cells;
using LetterApp.iOS.Views.TabBar.UserProfileViewController.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class UserProfileSource : UITableViewSource
    {
        private UITableView _tableView;
        private ProfileDetailsModel _details;
        private ProfileDivisionModel _division;

        public UserProfileSource(UITableView tableView, ProfileDetailsModel details, ProfileDivisionModel division)
        {
            _tableView = tableView;
            _details = details;
            _division = division;

            tableView.RegisterNibForCellReuse(DetailsCell.Nib, DetailsCell.Key);
            tableView.RegisterNibForCellReuse(DivisionsCell.Nib, DivisionsCell.Key);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var view = new UIView();
            view.BackgroundColor = Colors.White;

            if(section == (int)Sections.Divisions)
            {
                var label = new UILabel();
                label.Frame = new CGRect(20, 15, UIScreen.MainScreen.Bounds.Width, 15);
                UILabelExtensions.SetupLabelAppearance(label, _division.DivisionDescriptionLabel, Colors.ProfileGray, 12f);
                view.AddSubview(label);
            }

            return view;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            switch (section)
            {
                case (int)Sections.Details: return LocalConstants.Profile_DetailsHeader;
                case (int)Sections.Divisions: return LocalConstants.Profile_DivisionHeader;
                default: return 0;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = new UITableViewCell();

            switch (indexPath.Section)
            {
                case (int)Sections.Details:
                    var detailsCell = tableView.DequeueReusableCell(DetailsCell.Key) as DetailsCell;
                    detailsCell.Configure(_details.Details[indexPath.Row]);
                    cell = detailsCell;
                    break;

                case (int)Sections.Divisions:
                    var divisionsCell = tableView.DequeueReusableCell(DivisionsCell.Key) as DivisionsCell;
                    divisionsCell.Configure(_division);
                    cell = divisionsCell;
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
                case (int)Sections.Divisions: return LocalConstants.Profile_DivisionHeight;
                default: return 0;
            }
        }

        public override nint NumberOfSections(UITableView tableView) => (int)Sections.Count;

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch(section)
            {
                case (int)Sections.Details: return (int)Details.Count;
                case (int)Sections.Divisions: return 1;  
                default: return 0;
            }
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            var offSetY = scrollView.ContentOffset.Y;
            _tableView.TableHeaderView.Layer.MasksToBounds = offSetY > 0;
            var headerView = _tableView.TableHeaderView.Subviews[0] as ProfileHeaderView;
            var frame = headerView.Frame;

            if(offSetY > 0)
            {
                frame.Y = offSetY * 0.5f;
                headerView.Frame = frame;
            }

            if(_tableView.ContentOffset.Y < -20)
            {
                var headerRect = new CGRect(0, -headerView.Frame.Height, _tableView.Frame.Width, headerView.Frame.Height);
                headerRect.Location = new CGPoint(headerRect.Location.X, _tableView.ContentOffset.Y + 22);
                headerRect.Size = new CGSize(headerRect.Size.Width, headerView.Frame.Height);
                headerView.Frame = headerRect;
            }
        }

        private enum Sections
        {
            Details,
            Divisions,
            Count
        }

        private enum Details
        {
            Role,
            Email,
            Mobile,
            Count
        }
    }
}
