using System;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.Chat.Cells
{
    public partial class LabelCell : UITableViewCell
    {
        private long _messageId;
        private EventHandler<long> _messageEvent;
        public static readonly NSString Key = new NSString("LabelCell");
        public static readonly UINib Nib = UINib.FromName("LabelCell", NSBundle.MainBundle);
        protected LabelCell(IntPtr handle) : base(handle){}

        public void Configure(ChatMessagesModel chatMessagesModel, EventHandler<long> messageEvent)
        {
            _messageId = chatMessagesModel.MessageId;
            _messageEvent = messageEvent;

            var messageAttributes = new UIStringAttributes
            {
                Font = chatMessagesModel.FailedToSend ? UIFont.BoldSystemFontOfSize(15f) : UIFont.SystemFontOfSize(14f),
                ForegroundColor = chatMessagesModel.FailedToSend ? Colors.Red : Colors.Black,
                ParagraphStyle = new NSMutableParagraphStyle { LineSpacing = 2f }
            };

            var attributedText = new NSMutableAttributedString(chatMessagesModel.MessageData);
            attributedText.AddAttributes(messageAttributes, new NSRange(0, chatMessagesModel.MessageData.Length));
            _textLabel.AttributedText = attributedText;

            _button.TouchUpInside -= _button_TouchUpInside;
            _button.TouchUpInside += _button_TouchUpInside;
        }

        private void _button_TouchUpInside(object sender, EventArgs e)
        {
            _messageEvent?.Invoke(this, _messageId);
        }
    }
}
