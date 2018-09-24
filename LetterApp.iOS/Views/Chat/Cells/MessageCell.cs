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
        public string _picture;

        public static readonly NSString Key = new NSString("MessageCell");
        public static readonly UINib Nib = UINib.FromName("MessageCell", NSBundle.MainBundle);
        protected MessageCell(IntPtr handle) : base(handle){}

        public void Configure(ChatMessagesModel message, EventHandler<int> messageEvent, MemberPresence memberPresence)
        {
            UILabelExtensions.SetupLabelAppearance(_nameLabel,$"{message.Name} - {message.MessageDate}" , Colors.ProfileGrayDarker, 14f, UIFontWeight.Semibold);
            UILabelExtensions.SetupLabelAppearance(_messageLabel, message.MessageData, Colors.Black, 13f);

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
                default:
                    _presenceView.Hidden = true;
                    break;
            }

            CustomUIExtensions.RoundView(_presenceView);
            _presenceView.Layer.BorderWidth = 2f;
            _presenceView.Layer.BorderColor = Colors.White.CGColor;
        }

        private void CleanString(IScheduledWork obj)
        {
            _picture = null;
        }
    }
}
