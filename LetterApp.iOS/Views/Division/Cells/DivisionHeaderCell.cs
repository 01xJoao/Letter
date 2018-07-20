using System;

using Foundation;
using LetterApp.Core.Models;
using UIKit;

namespace LetterApp.iOS.Views.Division.Cells
{
    public partial class DivisionHeaderCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("DivisionHeaderCell");
        public static readonly UINib Nib = UINib.FromName("DivisionHeaderCell", NSBundle.MainBundle);
        protected DivisionHeaderCell(IntPtr handle) : base(handle) {}

        public void Configure(DivisionHeaderModel division)
        {
            
        }
    }
}
