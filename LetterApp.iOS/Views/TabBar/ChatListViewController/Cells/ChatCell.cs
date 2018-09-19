using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.ChatListViewController.Cells
{
    public partial class ChatCell : UITableViewCell
    {
        private string _picture;
        private ChatListUserCellModel _chatUser;

        public static readonly NSString Key = new NSString("ChatCell");
        public static readonly UINib Nib = UINib.FromName("ChatCell", NSBundle.MainBundle);

        protected ChatCell(IntPtr handle) : base(handle){}

        public void Configure(ChatListUserCellModel chatUser)
        {
            _chatUser = chatUser;

            UILabelExtensions.SetupLabelAppearance(_dateLabel, chatUser.LastMessageDate, chatUser.ShouldAlertNewMessage ? Colors.Black : Colors.ProfileGrayDarker, 11f, chatUser.ShouldAlertNewMessage ? UIFontWeight.Semibold : UIFontWeight.Regular);
            UILabelExtensions.SetupLabelAppearance(_messageLabel, chatUser.LastMessage, Colors.MessageTextColor, 13f, chatUser.ShouldAlertNewMessage ? UIFontWeight.Medium : UIFontWeight.Regular);

            var nameAttr = new UIStringAttributes
            {
                ForegroundColor = Colors.BlackChatName,
                Font = UIFont.SystemFontOfSize(15, chatUser.ShouldAlertNewMessage ? UIFontWeight.Semibold : UIFontWeight.Regular)
            };

            var RoleAttr = new UIStringAttributes
            {
                ForegroundColor = Colors.BlackChatName,
                Font = UIFont.SystemFontOfSize(13, chatUser.ShouldAlertNewMessage ? UIFontWeight.Semibold : UIFontWeight.Regular)
            };

            var letterCount = chatUser.MemberName.IndexOf("-");

            var customString = new NSMutableAttributedString(chatUser.MemberName);
            customString.SetAttributes(nameAttr.Dictionary, new NSRange(0, letterCount));
            customString.SetAttributes(RoleAttr.Dictionary, new NSRange(letterCount + 1, chatUser.MemberName.Length - (letterCount + 1)));

            _memberNameLabel.AttributedText = customString;

            _separatorLineView.BackgroundColor = Colors.ChatDivider;

            _profileImage.Image?.Dispose();

            if (!string.IsNullOrEmpty(chatUser.MemberPhoto))
            {
                _picture = string.Copy(chatUser.MemberPhoto);

                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, _picture);
                }).ErrorPlaceholder("profile_noimage", ImageSource.CompiledResource).Retry(3, 200).Finish(CleanString).Transform(new CircleTransformation()).Into(_profileImage);
            }
            else
            {
                _profileImage.Image = UIImage.FromBundle("profile_noimage");
                CustomUIExtensions.RoundView(_profileImage);
            }

            switch (chatUser.MemberPresence)
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

            _silentImage.Image?.Dispose();
            _silentImage.Hidden = !chatUser.IsMemberMuted;
            _silentImage.Image = UIImage.FromBundle("silent").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            _silentImage.TintColor = Colors.ProfileGrayDarker;

            //_silentImageWidthConstraint.Constant = chatUser.IsMemberMuted ? _silentImageWidthConstraint.Constant : 0;
            //_unreadCountWidthConstraint.Constant = chatUser.UnreadMessagesCount > 0 ? _unreadCountWidthConstraint.Constant : 0;
            //_unreadCountHeightConstraint.Constant = chatUser.UnreadMessagesCount > 0 ? _unreadCountWidthConstraint.Constant : 0;

            UILabelExtensions.SetupLabelAppearance(_unreadCountLabel, chatUser.UnreadMessagesCount.ToString(), Colors.White, 10f, UIFontWeight.Medium);
            _unreadCountLabel.Hidden = _unreadCountView.Hidden = chatUser.UnreadMessagesCount == 0;
            _unreadCountView.BackgroundColor = Colors.UnreadCount;
            CustomUIExtensions.RoundView(_unreadCountView);

            _messageTrailConstraint.Constant = chatUser.UnreadMessagesCount > 0 || chatUser.IsMemberMuted ? _messageTrailConstraint.Constant : 0;

            _chatButton.TouchUpInside -= OnChatButton_TouchUpInside;
            _chatButton.TouchUpInside += OnChatButton_TouchUpInside;

            _profileImageButton.TouchUpInside -= OnProfileImageButton_TouchUpInside;
            _profileImageButton.TouchUpInside += OnProfileImageButton_TouchUpInside;
        }

        private void OnProfileImageButton_TouchUpInside(object sender, EventArgs e)
        {
            _chatUser.OpenMemberProfile?.Invoke(this, _chatUser.MemberId);
        }

        private void OnChatButton_TouchUpInside(object sender, EventArgs e)
        {
            _chatUser.OpenChat?.Invoke(this, _chatUser.MemberId);
        }

        private void CleanString(IScheduledWork obj)
        {
            _picture = null;
        }
    }
}
