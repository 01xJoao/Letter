using System;

using Foundation;
using LetterApp.Core.Models;
using UIKit;

namespace LetterApp.iOS.Views.Division.Cells
{
    public partial class OrganizationDetailCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("OrganizationDetailCell");
        public static readonly UINib Nib = UINib.FromName("OrganizationDetailCell", NSBundle.MainBundle);
        protected OrganizationDetailCell(IntPtr handle) : base(handle) {}

        public void Configure(OrganizationInfoModel organizationInfoModel)
        {
            
        }
    }
}
