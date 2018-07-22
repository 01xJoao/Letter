using System;

using Foundation;
using LetterApp.Core.Models;
using UIKit;

namespace LetterApp.iOS.Views.Organization.Cells
{
    public partial class OrganizationDivisionsCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("OrganizationDivisionsCell");
        public static readonly UINib Nib = UINib.FromName("OrganizationDivisionsCell", NSBundle.MainBundle);
        protected OrganizationDivisionsCell(IntPtr handle) : base(handle) {}

        public void Configure(ProfileOrganizationModel divisions)
        {
            
        }
    }
}
