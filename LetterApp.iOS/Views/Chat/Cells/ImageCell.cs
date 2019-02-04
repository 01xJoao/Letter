using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
        private EventHandler<long> _messageEvent;
        private ChatMessagesModel _chatMessagesModel;

        public static readonly NSString Key = new NSString("ImageCell");
        public static readonly UINib Nib = UINib.FromName("ImageCell", NSBundle.MainBundle);
        protected ImageCell(IntPtr handle) : base(handle){}

        public void Configure(ChatMessagesModel chatMessagesModel, EventHandler<long> messageEvent)
        {
            _chatMessagesModel = chatMessagesModel;

            _imageView.Image?.Dispose();
            _imageView.Image = null;

            _messageEvent = messageEvent;

            LoadImage();


            if (chatMessagesModel.FailedToSend)
            {
                _imageView.Layer.BorderColor = Colors.Red.CGColor;     
                _imageView.Layer.BorderWidth = 5f;
                _imageView.Layer.MasksToBounds = false;
            }

            _button.TouchUpInside -= OnButton_TouchUpInside;
            _button.TouchUpInside += OnButton_TouchUpInside;
        }

        private async Task LoadImage()
        {
            try
            {
                if (!_chatMessagesModel.IsFakeMessage)
                {
                    ImageService.Instance.LoadUrl(_chatMessagesModel.MessageData).Retry(3, 200).DownSample((int)_imageView.Frame.Width, (int)_imageView.Frame.Height, false)
                                .LoadingPlaceholder("image_loading.gif", ImageSource.CompiledResource).DownSampleMode(InterpolationMode.Low).Transform(new RoundedTransformation(15)).Into(_imageView);
                }
                else
                {
                    ImageService.Instance.LoadFile(_chatMessagesModel.MessageData).Retry(3, 200).DownSample((int)_imageView.Frame.Width, (int)_imageView.Frame.Height, false)
                                .LoadingPlaceholder("image_loading.gif", ImageSource.CompiledResource).DownSampleMode(InterpolationMode.Low).Transform(new RoundedTransformation(15)).Into(_imageView);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnButton_TouchUpInside(object sender, EventArgs e)
        {
            _messageEvent?.Invoke(this, _chatMessagesModel.MessageId);
        }
    }
}
