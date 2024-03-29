﻿using System;
using CoreGraphics;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.Cells;
using LetterApp.iOS.Views.Organization.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class OrganizationSource : UITableViewSource
    {
        private ProfileDetailsModel _details;
        private ProfileOrganizationModel _divisions;

        public OrganizationSource(UITableView tableView, ProfileOrganizationModel divisions, ProfileDetailsModel details)
        {
            _details = details;
            _divisions = divisions;

            tableView.RegisterNibForCellReuse(DetailsCell.Nib, DetailsCell.Key);
            tableView.RegisterNibForCellReuse(OrganizationDivisionsCell.Nib, OrganizationDivisionsCell.Key);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var view = new UIView();
            view.BackgroundColor = Colors.White.ColorWithAlpha(0);

            if (section == (int)Sections.Divisions)
            {
                var label = new UILabel();
                label.Frame = new CGRect(20, 15, UIScreen.MainScreen.Bounds.Width, 15);
                UILabelExtensions.SetupLabelAppearance(label, _divisions.DivisionDescriptionLabel, Colors.ProfileGray, 12f);
                view.AddSubview(label);
            }

            return view;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            switch (section)
            {
                case (int)Sections.Details: return 0;
                case (int)Sections.Divisions: return LocalConstants.Profile_DivisionHeader;
                default: return 0;
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case (int)Sections.Details: return LocalConstants.Profile_Details;
                case (int)Sections.Divisions: return LocalConstants.Organization_DivisionsHeight;
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
                    var divisionsCell = tableView.DequeueReusableCell(OrganizationDivisionsCell.Key) as OrganizationDivisionsCell;
                    divisionsCell.Configure(_divisions);
                    cell = divisionsCell;
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
                case (int)Sections.Details: return _details.Details.Count;
                case (int)Sections.Divisions: return 1;
                default: return 0;
            }
        }

        private enum Sections
        {
            Details,
            Divisions,
            Count
        }
    }
}
