using System;
using CoreGraphics;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.ProfileDivision;
using UIKit;

namespace LetterApp.iOS.Views.Organization.Cells
{
    public partial class OrganizationDivisionsCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("OrganizationDivisionsCell");
        public static readonly UINib Nib = UINib.FromName("OrganizationDivisionsCell", NSBundle.MainBundle);
        protected OrganizationDivisionsCell(IntPtr handle) : base(handle) {}

        public void Configure(ProfileOrganizationModel organization)
        {
            int numberOfDivisions = 0;

            foreach (var division in organization.OrganizationDivisions)
            {
                var divisionView = OrganizationDivisionsView.Create;
                divisionView.Configure(division);
                divisionView.Frame = new CGRect((LocalConstants.Organization_DivisionsWidth * numberOfDivisions), 0, LocalConstants.Organization_DivisionsWidth, LocalConstants.Organization_DivisionsHeight);
                _scrollView.AddSubview(divisionView);
                numberOfDivisions++;
            }

            var contentSize = LocalConstants.Organization_DivisionsWidth * (numberOfDivisions);
            this.ContentView.Frame = new CGRect(0, 0, contentSize, LocalConstants.Organization_DivisionsHeight);
            _scrollView.ContentInset = new UIEdgeInsets(0, 5, 0, 0);
            _scrollView.ContentSize = new CGSize(contentSize, LocalConstants.Organization_DivisionsHeight);
            _scrollView.AutosizesSubviews = false;
            _scrollView.LayoutIfNeeded();
            _scrollView.LayoutSubviews();

        }

        public override void PrepareForReuse()
        {
            foreach (var subview in _scrollView.Subviews)
            {
                subview?.RemoveFromSuperview();
                subview?.Dispose();
            }
        }
    }
}
