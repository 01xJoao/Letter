using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.CallListViewController.Cells
{
    public partial class CallCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("CallCell");
        public static readonly UINib Nib = UINib.FromName("CallCell", NSBundle.MainBundle);
        protected CallCell(IntPtr handle) : base(handle){}

        public void Configure()
        {
            
        }
    }
}
