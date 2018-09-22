using System;
using Foundation;
using LetterApp.Core.Models;
using UIKit;

namespace LetterApp.iOS.Views.Chat.Cells
{
    public partial class ImageCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ImageCell");
        public static readonly UINib Nib = UINib.FromName("ImageCell", NSBundle.MainBundle);
        protected ImageCell(IntPtr handle) : base(handle){}

        public void Configure(ChatMessagesModel chatMessagesModel, EventHandler<int> messageEvent)
        {
            throw new NotImplementedException();
        }
    }
}
