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
using Xamarin.Essentials;
using static LetterApp.Core.ViewModels.TabBarViewModels.ContactListViewModel;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class ChatListViewModel : XViewModel
    {
        private readonly IMessengerService _messagerService;
        private readonly IDialogService _dialogService;
        private readonly IContactsService _contactsService;

        private int _thisUserId => AppSettings.UserId;
        private string _thisUserFinalId => AppSettings.UserAndOrganizationIds;
        private bool _isSearching;
        private bool _showNotifications = true;

        public bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool UpdateTableView { get; set; }
        public bool NoChats { get; set; }
        public string[] Actions;

        private bool _badgeForChat;
        public bool BadgeForChat
        {
            get => _badgeForChat;
            set => SetProperty(ref _badgeForChat, value);
        }

        private DateTime _updateFrequence;
        private List<GetUsersInDivisionModel> _users;
        private List<ChatListUserModel> _chatUserModel;
        private List<ChatListUserCellModel> _chatListSearch;

        private List<ChatListUserCellModel> _chatList;
        public List<ChatListUserCellModel> ChatList
        {
            get
            {
                return _isSearching
                    ? _chatListSearch?.OrderByDescending(x => x.LastMessageDateTime).ToList()
                    : _chatList?.OrderByDescending(x => x.LastMessageDateTime).ToList();
            }
            set
            {
                SetProperty(ref _chatList, value);
                _chatListSearch = value;
            }
        }

        private XPCommand _openContactsCommand;
        public XPCommand OpenContactsCommand => _openContactsCommand ?? (_openContactsCommand = new XPCommand(async () => await OpenContacts()));

        private XPCommand<string> _searchChatCommand;
        public XPCommand<string> SearchChatCommand => _searchChatCommand ?? (_searchChatCommand = new XPCommand<string>(SearchChat));

        private XPCommand _closeSearchCommand;
        public XPCommand CloseSearchCommand => _closeSearchCommand ?? (_closeSearchCommand = new XPCommand(CloseSearch));

        private XPCommand<Tuple<ChatEventType, int>> _rowActionCommand;
        public XPCommand<Tuple<ChatEventType, int>> RowActionCommand => _rowActionCommand ?? (_rowActionCommand = new XPCommand<Tuple<ChatEventType, int>>(RowAction));

        private XPCommand<bool> _chowNotificationsCommand;
        public XPCommand<bool> ShowNotificationsCommand => _chowNotificationsCommand ?? (_chowNotificationsCommand = new XPCommand<bool>((value) => SetNotifications(value)));

        public ChatListViewModel(IContactsService contactsService, IMessengerService messagerService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _contactsService = contactsService;
            _messagerService = messagerService;
            Actions = new string[] { ArchiveAction, MuteAction, UnMuteAction };
            ChatHandler();
            HandleConnection();
        }

        public override async Task Appearing()
        {
            IsLoading = true;

            _users = Realm.All<GetUsersInDivisionModel>().ToList();
            UpdateChatList();

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsLoading = false;
                return;
            }

            if (SendBirdClient.GetConnectionState() != SendBirdClient.ConnectionState.OPEN)
            {
                try
                {
                    if (SendBirdClient.Reconnect())
                        await UpdateMessengerService();
                }
                catch (Exception ex)
                {
                    IsLoading = false;
                    Ui.Handle(ex as dynamic);
                }
            }
            else
                await UpdateMessengerService();
        }

        private async Task GetUsers()
        {
            try
            {
                var result = await _contactsService.GetUsersFromAllDivisions();

                if (result == null && result.Count == 0)
                    return;

                Realm.Write(() => 
                {
                    foreach (var res in result) {
                        res.UniqueKey = $"{res.UserId}+{res.DivisionId}";
                        Realm.Add(res, true);
                    }
                });
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                _users = Realm.All<GetUsersInDivisionModel>().ToList();
            }
        }

        private async Task CheckForMessagesHandler(BaseChannel channel, BaseMessage message)
        {
            try
            {
                int msgUserId = 0;

                if (message is UserMessage umsg)
                    msgUserId = StringUtils.GetUserId(umsg.Sender.UserId);
                else if (message is FileMessage fmsg)
                    msgUserId = StringUtils.GetUserId(fmsg.Sender.UserId);

                if (msgUserId == 0)
                    return;

                DateTime lastMessageDate = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(message.CreatedAt.ToString())).ToLocalTime();

                var userChatModel = _chatUserModel?.Find(x => x.MemberId == msgUserId);

                if (userChatModel == null)
                {
                    var res = _users?.Find(x => x.UserId == msgUserId);

                    if (res == null)
                    {
                        await GetUsers();
                        var result = _users?.Find(x => x.UserId == msgUserId);

                        if (result == null)
                            return;

                        var user = new ChatListUserModel
                        {
                            MemberId = result.UserId,
                            MemberName = $"{result.FirstName} {result.LastName} - {result.Position}",
                            MemberPhoto = result.Picture,
                            IsMemberMuted = false,
                        };

                        Realm.Write(() => Realm.Add(user, true));

                        _chatUserModel.Add(user);
                    }
                    else
                    {
                        var user = new ChatListUserModel
                        {
                            MemberId = res.UserId,
                            MemberName = $"{res.FirstName} {res.LastName} - {res.Position}",
                            MemberPhoto = res.Picture,
                            IsMemberMuted = false,
                        };

                        Realm.Write(() => Realm.Add(user, true));
                        _chatUserModel.Add(user);
                    }
                }

                userChatModel = _chatUserModel?.Find(x => x.MemberId == msgUserId);

                if (userChatModel == null)
                    return;

                var newmsg = new MessagesModel
                {
                    MessageId = message.MessageId,
                    MessageDateTicks = lastMessageDate.Ticks
                };

                if (message is UserMessage u)
                {
                    newmsg.MessageType = 0;
                    newmsg.MessageData = u.Message;
                    newmsg.MessageSenderId = u.Sender.UserId;
                }
                else if (message is FileMessage m)
                {
                    newmsg.MessageType = 1;
                    newmsg.MessageData = m.Data;
                    newmsg.MessageSenderId = m.Sender.UserId;
                }

                Realm.Write(() =>
                {
                    userChatModel.MemberPresence = 0;
                    userChatModel.MemberPresenceConnectionDate = lastMessageDate.Ticks;

                    if (message is UserMessage um)
                        userChatModel.LastMessage = um.Message;
                    else if (message is FileMessage fm)
                        userChatModel.LastMessage = SentImage;
                        
                    userChatModel.LastMessageDateTimeTicks = lastMessageDate.Ticks;
                    userChatModel.IsArchived = false;
                    userChatModel.IsNewMessage = true;
                    userChatModel.UnreadMessagesCount += 1;
                    userChatModel.MessagesList.Add(newmsg);
                });

                var member = _chatList.Find(x => x.MemberId == userChatModel.MemberId);

                if (member == null)
                {
                    member = new ChatListUserCellModel
                    {
                        MemberId = userChatModel.MemberId,
                        MemberName = userChatModel.MemberName,
                        MemberPhoto = userChatModel.MemberPhoto,
                        IsMemberMuted = userChatModel.IsMemberMuted,
                        OpenChat = OpenUserChatEvent,
                        OpenMemberProfile = OpenUserProfileEvent
                    };

                    _chatList.Add(member);
                }

                member.UnreadMessagesCount = userChatModel.UnreadMessagesCount;
                member.IsNewMessage = userChatModel.IsNewMessage;
                member.LastMessage = userChatModel.LastMessage;
                member.LastMessageDate = DateUtils.DateForMessages(lastMessageDate);
                member.LastMessageDateTime = new DateTime(userChatModel.LastMessageDateTimeTicks);
                member.MemberPresence = MemberPresence.Online;

                if (!member.IsMemberMuted && _showNotifications)
                    AlertUser(member);

                if (!_isSearching)
                    RaisePropertyChanged(nameof(UpdateTableView));
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private async Task AlertUser(ChatListUserCellModel member)
        {
            if (NavigationService.ChatOpen() != member.MemberId)
            {
                _badgeForChat = true;
                RaisePropertyChanged(nameof(BadgeForChat));

                var result = await _dialogService.ShowMessageAlert(member.MemberPhoto, member.MemberName, member.LastMessage);

                if (result)
                {
                    BadgeForChat = false;
                    await NavigationService.NavigateAsync<ChatViewModel, int>(member.MemberId);
                }
            }
        }

        private async Task SetNotifications(bool value)
        {
            if (value)
                await Task.Delay(2000);

            _showNotifications = value;
        }

        private void UpdateChatList(bool updated = false)
        {
            _chatList = new List<ChatListUserCellModel>();
            _chatUserModel = Realm.All<ChatListUserModel>().ToList();

            foreach (var chat in _chatUserModel)
            {
                if (chat.IsArchived)
                    continue;

                var date = new DateTime(chat.LastMessageDateTimeTicks);

                var cht = new ChatListUserCellModel(chat.MemberId, chat.MemberName, chat.MemberPhoto, chat.LastMessage,
                                                    DateUtils.DateForMessages(date), chat.IsNewMessage, chat.IsMemberMuted, chat.UnreadMessagesCount,
                                                    OpenUserProfileEvent, OpenUserChatEvent, date);

                TimeSpan timeDifference = DateTime.Now.Subtract(new DateTime(chat.MemberPresenceConnectionDate));

                cht.MemberPresence = (chat.MemberPresence == 0 && updated == true) || (chat.MemberPresence == 0 && timeDifference.TotalMinutes < 5.0f)
                    ? MemberPresence.Online
                    : timeDifference.TotalMinutes < 30.0f ? MemberPresence.Recent : MemberPresence.Offline;

                _chatList.Add(cht);
            }

            if (_chatList.Count > 0)
                RaisePropertyChanged(nameof(ChatList));
        }

        private async Task UpdateMessengerService()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsLoading = false;
                return;
            }

            if (DateTime.Now >= _updateFrequence)
            {
                var newChatList = new List<ChatListUserModel>();

                try
                {
                    bool newUsersCheck = false;

                    var allChannels = await _messagerService.GetAllChannels();

                    foreach (var channel in allChannels)
                    {
                        var users = await _messagerService.CheckUsersInGroupPresence(channel);

                        foreach (var user in users.ToList())
                        {
                            var userId = StringUtils.GetUserId(user.UserId);
                            var userInDB = _users.Find(x => x.UserId == userId);
                            var userInModel = _chatUserModel?.Find(x => x?.MemberId == userId);

                            if (userId == _thisUserId)
                                continue;

                            if (userInDB == null && !newUsersCheck)
                            {
                                newUsersCheck = true;
                                await GetUsers();
                                userInDB = _users.Find(x => x.UserId == userId);
                            }

                            if (userInDB == null)
                                continue;

                            var lastMessageDate = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(channel.LastMessage.CreatedAt.ToString())).ToLocalTime();
                            long userLastSeen = DateTime.Now.AddMilliseconds(-user.LastSeenAt).Ticks;

                            var usr = new ChatListUserModel
                            {
                                MemberId = userInDB.UserId,
                                MemberName = $"{userInDB.FirstName} {userInDB.LastName} - {userInDB.Position}",
                                MemberPhoto = userInDB.Picture,
                                IsMemberMuted = userInModel != null && userInModel.IsMemberMuted,
                                IsNewMessage = channel.UnreadMessageCount > 0,
                                MemberPresence = user.ConnectionStatus == User.UserConnectionStatus.ONLINE ? 0 : 1,
                                MemberPresenceConnectionDate = userLastSeen,
                                LastMessageDateTimeTicks = lastMessageDate.Ticks,
                                IsArchived = userInModel != null && (DateTime.Compare(new DateTime(userInModel.ArchivedTime), lastMessageDate) >= 0 && userInModel.IsArchived),
                                ArchivedTime = userInModel != null ? userInModel.ArchivedTime : 0,
                                UnreadMessagesCount = channel.UnreadMessageCount,
                            };

                            if (channel.LastMessage is UserMessage um)
                                usr.LastMessage = um.Sender.UserId == _thisUserFinalId ? $"{YouChatLabel} {um.Message}" : um.Message;
                            else if (channel.LastMessage is FileMessage fm)
                                usr.LastMessage = fm.Sender.UserId == _thisUserFinalId ? $"{YouChatLabel} {SentImage}" : SentImage;
                            else
                                usr.LastMessage = string.Empty;

                            if (userInModel != null)
                            {
                                foreach (var msg in userInModel?.MessagesList)
                                    usr.MessagesList.Add(msg);
                            }

                            newChatList.Add(usr);
                        }
                    }

                    Realm.Write(() =>
                    {
                        foreach (var chat in newChatList)
                        {
                            Realm.Add(chat, true);
                        }
                    });

                    Debug.WriteLine("UpdateMessengerService with chats count: " + newChatList.Count);

                    if (!_isSearching)
                        UpdateChatList(true);
                }
                catch (Exception ex)
                {
                    _updateFrequence = default(DateTime);
                    Ui.Handle(ex as dynamic);
                }
                finally
                {
                    IsLoading = false;
                    _updateFrequence = DateTime.Now.AddMinutes(4);
                }
            }
            else
                IsLoading = false;
        }

        private async Task OpenContacts()
        {
            await NavigationService.NavigateAsync<ContactListViewModel, ContactsType>(ContactsType.Chat);
        }

        private void RowAction(Tuple<ChatEventType, int> action)
        {
            var userId = action.Item2;
            var _chatUser = _chatUserModel.FindLast(x => x.MemberId == userId);

            switch (action.Item1)
            {
                case ChatEventType.Archive:
                    Realm.Write(() =>
                    {
                        _chatUser.IsArchived = true;
                        _chatUser.ArchivedTime = DateTime.Now.Ticks;
                    });
                    _chatList.Remove(_chatList.FindLast(x => x.MemberId == userId));

                    if (_chatList.Count == 0)
                        RaisePropertyChanged(nameof(NoChats));

                    break;

                case ChatEventType.Mute:
                    Realm.Write(() => _chatUser.IsMemberMuted = !_chatUser.IsMemberMuted);
                    _chatList.FindLast(x => x.MemberId == userId).IsMemberMuted = _chatUser.IsMemberMuted;
                    break;

                default:
                    break;
            }
        }

        public Task<Tuple<BaseChannel, BaseMessage>> ChatHandler()
        {
            var tcs = new TaskCompletionSource<Tuple<BaseChannel, BaseMessage>>();

            Debug.WriteLine("Initializing SendBird Handlers in Chat");

            SendBirdClient.ChannelHandler ch = new SendBirdClient.ChannelHandler
            {
                OnMessageReceived = MessageListner
            };

            SendBirdClient.AddChannelHandler("ChatListHandler", ch);

            Debug.WriteLine("SendBird Handlers Initialized");

            return tcs.Task;
        }

        private void MessageListner(BaseChannel channel, BaseMessage message)
        {
            MainThread.BeginInvokeOnMainThread(() => { 
                CheckForMessagesHandler(channel, message); 
            });
        }

        private void SearchChat(string search)
        {
            _isSearching = true;
            _chatListSearch = _chatList.FindAll(x => x.MemberName.ToLower().Contains(search));
            RaisePropertyChanged(nameof(ChatList));
        }

        private void CloseSearch()
        {
            _isSearching = false;
            RaisePropertyChanged(nameof(ChatList));
        }

        private async Task HandleConnection()
        {
            Connectivity.ConnectivityChanged -= ConnectivityChanged;
            Connectivity.ConnectivityChanged += ConnectivityChanged;
        }

        private void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                _updateFrequence = default(DateTime);
                Appearing();
            }
        }

        private void OpenUserChatEvent(object sender, int userId) => NavigateToUserChat(userId);
        private void OpenUserProfileEvent(object sender, int userId) => NavigateToUserProfile(userId);
        private async Task NavigateToUserChat(int userId) => await NavigationService.NavigateAsync<ChatViewModel, int>(userId);
        private async Task NavigateToUserProfile(int userId) => await NavigationService.NavigateAsync<MemberViewModel, int>(userId);

        #region Resources

        public string Title          => L10N.Localize("ChatList_Title");
        public string NoRecentChat   => L10N.Localize("ChatList_NoChats");
        private string YouChatLabel  => L10N.Localize("ChatList_You");
        private string ArchiveAction => L10N.Localize("ChatList_Archive");
        private string MuteAction    => L10N.Localize("ChatList_Mute");
        private string UnMuteAction  => L10N.Localize("ChatList_UnMute");
        private string SentImage     => L10N.Localize("ChatList_SentImage");

        public enum ChatEventType
        {
            Mute,
            Archive
        }

        #endregion
    }
}
