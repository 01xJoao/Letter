using System;
using Foundation;
using LetterApp.Core.Models;
using UIKit;

namespace LetterApp.iOS.Views.Chat.Cells
{
    public partial class FileWithUserCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("FileWithUserCell");
        public static readonly UINib Nib = UINib.FromName("FileWithUserCell", NSBundle.MainBundle);
        protected FileWithUserCell(IntPtr handle) : base(handle){}

        public void Configure(ChatMessagesModel chatMessagesModel, EventHandler<long> messageEvent, MemberPresence memberPresence)
        {
        }
    }
}
