using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;
using SendBird;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class ChatListViewModel : XViewModel
    {
        private readonly IMessengerService _messagerService;

        private int _thisUserId => AppSettings.UserId;
        private string _thisUserFinalId => AppSettings.UserAndOrganizationIds;

        public bool UpdateTableView { get; set; }
        public bool NoChats { get; set; }
        public string[] Actions;

        private DateTime _updateFrequence = DateTime.Now;
        private List<GetUsersInDivisionModel> _users;
        private List<ChatListUserModel> _chatUserModel;

        public List<ChatListUserCellModel> _chatList;
        public List<ChatListUserCellModel> ChatList
        {
            get => _chatList;
            set => SetProperty(ref _chatList, value);
        }

        private XPCommand _openContactsCommand;
        public XPCommand OpenContactsCommand => _openContactsCommand ?? (_openContactsCommand = new XPCommand(async () => await OpenContacts()));

        private XPCommand<string> _searchChatCommand;
        public XPCommand<string> SearchChatCommand => _searchChatCommand ?? (_searchChatCommand = new XPCommand<string>(SearchChat));

        private XPCommand _closeSearchCommand;
        public XPCommand CloseSearchCommand => _closeSearchCommand ?? (_closeSearchCommand = new XPCommand(CloseSearch));

        private XPCommand<Tuple<ChatEventType, int>> _rowActionCommand;
        public XPCommand<Tuple<ChatEventType, int>> RowActionCommand => _rowActionCommand ?? (_rowActionCommand = new XPCommand<Tuple<ChatEventType, int>>(RowAction));

        public ChatListViewModel(IMessengerService messagerService)
        {
            _messagerService = messagerService;
            Actions = new string[] { DeleteAction, MuteAction, UnMuteAction };
            CheckForMessagesHandler();
        }

        public override async Task Appearing()
        {
            Debug.WriteLine("Appearing");


            if (_users == null || _users.Count == 0)
            {
                _users = Realm.All<GetUsersInDivisionModel>().ToList();
                UpdateChatList();
            }

            if (SendBirdClient.GetConnectionState() != SendBirdClient.ConnectionState.OPEN)
            {
                try
                {
                    var result = await _messagerService.ConnectMessenger();

                    if(result) 
                        UpdateMessengerService();

                    CreateChannel();
                }
                catch (Exception ex)
                {
                    Ui.Handle(ex as dynamic);
                }
            }
            else
                UpdateMessengerService();
                
        }

        private async Task CheckForMessagesHandler()
        {
            try
            {
                var message = await _messagerService.InitializeHandlers();
                var channel = await _messagerService.GetUsersInChannel(message.ChannelUrl);

                if (channel?.Members.FirstOrDefault()?.UserId == null)
                    return;
                   
                var user = _chatUserModel.Find(x => x.MemberId == StringUtils.GetUserId(channel.Members.FirstOrDefault(y => y.UserId != _thisUserFinalId).UserId));

                var msg = message as UserMessage;

                Realm.Write(() =>
                {
                    user.LastMessage = msg.Message;
                    user.LastMessageDate = DateUtils.TimePassed(new DateTime(msg.CreatedAt));
                    user.LastMessageDateTimeTicks = msg.CreatedAt;
                    user.ShouldAlert = true;
                });

                var member = _chatList.Find(x => x.MemberId == user.MemberId);
                member.LastMessage = user.LastMessage;
                member.LastMessageDate = user.LastMessageDate;
                member.LastMessageDateTime = new DateTime(user.LastMessageDateTimeTicks);
                member.ShouldAlert = user.ShouldAlert;

                Debug.WriteLine("New Message! " + msg.Message);

                RaisePropertyChanged(nameof(UpdateTableView));

                CheckForMessagesHandler();
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private void UpdateChatList()
        {
            _chatList = new List<ChatListUserCellModel>();
            _chatUserModel = Realm.All<ChatListUserModel>().ToList();

            foreach (var chat in _chatUserModel)
            {
                var cht = new ChatListUserCellModel(chat.MemberId, chat.MemberName, chat.MemberPhoto, chat.LastMessage,
                                                    chat.LastMessageDate, chat.ShouldAlert, chat.IsMemeberMuted,
                                                    OpenUserProfileEvent, OpenUserChat, new DateTime(chat.LastMessageDateTimeTicks));

                TimeSpan timeDifference = DateTime.Now.Subtract(new DateTime(chat.MemberPresenceConnectionDate));

                cht.MemberPresence = chat.MemberPresence == 0 && timeDifference.TotalMinutes < 5
                    ? MemberPresence.Online
                    : timeDifference.TotalMinutes < 60 ? MemberPresence.Recent : MemberPresence.Offline;

                _chatList.Add(cht);
            }

            if (_chatList.Count > 0)
            {
                Debug.WriteLine("going to updateve from viewmode the TableView with chats count: " + _chatList.Count);

                RaisePropertyChanged(nameof(UpdateTableView));
            }
        }

        private async Task UpdateMessengerService()
        {
            if (_users.Count == 0)
                return;

            if (DateTime.Now >= _updateFrequence)
            {
                var newChatList = new List<ChatListUserModel>();

                try
                {
                    var allChannels = await _messagerService.GetAllChannels();

                    foreach(var channel in allChannels)
                    {
                        var users = await _messagerService.CheckUsersInGroupPresence(channel);
                       
                        foreach (var user in users)
                        {
                            var userId = StringUtils.GetUserId(user.UserId);
                            var userInDB = _users.Find(x => x.UserId == userId);
                            var userInModel = _chatUserModel?.Find(x => x?.MemberId == userId);

                            if (userInDB == null || userId == _thisUserId)
                                continue;

                            var lastMessage = channel.LastMessage as UserMessage;

                            DateTime lastMessageDate = !string.IsNullOrEmpty(lastMessage.Data) ? new DateTime(Int32.Parse(lastMessage.Data)) : DateTime.Now;

                            var usr = new ChatListUserModel
                            {
                                MemberId = userInDB.UserId,
                                MemberName = $"{userInDB.FirstName} {userInDB.LastName} - {userInDB.Position}",
                                MemberPhoto = userInDB.Picture,
                                IsMemeberMuted = userInModel != null && userInModel.IsMemeberMuted,
                                MemberPresence = user.ConnectionStatus == User.UserConnectionStatus.ONLINE ? 0 : 1,
                                MemberPresenceConnectionDate = user.LastSeenAt,
                                ShouldAlert = userInModel == null || channel.LastMessage.CreatedAt > userInModel.LastTimeChatWasOpen,
                                LastMessage = lastMessage.Message,
                                LastMessageDate = DateUtils.TimePassed(lastMessageDate),
                                LastMessageDateTimeTicks = lastMessageDate.Ticks
                            };

                            newChatList.Add(usr);
                        }
                    }

                    Realm.Write(() => {
                        foreach(var chat in newChatList)
                        {
                            Realm.Add(chat, true);
                        }
                    });

                    Debug.WriteLine("UpdateMessengerService with chats count: " + newChatList.Count);

                    UpdateChatList();
                }
                catch (Exception ex)
                {
                    Ui.Handle(ex as dynamic);
                }
                finally
                {
                    _updateFrequence = DateTime.Now.AddMinutes(2);
                }
            }
        }

        private async Task CreateChannel()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            try
            {
                var channel = await _messagerService.CreateChannel(new List<string> { "85-33" });

                if (channel != null)
                    await _messagerService.SendMessage(channel, "Hello World! - Test", DateTime.Now.Ticks.ToString());

            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private async Task OpenContacts()
        {
        }

        private void RowAction(Tuple<ChatEventType, int> action)
        {
        }

        private void SearchChat(string search)
        {
        }

        private void CloseSearch()
        {
        }

        private void OpenUserChat(object sender, int userId) => NavigateToUserChat(userId);
        private void OpenUserProfileEvent(object sender, int userId) => NavigateToUserProfile(userId);
        private async Task NavigateToUserChat(int userId) => await NavigationService.NavigateAsync<ChatViewModel, int>(userId);
        private async Task NavigateToUserProfile(int userId) => await NavigationService.NavigateAsync<MemberViewModel, int>(userId);

        #region Resources

        public string Title => L10N.Localize("ChatList_Title");
        public string NoRecentChat => L10N.Localize("ChatList_NoChats");

        public string DeleteAction => L10N.Localize("ChatList_Delete");
        public string MuteAction => L10N.Localize("ChatList_Mute");
        public string UnMuteAction => L10N.Localize("ChatList_UnMute");

        public enum ChatEventType
        {
            Mute,
            Delete
        }

        #endregion
    }
}
