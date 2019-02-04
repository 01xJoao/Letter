using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.iOS.Helpers;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;
using static LetterApp.Core.ViewModels.TabBarViewModels.ContactListViewModel;

namespace LetterApp.iOS.Views.TabBar.ContactListViewController.Cells
{
    public partial class ContactsCell : UITableViewCell
    {
        private ContactsType _showContactType;
        private int _userId;
        private string _picture;
        private EventHandler<Tuple<ContactEventType, int>> _contactEventHandler;
        public static readonly NSString Key = new NSString("ContactsCell");
        public static readonly UINib Nib = UINib.FromName("ContactsCell", NSBundle.MainBundle);
        protected ContactsCell(IntPtr handle) : base(handle) {}

        public void Configure(GetUsersInDivisionModel user, EventHandler<Tuple<ContactEventType, int>> contactEventHandler, ContactsType showContactType)
        {
            _imageView.Image?.Dispose();
            _chatImage.Image?.Dispose();
            _callImage.Image?.Dispose();

            _showContactType = showContactType;
            _userId = user.UserId;
            _contactEventHandler = contactEventHandler;

            UILabelExtensions.SetupLabelAppearance(_nameLabel, $"{user.FirstName} {user.LastName}", Colors.ProfileGray, 14f, UIFontWeight.Semibold);
            UILabelExtensions.SetupLabelAppearance(_roleLabel, user.Position, Colors.Black, 13f);

            if (!string.IsNullOrEmpty(user?.Picture))
            {
                _picture = string.Copy(user.Picture); 

                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, _picture);
                }).ErrorPlaceholder("profile_noimage", ImageSource.CompiledResource).Retry(3, 200).Finish(CleanString).Transform(new CircleTransformation()).Into(_imageView);
            }
            else
            {
                _imageView.Image = UIImage.FromBundle("profile_noimage");
                CustomUIExtensions.RoundView(_imageView);
            }

            _callImage.Image = UIImage.FromBundle("tabbar_call_selected");

            _callButton.TouchUpInside -= OnCallButton_TouchUpInside;
            _callButton.TouchUpInside += OnCallButton_TouchUpInside;


            _chatImage.Image = UIImage.FromBundle("user_chat");

            _chatButton.TouchUpInside -= OnChatButton_TouchUpInside;
            _chatButton.TouchUpInside += OnChatButton_TouchUpInside;

            switch (showContactType)
            {
                case ContactsType.All:
                    _chatButton.Hidden = false;
                    _chatImage.Hidden = false;
                    _callImage.Hidden = false;
                    _callButton.Hidden = false;
                    break;
                case ContactsType.Call:
                    _chatButton.Hidden = true;
                    _chatImage.Hidden = true;
                    break;
                case ContactsType.Chat:
                    _chatButton.Hidden = true;
                    _chatImage.Hidden = true;
                    _callImage.Image = UIImage.FromBundle("user_chat");
                    break;
                default:
                    break;
            }
        }

        private void CleanString(IScheduledWork obj)
        {
            _picture = null;
        }

        private void OnChatButton_TouchUpInside(object sender, EventArgs e)
        {
            _contactEventHandler?.Invoke(this, new Tuple<ContactEventType, int>(ContactEventType.Chat, _userId));
        }

        private void OnCallButton_TouchUpInside(object sender, EventArgs e)
        {
            if(_showContactType != ContactsType.Chat)
                _contactEventHandler?.Invoke(this, new Tuple<ContactEventType, int>(ContactEventType.Call, _userId));
            else
                _contactEventHandler?.Invoke(this, new Tuple<ContactEventType, int>(ContactEventType.Chat, _userId));
        }
    }
}
