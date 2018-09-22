using System;

using Foundation;
using LetterApp.Core.Models;
using UIKit;

namespace LetterApp.iOS.Views.Chat.Cells
{
    public partial class MessageCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MessageCell");
        public static readonly UINib Nib = UINib.FromName("MessageCell", NSBundle.MainBundle);
        protected MessageCell(IntPtr handle) : base(handle){}

        public void Configure(ChatMessagesModel chatMessagesModel, EventHandler<int> messageEvent)
        {
            throw new NotImplementedException();
        }
    }
}
