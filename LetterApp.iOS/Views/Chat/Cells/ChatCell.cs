using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.Chat.Cells
{
    public partial class ChatCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ChatCell");
        public static readonly UINib Nib = UINib.FromName("ChatCell", NSBundle.MainBundle);
        protected ChatCell(IntPtr handle) : base(handle){}

        public void Configure()
        {

        }
    }
}
