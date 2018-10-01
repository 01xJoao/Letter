using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.Chat.Cells
{
    public partial class MessageCell : UITableViewCell
    {
        private string _picture;
        private long _messageId;
        private EventHandler<long> _messageEvent;
        public static readonly NSString Key = new NSString("MessageCell");
        public static readonly UINib Nib = UINib.FromName("MessageCell", NSBundle.MainBundle);
        protected MessageCell(IntPtr handle) : base(handle){}

        public void Configure(ChatMessagesModel message, EventHandler<long> messageEvent, MemberPresence memberPresence)
        {
            _messageId = message.MessageId;
            _messageEvent = messageEvent;

            var nameAttr = new UIStringAttributes
            {
                ForegroundColor = Colors.Black,
                Font = UIFont.SystemFontOfSize(15f, UIFontWeight.Semibold)
            };

            var timeAttr = new UIStringAttributes
            {
                ForegroundColor = Colors.ProfileGrayDarker,
                Font = UIFont.SystemFontOfSize(11f, UIFontWeight.Regular)
            };

            var messageAttributes = new UIStringAttributes
            {
                Font = message.FailedToSend ? UIFont.BoldSystemFontOfSize(15f) : UIFont.SystemFontOfSize(14f),
                ForegroundColor = message.FailedToSend ? Colors.Red : Colors.Black,
                ParagraphStyle = new NSMutableParagraphStyle { LineSpacing = 2f },
            };

            var customString = new NSMutableAttributedString(message.Name + message.MessageDate);
            customString.SetAttributes(nameAttr.Dictionary, new NSRange(0, message.Name.Length));
            customString.SetAttributes(timeAttr.Dictionary, new NSRange(message.Name.Length + 1, message.MessageDate.Length - 1));
            _nameLabel.AttributedText = customString;

            var attributedText = new NSMutableAttributedString(message.MessageData);
            attributedText.AddAttributes(messageAttributes, new NSRange(0, message.MessageData.Length));
            _messageLabel.AttributedText = attributedText;

            if (!string.IsNullOrEmpty(message.Picture))
            {
                _picture = string.Copy(message.Picture);

                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, _picture);
                }).ErrorPlaceholder("profile_noimage", ImageSource.CompiledResource).Retry(3, 200).Finish(CleanString).Transform(new CircleTransformation()).Into(_imageView);
            }
            else
            {
                _imageView.Image = UIImage.FromBundle("profile_noimage");
                CustomUIExtensions.RoundView(_imageView);
            }

            switch (memberPresence)
            {
                case MemberPresence.Online:
                    _presenceView.BackgroundColor = Colors.UserOnline;
                    _presenceView.Hidden = false;
                    break;
                case MemberPresence.Recent:
                    _presenceView.BackgroundColor = Colors.UserRecent;
                    _presenceView.Hidden = false;
                    break;
                case MemberPresence.Offline:
                    _presenceView.Hidden = true;
                    break;
            }

            _presenceView.Hidden = !message.ShowPresense;

            CustomUIExtensions.RoundView(_presenceView);
            _presenceView.Layer.BorderWidth = 1f;
            _presenceView.Layer.BorderColor = Colors.White.CGColor;

            _dividerLeftView.BackgroundColor = Colors.AlertDividerColor;
            _dividerRightView.BackgroundColor = Colors.AlertDividerColor;
            UILabelExtensions.SetupLabelAppearance(_dateLabel, message.HeaderDate, Colors.ChatDate, 12f, UIFontWeight.Medium);

            _dateView.Hidden = !message.ShowHeaderDate;
            _dateViewHeightConstraint.Constant = message.ShowHeaderDate ? LocalConstants.Chat_HeaderDateBig : LocalConstants.Chat_HeaderDateSmall;
        }

        private void CleanString(IScheduledWork obj)
        {
            _picture = null;
        }
    }
}
