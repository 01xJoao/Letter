using System;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.TabBarView
{
    [Register("TabBarView")]
    public partial class TabBarView : UIView
    {
        private ContactTabModel _division;
        public static readonly UINib Nib = UINib.FromName("TabBarView", NSBundle.MainBundle);
        public TabBarView(IntPtr handle) : base(handle) { }
        public static TabBarView Create => Nib.Instantiate(null, null)[0] as TabBarView;

        public void Configure(ContactTabModel division)
        {
            _division = division;
            UIButtonExtensions.SetupButtonAppearance(_tabButton, division.IsSelected ? Colors.MainBlue : Colors.Black, 13f, division.DivisionName, UIFontWeight.Semibold);

            _tabButton.TouchUpInside -= OnTabButton_TouchUpInside;
            _tabButton.TouchUpInside += OnTabButton_TouchUpInside;
        }

        private void OnTabButton_TouchUpInside(object sender, EventArgs e)
        {
            _division?.DivisionEvent(this, _division.DivisionIndex);
        }
    }
}
