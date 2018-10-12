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
    public partial class ImageCell : UITableViewCell
    {
        private long _messageId;
        EventHandler<long> _messageEvent;

        public static readonly NSString Key = new NSString("ImageCell");
        public static readonly UINib Nib = UINib.FromName("ImageCell", NSBundle.MainBundle);
        protected ImageCell(IntPtr handle) : base(handle){}

        public void Configure(ChatMessagesModel chatMessagesModel, EventHandler<long> messageEvent)
        {
            _imageView.Image?.Dispose();
            _imageView.Image = null;

            _messageEvent = messageEvent;
            _messageId = chatMessagesModel.MessageId;

            ImageService.Instance.LoadUrl(chatMessagesModel.MessageData).Retry(3, 200).DownSample((int)_imageView.Frame.Width, (int)LocalConstants.Chat_Images, allowUpscale: true)
                        .Transform(new RoundedTransformation(15)).Into(_imageView);

            if (chatMessagesModel.FailedToSend)
            {
                _imageView.Layer.BorderColor = Colors.Red.CGColor;
                _imageView.Layer.BorderWidth = 2f;
                _imageView.Layer.MasksToBounds = false;
            }

            _button.TouchUpInside -= OnButton_TouchUpInside;
            _button.TouchUpInside += OnButton_TouchUpInside;
        }

        private void OnButton_TouchUpInside(object sender, EventArgs e)
        {
            _messageEvent?.Invoke(this, _messageId);
        }
    }
}
