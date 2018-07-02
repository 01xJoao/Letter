using System;

using Foundation;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class LeaveOrganizationCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("LeaveOrganizationCell");
        public static readonly UINib Nib = UINib.FromName("LeaveOrganizationCell", NSBundle.MainBundle);
        protected LeaveOrganizationCell(IntPtr handle) : base(handle) {}

        public void Configure(string text)
        {
            UIButtonExtensions.SetupButtonAppearance(_leaveButton, Colors.White, 14f, text);
        }
    }
}
