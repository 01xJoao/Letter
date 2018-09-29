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
using Xamarin.Essentials;

namespace LetterApp.Core.ViewModels
{
    public class ChatViewModel : XViewModel<int>
    {
        private readonly IDivisionService _divisionService;
        private readonly IContactsService _contactService;
        private readonly IMessengerService _messengerService;
        private readonly IDialogService _dialogService;

        //private bool _updated;
        private int _differentDateCount;
        private int _messagesInDate;

        private PreviousMessageListQuery _PrevMessageListQuery;
        private GetUsersInDivisionModel _user;
        private ChatListUserModel _userChat;
        private GroupChannel _channel;
        private DateTime _lastDayMessage;
        private DateTime _sendedMessageDateTime;

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

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string MemberName;
        public string MemberDetails;

        public bool Status;
        public List<MessagesModel> SendedMessages = new List<MessagesModel>();

        private XPCommand<Tuple<string, MessageType>> _sendMessageCommand;
        public XPCommand<Tuple<string, MessageType>> SendMessageCommand => _sendMessageCommand ?? (_sendMessageCommand = new XPCommand<Tuple<string, MessageType>>(async (msg) => await SendMessage(msg), CanExecuteMsg));

        private XPCommand _viewWillCloseCommand;
        public XPCommand ViewWillCloseCommand => _viewWillCloseCommand ?? (_viewWillCloseCommand = new XPCommand(ViewWillClose));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        private XPCommand _loadMessagesCommand;
        public XPCommand LoadMessagesCommand => _loadMessagesCommand ?? (_loadMessagesCommand = new XPCommand(async () => { await Task.Delay(2000); await LoadPreviousMessages(); }));

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
            _isLoading = true;

            CheckConnection();

            string fromDivision = string.Empty;

            _userChat = Realm.Find<ChatListUserModel>(_userId);
            var users = Realm.All<GetUsersInDivisionModel>();
            _user = users?.First(x => x.UserId == _userId);
            _thisUser = Realm.Find<UserModel>(AppSettings.UserId);

            if (_user == null || _thisUser == null) {
                _dialogService.ShowAlert(UserNotFound, AlertType.Error, 4f);
                CloseView();
                return;
            }

            if (_thisUser.Divisions.Count > 1)
            {
                var division = Realm.Find<DivisionModelProfile>(_user.MainDivisionId);

                if (division == null)
                {
                    MemberName = $"{_user?.FirstName} {_user?.LastName}";
                    MemberDetails = _user?.Position;

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
                if (await CheckMessageServiceConnection())
                    _PrevMessageListQuery = _channel.CreatePreviousMessageListQuery();
                else
                    return;
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }

            LoadPreviousMessages();
        }

        private async Task LoadPreviousMessages()
        {
            if (!await CheckMessageServiceConnection()) return;

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
                finally
                {
                    IsLoading = false;
                }
            }

            MessagesLogic(_messagesModel, false);
            //_updated = true;

            bool shouldUpdate = _chat.Messages?.Last().MessageId != _chatMessages?.Last()?.MessageId;

            _chat.Messages = _chatMessages;
            _chat.SectionsAndRowsCount = _sectionsAndRowsCount;

            if(shouldUpdate)
                RaisePropertyChanged(nameof(Chat));
        }

        private async Task<bool> CheckMessageServiceConnection()
        {
            IsBusy = true;

            var tcs = new TaskCompletionSource<bool>();

            try
            {
                if (SendBirdClient.GetConnectionState() != SendBirdClient.ConnectionState.OPEN)
                    await _messengerService.ConnectMessenger();

                _channel = await _messengerService.GetCurrentChannel($"{_userId}-{_organizationId}");

                if (_channel == null)
                    _channel = await _messengerService.CreateChannel(new List<string> { $"{_userId}-{_organizationId}" });

                if (_channel.MemberCount < 2)
                {
                    _messengerService.RemoveChannel(_channel);
                    _dialogService.ShowAlert(UserNotRegistered, AlertType.Error, 4f);

                    if (_userChat != null)
                        Realm.Write(() => Realm.Remove(_userChat));

                    _chat = null;
                    await CloseView();
                    return false;
                }

                return SendBirdClient.GetConnectionState() == SendBirdClient.ConnectionState.OPEN;
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CheckConnection()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
                ConnectToMessenger();
        }

        private async Task ConnectToMessenger()
        {
            if (await CheckMessageServiceConnection())
                LoadPreviousMessages();
        }

        private bool MessagesLogic(IList<MessagesModel> messagesList, bool shouldKeepOldMessages = false)
        {
            if (messagesList == null || messagesList?.Count == 0)
                return false;

            bool updateTableView = !shouldKeepOldMessages;
                
            if (!shouldKeepOldMessages || _chatMessages == null || _sectionsAndRowsCount == null)
            {
                _chatMessages = new List<ChatMessagesModel>();
                _sectionsAndRowsCount = new Dictionary<int, Tuple<string, int>>();

                _differentDateCount = 0;
                _messagesInDate = 0;
            }

            var messageOrdered = messagesList.OrderBy(x => x.MessageDateTicks).ToList();

            if(_lastDayMessage == default(DateTime))
                _lastDayMessage = new DateTime(messageOrdered.First().MessageDateTicks).Date;

            foreach (MessagesModel message in messageOrdered)
            {
                if (string.IsNullOrEmpty(message.MessageData))
                    continue;

                var newMessage = new ChatMessagesModel();
                var dateMessage = new DateTime(message.MessageDateTicks);

                if (_lastDayMessage != dateMessage.Date && !shouldKeepOldMessages)
                {
                    _sectionsAndRowsCount.Add(_differentDateCount, new Tuple<string, int>(_lastDayMessage.ToString("dd MMM"), _messagesInDate));

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

            //This is to add the Last Message
            if (_messagesInDate > 0 && !shouldKeepOldMessages)
            {
                _sectionsAndRowsCount.Add(_differentDateCount, new Tuple<string, int>(_chatMessages.Last().MessageDateTime.ToString("dd MMM"), _messagesInDate));
            }
            else if (_messagesInDate > 0)
            {
                int sectionsCount = _sectionsAndRowsCount.Count;
                bool sameDay = false;

                if (_chatMessages.Count > 2)
                        sameDay = _chatMessages[_chatMessages.Count - 2].MessageDateTime.Date == _chatMessages[_chatMessages.Count - 1].MessageDateTime.Date;

                if (sameDay)
                    _sectionsAndRowsCount[sectionsCount - 1] = new Tuple<string, int>(_chatMessages.Last().MessageDateTime.ToString("dd MMM"), _sectionsAndRowsCount[sectionsCount - 1].Item2 + 1);
                else
                {
                    _sectionsAndRowsCount.Add(sectionsCount, new Tuple<string, int>(_chatMessages.Last().MessageDateTime.ToString("dd MMM"), 1));
                    updateTableView = true;
                }
            }

            _lastDayMessage = new DateTime(messageOrdered.Last().MessageDateTicks).Date;

            return updateTableView;
        }

        private async Task SendMessage(Tuple<string, MessageType> message)
        {
            await CheckMessageServiceConnection();

            string messageToSend = message.Item1;

            while(messageToSend.EndsWith(Environment.NewLine)) {
                messageToSend = messageToSend.TrimEnd(Environment.NewLine.ToCharArray());
            }

            try
            {
                _sendedMessageDateTime = DateTime.UtcNow;

                var fakeMessage = new MessagesModel
                {
                    MessageId = _sendedMessageDateTime.Ticks,
                    MessageData = messageToSend,
                    MessageType = (int)message.Item2,
                    MessageSenderId = _finalUserId,
                    MessageDateTicks = _sendedMessageDateTime.ToLocalTime().Ticks,
                };

                MessagesLogic(new List<MessagesModel> { fakeMessage }, true);

                _chat.Messages = _chatMessages;
                _chat.SectionsAndRowsCount = _sectionsAndRowsCount;

                SendedMessages.Add(fakeMessage);
                RaisePropertyChanged(nameof(SendedMessages));

                var result = await _messengerService.SendMessage(_channel, messageToSend, _sendedMessageDateTime.ToString());

                if(result != null)
                {
                    var sendedmsg = SendedMessages.FirstOrDefault(x => new DateTime(x.MessageDateTicks).Date == DateTime.Parse(result.Data).ToLocalTime().Date);
                    SendedMessages.Remove(sendedmsg);

                    var msg = new MessagesModel
                    {
                        MessageId = result.MessageId,
                        MessageData = result.Message,
                        MessageType = (int)message.Item2,
                        MessageSenderId = result.Sender.UserId,
                        MessageDateTicks = DateTime.Parse(result.Data).ToLocalTime().Ticks
                    };

                    if (_messagesModel == null)
                        _messagesModel = new List<MessagesModel>();

                    _messagesModel.Add(msg);
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);

                foreach (var messageId in SendedMessages) {
                    _chat.Messages.Last(x => x.MessageId == messageId.MessageId).FailedToSend = true;
                }

                _dialogService.ShowAlert(SendMessageError, AlertType.Error, 4f);

                RaisePropertyChanged(nameof(Chat));
            }
        }

        private async Task RetrySendMessages()
        {
            try
            {
                if (await CheckMessageServiceConnection())
                {
                    var listMsg = new List<MessagesModel>();
                    foreach (var msg in SendedMessages) { listMsg.Add(msg); }

                    foreach(var msg in listMsg)
                    {
                        SendedMessages.Remove(msg);
                        await SendMessage(new Tuple<string, MessageType>(msg.MessageData, (MessageType)msg.MessageType));
                    }

                    foreach(var msg in listMsg)
                    {
                        _chat.Messages.Remove(_chat.Messages.Find(x => x.MessageId == msg.MessageId));
                    }

                    RaisePropertyChanged(nameof(Chat));

                    listMsg = null;
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private void MessageClickEvent(object sender, long messageId)
        {
            var message = SendedMessages.Find(x => x.MessageId == messageId);

            if(message != null)
                RetrySendMessages();
        }

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }

        private void ViewWillClose()
        {
            if (_chat?.Messages?.Count > 0)
            {
                Realm.Write(() =>
                {
                    var lastMessage = _chat.Messages.Last();

                    var userChat = new ChatListUserModel
                    {
                        MemberId = _user.UserId,
                        MemberName = $"{_user.FirstName} {_user.LastName} - {_user.Position}",
                        MemberPhoto = _user.Picture,
                        IsMemeberMuted = _chat.MemberMuted,
                        IsNewMessage = false,
                        MemberPresence = (int)_chat.MemberPresence,
                        MemberPresenceConnectionDate = _userChat != null ? _userChat.MemberPresenceConnectionDate : default(DateTime).Ticks,
                        //TODO LastMessage: Add logic for file and images
                        LastMessage = _chat.Messages.Last().MessageSenderId == _finalUserId ? $"{YouChatLabel} {lastMessage.MessageData}" : lastMessage.MessageData,
                        LastMessageDateTimeTicks = lastMessage.MessageDateTime.Ticks,
                        IsArchived = _chat.MemberArchived,
                        ArchivedTime = _chat.MemberMuted ? DateTime.Now.Ticks : default(DateTime).Ticks,
                        UnreadMessagesCount = 0,
                        MemberSeenMyLastMessage = _chat.MemberSeenMyLastMessage,
                    };

                    var historyMessages = _messagesModel?.Skip(Math.Max(0, _messagesModel.Count() - 30)) ?? _userChat?.MessagesList;

                    foreach (var msg in historyMessages)
                        userChat.MessagesList.Add(msg);

                    Realm.Add(userChat, true);
                });
            }
        }

        private bool CanExecute() => !IsBusy;
        private bool CanExecuteMsg(Tuple<string, MessageType> msg) => !IsBusy;

        #region Resources 

        private string YouChatLabel => L10N.Localize("ChatList_You");
        private string UserNotRegistered => L10N.Localize("ChatList_UserNotRegistered");
        private string SendMessageError => L10N.Localize("ChatList_MessageError");
        private string UserNotFound => L10N.Localize("ChatList_UserNotFound");
        public string TypeSomething => L10N.Localize("OnBoardingViewModel_LetterSlogan");

        public string SendingMessage => L10N.Localize("Chat_SendingMessage");
        public string TypingMessage => L10N.Localize("Chat_TypingMessage");
        public string SeenMessage => L10N.Localize("Chat_SeenMessage");
        public string SendMessageButton => L10N.Localize("Chat_SendMessage");

        #endregion
    }
}
