using System;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.TabBarView
{
    public partial class TabBarView : UIView
    {
        public ContactTabModel Division;
        public static readonly UINib Nib = UINib.FromName("TabBarView", NSBundle.MainBundle);
        public TabBarView(IntPtr handle) : base(handle) { }
        public static TabBarView Create => Nib.Instantiate(null, null)[0] as TabBarView;

        public void Configure(ContactTabModel division, bool enableUnderLine = false)
        {
            Division = division;

            UILabelExtensions.SetupLabelAppearance(_label, division.DivisionName, division.IsSelected ? Colors.MainBlue : Colors.Black, 13f, UIFontWeight.Semibold);
            _tabButton.TouchUpInside -= OnTabButton_TouchUpInside;
            _tabButton.TouchUpInside += OnTabButton_TouchUpInside;

            _underLineView.BackgroundColor = Colors.MainBlue;
            _underLineView.Hidden = true;

            if (enableUnderLine && division.IsSelected)
                _underLineView.Hidden = false;
        }

        private void OnTabButton_TouchUpInside(object sender, EventArgs e)
        {
            Division?.DivisionEvent(true, Division.DivisionIndex);
        }
    }
}
