using System;
using Foundation;
using LetterApp.Core.Models;
using UIKit;

namespace LetterApp.iOS.Views.Chat.Cells
{
    public partial class FileCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("FileCell");
        public static readonly UINib Nib = UINib.FromName("FileCell", NSBundle.MainBundle);
        protected FileCell(IntPtr handle) : base(handle){}

        public void Configure(ChatMessagesModel chatMessagesModel, EventHandler<long> messageEvent)
        {
        }
    }
}
