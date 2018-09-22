using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.Chat.Cells
{
    public partial class DateHeaderCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("DateHeaderCell");
        public static readonly UINib Nib = UINib.FromName("DateHeaderCell", NSBundle.MainBundle);
        protected DateHeaderCell(IntPtr handle) : base(handle){}

        public void Configure(string date)
        {

        }
    }
}
