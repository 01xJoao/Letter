using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class LeaveOrganizationCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("LeaveOrganizationCell");
        public static readonly UINib Nib;

        static LeaveOrganizationCell()
        {
            Nib = UINib.FromName("LeaveOrganizationCell", NSBundle.MainBundle);
        }

        protected LeaveOrganizationCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
