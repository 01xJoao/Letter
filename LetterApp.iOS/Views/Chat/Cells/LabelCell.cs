using System;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.Chat.Cells
{
    public partial class LabelCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("LabelCell");
        public static readonly UINib Nib = UINib.FromName("LabelCell", NSBundle.MainBundle);
        protected LabelCell(IntPtr handle) : base(handle){}

        public void Configure(ChatMessagesModel chatMessagesModel, EventHandler<int> messageEvent)
        {
            UILabelExtensions.SetupLabelAppearance(_textLabel, chatMessagesModel.MessageData, Colors.Black, 14f);
        }
    }
}
