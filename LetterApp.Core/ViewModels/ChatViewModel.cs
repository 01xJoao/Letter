using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;
using SendBird;

namespace LetterApp.Core.ViewModels
{
    public class ChatViewModel : XViewModel<int>
    {
        private readonly IDivisionService _divisionService;
        private readonly IContactsService _contactService;
        private readonly IMessengerService _messengerService;
        private readonly IDialogService _dialogService;

        private bool _updated;
        private int _differentDateCount;
        private int _messagesInDate;

        private PreviousMessageListQuery _PrevMessageListQuery;
        private GetUsersInDivisionModel _user;
        private ChatListUserModel _userChat;
        private GroupChannel _channel;
        private DateTime _lastDayMessage;

        private int _userId;
        private int _organizationId = AppSettings.OrganizationId;
        private string _finalUserId = AppSettings.UserAndOrganizationIds;

        private ChatModel _chat;
        public ChatModel Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }

        private UserModel _thisUser;
        private List<MessagesModel> _messagesModel;
        private List<ChatMessagesModel> _chatMessages;
        private Dictionary<int, Tuple<string, int>> _sectionsAndRowsCount;

        private string _status;
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private XPCommand<string> _sendMessageCommand;
        public XPCommand<string> SendMessageCommand => _sendMessageCommand ?? (_sendMessageCommand = new XPCommand<string>(async (msg) => await SendMessage(msg), CanExecuteMsg));

        private XPCommand _viewWillCloseCommand;
        public XPCommand ViewWillCloseCommand => _viewWillCloseCommand ?? (_viewWillCloseCommand = new XPCommand(ViewWillClose));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public ChatViewModel(IContactsService contactsService, IMessengerService messengerService, IDialogService dialogService, IDivisionService divisionService)
        {
            _divisionService = divisionService;
            _contactService = contactsService;
            _messengerService = messengerService;
            _dialogService = dialogService;
        }

        protected override void Prepare(int userId)
        {
            _userId = userId;
        }

        public override async Task InitializeAsync()
        {
            string fromDivision = string.Empty;

            _userChat = Realm.Find<ChatListUserModel>(_userId);

            var users = Realm.All<GetUsersInDivisionModel>();
            _user = users?.First(x => x.UserId == _userId);

            _thisUser = Realm.Find<UserModel>(AppSettings.UserId);

            if (_user == null || _thisUser == null)
            {
                CloseView();
                return;
            }

            if (_thisUser.Divisions.Count > 1)
            {
                var division = Realm.Find<DivisionModelProfile>(_user.MainDivisionId);

                if (division == null)
                {
                    try
                    {
                        division = await _divisionService.GetDivisionProfile(_user.MainDivisionId);
                        Realm.Write(() => Realm.Add(division, true));

                        fromDivision = division?.Name;
                    }
                    catch (Exception ex)
                    {
                        Ui.Handle(ex as dynamic);
                    }
                }
                else
                {
                    fromDivision = division?.Name;
                }
            }

            MessagesLogic(_userChat?.MessagesList);

            var chat = new ChatModel
            {
                MemberId = _userId,
                MemberName = $"{_user?.FirstName} {_user?.LastName}",
                MemberDetails = string.IsNullOrEmpty(fromDivision) ? _user?.Position : $"{fromDivision} - {_user?.Position}",
                MemberPhoto = _user.Picture,
                MemberEmail = _user.Email,
                MemberMuted = _userChat != null && _userChat.IsMemeberMuted,
                MemberSeenMyLastMessage = _userChat != null && _userChat.MemberSeenMyLastMessage,
                //MemberPresence = (MemberPresence)_userChat.MemberPresence,
                Messages = _chatMessages,
                MessageEvent = MessageClickEvent,
                SectionsAndRowsCount = _sectionsAndRowsCount,
            };

            _chat = chat;

            try
            {
                _channel = await _messengerService.GetCurrentChannel($"{_userId}-{_organizationId}");

                if (_channel == null)
                    _channel = await _messengerService.CreateChannel(new List<string> { $"{_userId}-{_organizationId}" });

                _PrevMessageListQuery = _channel.CreatePreviousMessageListQuery();
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }

            LoadPreviousMessages();
        }

        private async Task LoadPreviousMessages()
        {
            _messagesModel = new List<MessagesModel>();

            if (_PrevMessageListQuery != null)
            {
                try
                {
                    var result = await _messengerService.LoadMessages(_PrevMessageListQuery);

                    foreach (BaseMessage message in result)
                    {
                        if (message is UserMessage msg)
                        {
                            var newMessage = new MessagesModel
                            {
                                MessageId = msg.MessageId,
                                MessageType = 0,
                                MessageData = msg.Message,
                                MessageSenderId = msg.Sender.UserId,
                                MessageDateTicks = DateTime.Parse(msg.Data).ToLocalTime().Ticks
                            };

                            _messagesModel.Add(newMessage);
                        }
                        else
                        {
                            if (!(message is FileMessage mensg))
                                continue;

                            var newMessage = new MessagesModel
                            {
                                MessageId = mensg.MessageId,
                                MessageType = mensg.Type == "IMAGE" ? 1 : 2,
                                MessageData = mensg.Url,
                                MessageSenderId = mensg.Sender.UserId,
                                MessageDateTicks = DateTime.Parse(mensg.Data).ToLocalTime().Ticks,
                                CustomData = mensg.Name
                            };

                            _messagesModel.Add(newMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Ui.Handle(ex as dynamic);
                }
            }

            MessagesLogic(_messagesModel, _updated);
            _updated = true;

            bool shouldUpdate = _chat.Messages?.Last()?.MessageId != _chatMessages?.Last()?.MessageId;

            _chat.Messages = _chatMessages;
            _chat.SectionsAndRowsCount = _sectionsAndRowsCount;

            if(shouldUpdate)
                RaisePropertyChanged(nameof(Chat));
        }

        private void MessagesLogic(IList<MessagesModel> messagesList, bool shouldKeepOldMessages = false)
        {
            if (messagesList == null || messagesList?.Count == 0)
                return;
                
            if (!shouldKeepOldMessages || _chatMessages == null || _sectionsAndRowsCount == null)
            {
                _chatMessages = new List<ChatMessagesModel>();
                _sectionsAndRowsCount = new Dictionary<int, Tuple<string, int>>();

                _differentDateCount = 0;
                _messagesInDate = 0;
            }

            var messageOrdered = messagesList.OrderBy(x => x.MessageDateTicks).ToList();

            _lastDayMessage = new DateTime(messageOrdered.First().MessageDateTicks).Date;

            foreach (MessagesModel message in messageOrdered)
            {
                if (string.IsNullOrEmpty(message.MessageData))
                    continue;

                var newMessage = new ChatMessagesModel();
                var dateMessage = new DateTime(message.MessageDateTicks);

                if (_lastDayMessage != dateMessage.Date)
                {
                    _sectionsAndRowsCount.Add(_differentDateCount, new Tuple<string, int>(_lastDayMessage.ToString("MMMM, dd"), _messagesInDate));

                    _lastDayMessage = dateMessage.Date;
                    _differentDateCount++;
                    _messagesInDate = 1;

                    newMessage.PresentMessage = (PresentMessageType)message.MessageType;
                    newMessage.Name = message.MessageSenderId == _finalUserId ? $"{_thisUser.FirstName} {_thisUser.LastName}" : $"{_user.FirstName} {_user.LastName}";
                    newMessage.Picture = message.MessageSenderId == _finalUserId ? _thisUser.Picture : _user.Picture;
                }
                else
                {
                    _messagesInDate++;

                    if (_chatMessages.Count == 0 || _chatMessages[_chatMessages.Count - 1].MessageSenderId != message.MessageSenderId)
                    {
                        newMessage.PresentMessage = (PresentMessageType)message.MessageType;
                        newMessage.Name = message.MessageSenderId == _finalUserId ? $"{_thisUser.FirstName} {_thisUser.LastName}" : $"{_user.FirstName} {_user.LastName}";
                        newMessage.Picture = message.MessageSenderId == _finalUserId ? _thisUser.Picture : _user.Picture;
                    }
                    else
                        newMessage.PresentMessage = (PresentMessageType)(message.MessageType + 3);
                }

                newMessage.MessageId = message.MessageId;
                newMessage.MessageData = message.MessageData;
                newMessage.MessageType = (MessageType)message.MessageType;
                newMessage.MessageSenderId = message.MessageSenderId;
                newMessage.CustomData = message.CustomData;
                newMessage.MessageDate = $"  •  {dateMessage.ToString("HH:mm")}";
                newMessage.MessageDateTime = dateMessage;
                newMessage.ShowPresense = message.MessageSenderId != _finalUserId;

                _chatMessages.Add(newMessage);
            }

            if (_messagesInDate > 0 && !shouldKeepOldMessages)
            {
                _sectionsAndRowsCount.Add(_differentDateCount, new Tuple<string, int>(_chatMessages.Last().MessageDateTime.ToString("MMMM, dd"), _messagesInDate));
            }
            else
            {
                var sameDay = _chatMessages[_chatMessages.Count - 2].MessageDateTime.Date == _chatMessages[_chatMessages.Count - 1].MessageDateTime.Date;

                if (sameDay)
                {
                    _sectionsAndRowsCount[_sectionsAndRowsCount.Count - 1] = new Tuple<string, int>
                        (_chatMessages.Last().MessageDateTime.ToString("MMMM, dd"), _sectionsAndRowsCount[_sectionsAndRowsCount.Count - 1].Item2 + 1);
                }
                else
                    _sectionsAndRowsCount.Add(_sectionsAndRowsCount.Count, new Tuple<string, int>(_chatMessages.Last().MessageDateTime.ToString("MMMM, dd"),1));
            }
        }

        private async Task SendMessage(string msg)
        {
            try
            {
                var result = await _messengerService.SendMessage(_channel, msg, DateTime.UtcNow.ToString());

                if(result != null)
                {
                    var newMessage = new MessagesModel
                    {
                        MessageId = result.MessageId,
                        MessageType = 0,
                        MessageData = result.Message,
                        MessageSenderId = result.Sender.UserId,
                        MessageDateTicks = DateTime.Parse(result.Data).ToLocalTime().Ticks
                    };

                    _messagesModel.Add(newMessage);

                    MessagesLogic(_messagesModel, _updated);

                    _updated = true;

                    _chat.Messages = _chatMessages;
                    _chat.SectionsAndRowsCount = _sectionsAndRowsCount;
                    RaisePropertyChanged(nameof(Chat));
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private void MessageClickEvent(object sender, int e)
        {
        }

        private async Task CloseView()
        {
            ViewWillClose();
            await NavigationService.Close(this);
        }

        private void ViewWillClose()
        {
            if (_chat?.Messages?.Count > 0 && _userChat != null)
            {
                Realm.Write(() =>
                {
                    var lastMessage = _chat.Messages.Last();

                    var userChat = new ChatListUserModel()
                    {
                        MemberId = _user.UserId,
                        MemberName = $"{_user.FirstName} {_user.LastName} - {_user.Position}",
                        MemberPhoto = _user.Picture,
                        IsMemeberMuted = _userChat.IsMemeberMuted,
                        IsNewMessage = false,
                        MemberPresence = (int)_chat.MemberPresence,
                        MemberPresenceConnectionDate = _userChat.MemberPresenceConnectionDate,
                        //TODO LastMessage: Add logic for file and images
                        LastMessage = _chat.Messages.Last().MessageSenderId == _finalUserId ? $"{YouChatLabel} {lastMessage.MessageData}" : lastMessage.MessageData,
                        LastMessageDateTimeTicks = lastMessage.MessageDateTime.Ticks,
                        IsArchived = _chat.MemberArchived,
                        ArchivedTime = _chat.MemberMuted ? DateTime.Now.Ticks : default(DateTime).Ticks,
                        UnreadMessagesCount = 0,
                        MemberSeenMyLastMessage = _chat.MemberSeenMyLastMessage,
                    };

                    var historyMessages = _messagesModel?.Take(30) ?? _userChat?.MessagesList;

                    foreach (var msg in historyMessages)
                        userChat.MessagesList.Add(msg);

                    Realm.Add(userChat, true);
                });
            }
        }

        private bool CanExecute() => !IsBusy;
        private bool CanExecuteMsg(string msg) => !IsBusy;

        #region Resources 

        private string YouChatLabel => L10N.Localize("ChatList_You");
        public string TypeSomething => L10N.Localize("OnBoardingViewModel_LetterSlogan");

        #endregion
    }
}
