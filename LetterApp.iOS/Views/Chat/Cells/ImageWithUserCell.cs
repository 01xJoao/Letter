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
    public partial class ImageWithUserCell : UITableViewCell
    {
        private string _picture;
        private long _messageId;
        private EventHandler<long> _messageEvent;

        public static readonly NSString Key = new NSString("ImageWithUserCell");
        public static readonly UINib Nib = UINib.FromName("ImageWithUserCell", NSBundle.MainBundle);
        protected ImageWithUserCell(IntPtr handle) : base(handle){}

        public void Configure(ChatMessagesModel chatMessagesModel, EventHandler<long> messageEvent, MemberPresence memberPresence)
        {
            _imageView.Image?.Dispose();
            _imageView.Image = null;

            _pictureImage.Image?.Dispose();
            _pictureImage.Image = null;

            _messageId = chatMessagesModel.MessageId;
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

            var customString = new NSMutableAttributedString(chatMessagesModel.Name + chatMessagesModel.MessageDate);
            customString.SetAttributes(nameAttr.Dictionary, new NSRange(0, chatMessagesModel.Name.Length));
            customString.SetAttributes(timeAttr.Dictionary, new NSRange(chatMessagesModel.Name.Length + 1, chatMessagesModel.MessageDate.Length - 1));
            _nameLabel.AttributedText = customString;

            if (!string.IsNullOrEmpty(chatMessagesModel.Picture))
            {
                _picture = string.Copy(chatMessagesModel.Picture);

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

            _presenceView.Hidden = !chatMessagesModel.ShowPresense;

            CustomUIExtensions.RoundView(_presenceView);
            _presenceView.Layer.BorderWidth = 1f;
            _presenceView.Layer.BorderColor = Colors.White.CGColor;

            _dividerLeftView.BackgroundColor = Colors.AlertDividerColor;
            _dividerRightView.BackgroundColor = Colors.AlertDividerColor;
            UILabelExtensions.SetupLabelAppearance(_dateLabel, chatMessagesModel.HeaderDate, Colors.ChatDate, 12f, UIFontWeight.Medium);

            _dateView.Hidden = !chatMessagesModel.ShowHeaderDate;
            _dateViewHeightConstraint.Constant = chatMessagesModel.ShowHeaderDate ? LocalConstants.Chat_HeaderDateBig : LocalConstants.Chat_HeaderDateSmall;

            ImageService.Instance.LoadUrl(chatMessagesModel.MessageData).
                       ErrorPlaceholder("warning_image", ImageSource.CompiledResource).Retry(3, 200)
                       .DownSample((int)_pictureImage.Frame.Width, allowUpscale: true).Transform(new RoundedTransformation(15)).Into(_pictureImage);

            if (chatMessagesModel.FailedToSend)
            {
                _pictureImage.Layer.BorderColor = Colors.Red.CGColor;
                _pictureImage.Layer.BorderWidth = 2f;
                _pictureImage.Layer.MasksToBounds = false;
            }

            _pictureButton.TouchUpInside -= OnPictureButton_TouchUpInside;
            _pictureButton.TouchUpInside += OnPictureButton_TouchUpInside;
        }

        private void OnPictureButton_TouchUpInside(object sender, EventArgs e)
        {
            _messageEvent?.Invoke(this, _messageId);
        }

        private void CleanString(IScheduledWork obj)
        {
            _picture = null;
        }
    }
}
