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
        private readonly IDialogService _dialogService;
        private readonly IContactsService _contactsService;

        private int _thisUserId => AppSettings.UserId;
        private string _thisUserFinalId => AppSettings.UserAndOrganizationIds;
        private bool _isSearching;

        public bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }


        public bool UpdateTableView { get; set; }
        public bool NoChats { get; set; }
        public string[] Actions;

        private DateTime _updateFrequence = default(DateTime);
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

        public ChatListViewModel(IContactsService contactsService, IMessengerService messagerService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _contactsService = contactsService;
            _messagerService = messagerService;
            Actions = new string[] { ArchiveAction, MuteAction, UnMuteAction };
            CheckForMessagesHandler();
        }

        public override async Task Appearing()
        {
            Debug.WriteLine("Appearing");

            if (_users == null || _users.Count == 0)
                await GetUsers();

            UpdateChatList();

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

        private async Task GetUsers()
        {
            try
            {
                _users = Realm.All<GetUsersInDivisionModel>().ToList();

                if (_users.Count == 0)
                {
                    var result = await _contactsService.GetUsersFromAllDivisions();

                    if (result == null && result.Count == 0)
                        return;

                    foreach (var res in result)
                    {
                        res.UniqueKey = $"{res.UserId}+{res.DivisionId}";
                        var contacNumber = res.ShowNumber ? res?.ContactNumber : string.Empty;
                        string[] stringSearch = { res?.FirstName?.ToLower(), res?.LastName?.ToLower(), res?.Position?.ToLower() };
                        stringSearch = StringUtils.NormalizeString(stringSearch);
                        res.SearchContainer = $"{stringSearch[0]}, {stringSearch[1]}, {stringSearch[2]}, {contacNumber} {res?.Email?.ToLower()}";

                        Realm.Write(() => { Realm.Add(res, true); });
                    }

                    _users = Realm.All<GetUsersInDivisionModel>().ToList();
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private async Task CheckForMessagesHandler()
        {
            try
            {
                var message = await _messagerService.InitializeHandlers();

                if (!(message is UserMessage msg))
                {
                    CheckForMessagesHandler();
                    return;
                }

                DateTime lastMessageDate = !string.IsNullOrEmpty(msg?.Data) ? DateTime.Parse(msg.Data) : DateTime.Now;

                var userChatModel = _chatUserModel.Find(x => x.MemberId == StringUtils.GetUserId(msg.Sender.UserId));

                if(userChatModel == null)
                {
                    var res = _users.Find(x => x.UserId == StringUtils.GetUserId(msg.Sender.UserId));

                    if (res == null)
                    {
                        await GetUsers();
                        var result = _users.Find(x => x.UserId == StringUtils.GetUserId(msg.Sender.UserId));

                        userChatModel = new ChatListUserModel
                        {
                            MemberId = result.UserId,
                            MemberName = $"{result.FirstName} {result.LastName} - {result.Position}",
                            MemberPhoto = result.Picture,
                            IsMemeberMuted = false,
                        };

                        Realm.Write(() => Realm.Add(userChatModel));
                    }
                }

                Realm.Write(() => {
                    userChatModel.MemberPresence = 0;
                    userChatModel.MemberPresenceConnectionDate = lastMessageDate.Ticks;
                    userChatModel.LastMessage = msg.Message;
                    userChatModel.LastMessageDateTimeTicks = lastMessageDate.Ticks;
                    userChatModel.IsArchived = false;
                    userChatModel.ShouldAlertNewMessage = true;
                });

                var member = _chatList.Find(x => x.MemberId == userChatModel.MemberId);

                if(member == null)
                {
                    member = new ChatListUserCellModel
                    { 
                        MemberId = userChatModel.MemberId,
                        MemberName = userChatModel.MemberName,
                        MemberPhoto = userChatModel.MemberPhoto,
                        IsMemberMuted = userChatModel.IsMemeberMuted,
                        OpenChat = OpenUserChatEvent,
                        OpenMemberProfile = OpenUserProfileEvent
                    };

                    _chatList.Add(member);
                }

                member.ShouldAlertNewMessage = userChatModel.ShouldAlertNewMessage;
                member.LastMessage = userChatModel.LastMessage;
                member.LastMessageDate = DateUtils.TimeForChat(lastMessageDate);
                member.LastMessageDateTime = new DateTime(userChatModel.LastMessageDateTimeTicks);
                member.MemberPresence = MemberPresence.Online;

                if(!member.IsMemberMuted)
                    AlertUser(member);

                if(!_isSearching)
                    RaisePropertyChanged(nameof(UpdateTableView));
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                CheckForMessagesHandler();
            }
        }

        private async Task AlertUser(ChatListUserCellModel member)
        {
            var result = await _dialogService.ShowMessageAlert(member.MemberPhoto, member.MemberName, member.LastMessage);

            if (result)
                await NavigationService.NavigateAsync<ChatViewModel, int>(member.MemberId);
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
                                                    DateUtils.TimeForChat(date), chat.ShouldAlertNewMessage, chat.IsMemeberMuted,
                                                    OpenUserProfileEvent, OpenUserChatEvent, date);

                TimeSpan timeDifference = DateTime.Now.Subtract(new DateTime(chat.MemberPresenceConnectionDate));

                cht.MemberPresence = (chat.MemberPresence == 0 && updated == true) || (chat.MemberPresence == 0 && timeDifference.TotalMinutes < 5.0f)
                    ? MemberPresence.Online
                    : timeDifference.TotalMinutes < 30.0f ? MemberPresence.Recent : MemberPresence.Offline;

                _chatList.Add(cht);
            }

            if (_chatList.Count > 0)
            {
                Debug.WriteLine("going to updateve from viewmode the TableView with chats count: " + _chatList.Count);

                RaisePropertyChanged(nameof(ChatList));
            }
        }

        private async Task UpdateMessengerService()
        {
            if (_users.Count == 0)
                return;

            if (DateTime.Now >= _updateFrequence)
            {
                if (_updateFrequence == default(DateTime))
                    IsLoading = true;

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

                            DateTime lastMessageDate = !string.IsNullOrEmpty(lastMessage.Data) ? DateTime.Parse(lastMessage.Data) : DateTime.Now;

                            long lastSeenTime;

                            if (userInModel != null)
                            {
                                lastSeenTime = userInModel.MemberPresenceConnectionDate >= user.LastSeenAt && userInModel.MemberPresenceConnectionDate <= lastMessageDate.Ticks
                                    ? lastMessageDate.Ticks
                                    : userInModel.MemberPresenceConnectionDate >= user.LastSeenAt ? userInModel.MemberPresenceConnectionDate : user.LastSeenAt;
                            }
                            else
                                lastSeenTime = default(DateTime).Ticks;

                            var userLastSeen = user.ConnectionStatus == User.UserConnectionStatus.ONLINE || user.LastSeenAt == 0 ? DateTime.Now.Ticks : lastSeenTime;

                            var shouldAlertMsg = !channel.GetReadMembers(channel.LastMessage).Any(x => x.UserId == _thisUserFinalId);

                            var usr = new ChatListUserModel
                            {
                                MemberId = userInDB.UserId,
                                MemberName = $"{userInDB.FirstName} {userInDB.LastName} - {userInDB.Position}",
                                MemberPhoto = userInDB.Picture,
                                IsMemeberMuted = userInModel != null && userInModel.IsMemeberMuted,
                                ShouldAlertNewMessage = shouldAlertMsg,
                                MemberPresence = user.ConnectionStatus == User.UserConnectionStatus.ONLINE ? 0 : 1,
                                MemberPresenceConnectionDate = userLastSeen,
                                LastMessage = lastMessage.Sender.UserId == _thisUserFinalId ? $"{YouChatLabel} {lastMessage.Message}" : lastMessage.Message,
                                LastMessageDateTimeTicks = lastMessageDate.Ticks,
                                IsArchived = userInModel != null && (DateTime.Compare(new DateTime(userInModel.ArchivedTime), lastMessageDate) >= 0 && userInModel.IsArchived),
                                ArchivedTime = userInModel != null ? userInModel.ArchivedTime : 0
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

                    if (!_isSearching)
                        UpdateChatList(true);
                }
                catch (Exception ex)
                {
                    Ui.Handle(ex as dynamic);
                }
                finally
                {
                    if (_updateFrequence == default(DateTime))
                        IsLoading = false;

                    _updateFrequence = DateTime.Now.AddMinutes(4);
                }
            }
        }

        private async Task CreateChannel()
        {
            try
            {
                var channel = await _messagerService.CreateChannel(new List<string> { "59-33" });

                if (channel != null)
                    await _messagerService.SendMessage(channel, "This is a text message to check how it works in two lines label, thank you.", DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private async Task OpenContacts()
        {
            UpdateChatList();
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
                    Realm.Write(() => _chatUser.IsMemeberMuted = !_chatUser.IsMemeberMuted);
                    _chatList.FindLast(x => x.MemberId == userId).IsMemberMuted = _chatUser.IsMemeberMuted;
                    break;

                default:
                    break;
            }
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

        private void OpenUserChatEvent(object sender, int userId) => NavigateToUserChat(userId);
        private void OpenUserProfileEvent(object sender, int userId) => NavigateToUserProfile(userId);
        private async Task NavigateToUserChat(int userId) => await NavigationService.NavigateAsync<ChatViewModel, int>(userId);
        private async Task NavigateToUserProfile(int userId) => await NavigationService.NavigateAsync<MemberViewModel, int>(userId);

        #region Resources

        public string Title => L10N.Localize("ChatList_Title");
        public string NoRecentChat => L10N.Localize("ChatList_NoChats");
        private string YouChatLabel => L10N.Localize("ChatList_You");

        private string ArchiveAction => L10N.Localize("ChatList_Archive");
        private string MuteAction => L10N.Localize("ChatList_Mute");
        private string UnMuteAction => L10N.Localize("ChatList_UnMute");

        public enum ChatEventType
        {
            Mute,
            Archive
        }

        #endregion
    }
}
