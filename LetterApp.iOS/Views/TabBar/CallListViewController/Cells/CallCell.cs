using System;

using Foundation;
using LetterApp.Core.Models;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.CallListViewController.Cells
{
    public partial class CallCell : UITableViewCell
    {
        private int _callerId;
        EventHandler<int> _openProfile;
        public static readonly NSString Key = new NSString("CallCell");
        public static readonly UINib Nib = UINib.FromName("CallCell", NSBundle.MainBundle);
        protected CallCell(IntPtr handle) : base(handle){}

        public void Configure(CallModel call, EventHandler<int> openProfile)
        {
            _callerId = call.CallerId;
            _openProfile = openProfile;
        }
    }
}
