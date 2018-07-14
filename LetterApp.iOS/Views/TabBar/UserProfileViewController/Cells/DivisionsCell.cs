using System;
using System.Collections.Generic;
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
            var frame = new CGRect(0, 0, LocalConstants.Profile_DivisionWidth, LocalConstants.Profile_DivisionHeight);
            int i = 0;
            foreach(var division in divisionModels.Divisions)
            {
                var divisionView = DivisionView.Create(division, divisionModels.DivisionPressedEvent);
                divisionView.Frame = new CGRect(LocalConstants.Profile_DivisionWidth * i, 0, 0, LocalConstants.Profile_DivisionHeight);
                _scrollView.AddSubview(divisionView);
                i++;
            }

            var addDivisionModel = new ProfileDivision();
            addDivisionModel.AddButtonImage = true;
            addDivisionModel.Id = 0;

            var divisionButtonView = DivisionView.Create(addDivisionModel, divisionModels.AddDivisionEvent);
            divisionButtonView.Frame = new CGRect(LocalConstants.Profile_DivisionWidth * i, 0, 0, LocalConstants.Profile_DivisionHeight);
            _scrollView.AddSubview(divisionButtonView);

            this.ContentView.Frame = new CGRect(0,0,400, frame.Height);
            _scrollView.ContentSize = new CGSize(LocalConstants.Profile_DivisionWidth * i + LocalConstants.Profile_DivisionWidth, frame.Height);

            _scrollView.SetContentOffset(new CGPoint(_scrollView.ContentSize.Width * 0.5f - LocalConstants.Profile_DivisionWidth * 0.5f, 0), true);
            _scrollView.ContentOffset = new CGPoint(400, 0);

            _scrollView.Delegate = this;
        }


        #region IUIScrollViewDelegate

        [Export("scrollViewDidScroll:")]
        public void Scrolled(UIScrollView scrollView)
        {
            _scrollView.ContentSize = new CGSize(_scrollView.ContentSize.Width, _scrollView.Frame.Size.Height);
        }

        #endregion
    }
}
