using System;
using CoreGraphics;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.ProfileDivision;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.UserProfileViewController.Cells
{
    public partial class DivisionsCell : UITableViewCell, IUIScrollViewDelegate
    {
        public static readonly NSString Key = new NSString("DivisionsCell");
        public static readonly UINib Nib = UINib.FromName("DivisionsCell", NSBundle.MainBundle);
        protected DivisionsCell(IntPtr handle) : base(handle) {}

        public void Configure(ProfileDivisionModel divisionModels)
        {

            _scrollView.Delegate = this;

            int numberOfDivisions = 0;
            foreach(var division in divisionModels.Divisions)
            {
                var divisionView = DivisionView.Create;
                divisionView.Configure(division, divisionModels.DivisionPressedEvent);
                divisionView.Frame = new CGRect((LocalConstants.Profile_DivisionWidth * numberOfDivisions), 0, LocalConstants.Profile_DivisionWidth, LocalConstants.Profile_DivisionHeight);
                _scrollView.AddSubview(divisionView);
                numberOfDivisions++;
            }

            var addDivisionModel = new ProfileDivision();
            addDivisionModel.AddButtonImage = true;
            addDivisionModel.Id = 0;
            var divisionButtonView = DivisionView.Create;
            divisionButtonView.Configure(addDivisionModel, divisionModels.AddDivisionEvent);
            divisionButtonView.Frame = new CGRect((LocalConstants.Profile_DivisionWidth * numberOfDivisions), 0, LocalConstants.Profile_DivisionWidth, LocalConstants.Profile_DivisionHeight);
            _scrollView.AddSubview(divisionButtonView);

            var contentSize = LocalConstants.Profile_DivisionWidth * (divisionModels.Divisions.Count + 1);
            this.ContentView.Frame = new CGRect(0, 0, contentSize, LocalConstants.Profile_DivisionHeight);
        }


        #region IUIScrollViewDelegate

        [Export("scrollViewDidScroll:")]
        public void Scrolled(UIScrollView scrollView)
        {
            //
        }

        #endregion


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
