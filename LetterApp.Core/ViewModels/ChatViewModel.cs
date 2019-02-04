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

namespace LetterApp.Core.ViewModels
{
    public class ChatViewModel : XViewModel<int>
    {
        private readonly IDivisionService _divisionService;
        private readonly IContactsService _contactService;
        private readonly IMessengerService _messengerService;
        private readonly IDialogService _dialogService;
        private readonly IAudioService _audioService;
        private readonly IPicturePicker _picturePicker;

        private PreviousMessageListQuery _prevMessageListQuery;
        private GetUsersInDivisionModel _user;
        private ChatListUserModel _userChat;
        private GroupChannel _channel;

        private int _userId;
        private int _organizationId = AppSettings.OrganizationId;
        private string _finalUserId = AppSettings.UserAndOrganizationIds;
        public string MemberName;
        public string MemberDetails;
        public bool NewMessageAlert;
        private bool _isPickingImage;

        private ChatModel _chat;
        public ChatModel Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }

        private UserModel _thisUser;
        private List<MessagesModel> _messagesModel;
        private List<ChatMessagesModel> _chatMessages;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _status;
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public object ScrollToMiddle { get; private set; }
        public List<MessagesModel> SendedMessages = new List<MessagesModel>();
        private List<ChatMessagesModel> _failedMessages = new List<ChatMessagesModel>();
        private bool _isSendingFailedMessage = false;

        private XPCommand<Tuple<string, MessageType>> _sendMessageCommand;
        public XPCommand<Tuple<string, MessageType>> SendMessageCommand => _sendMessageCommand ?? (_sendMessageCommand = new XPCommand<Tuple<string, MessageType>>(async (msg) => await SendMessage(msg), CanExecuteMsg));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView()));

        private XPCommand<bool> _typingCommand;
        public XPCommand<bool> TypingCommand => _typingCommand ?? (_typingCommand = new XPCommand<bool>(TypingStatus));

        private XPCommand<bool> _loadMessagesUpdateReceiptCommand;
        public XPCommand<bool> LoadMessagesUpdateReceiptCommand => _loadMessagesUpdateReceiptCommand ?? (_loadMessagesUpdateReceiptCommand = new XPCommand<bool>(async (val) =>
        {
            if (!IsBusy)
            {
                IsBusy = true;
                _isLoading = val;
                RaisePropertyChanged(nameof(IsLoading));
                await Task.Delay(200);
                await LoadMessagesAndUpdateReadReceipt(val, val);
            }
        }, CanExecuteLoad));

        private XPCommand<bool> _chatHandlersCommand;
        public XPCommand<bool> ChatHandlersCommand => _chatHandlersCommand ?? (_chatHandlersCommand = new XPCommand<bool>(ChatHandlers));

        private XPCommand _callCommand;
        public XPCommand CallCommand => _callCommand ?? (_callCommand = new XPCommand(async () => await Call()));

        private XPCommand _optionsCommand;
        public XPCommand OptionsCommand => _optionsCommand ?? (_optionsCommand = new XPCommand(async () => await Options()));

        private XPCommand _fetchOldMessagesCommand;
        public XPCommand FetchOldMessagesCommand => _fetchOldMessagesCommand ?? (_fetchOldMessagesCommand = new XPCommand(async () => await FetchOldMessages(), CanExecute));

        private XPCommand _openImagesCommand;
        public XPCommand OpenImagesCommand => _openImagesCommand ?? (_openImagesCommand = new XPCommand(async () => await OpenImages()));

        public ChatViewModel(IContactsService contactsService, IMessengerService messengerService, IDialogService dialogService, IDivisionService divisionService, IAudioService audioService, IPicturePicker picturePicker)
        {
            _divisionService = divisionService;
            _contactService = contactsService;
            _messengerService = messengerService;
            _dialogService = dialogService;
            _audioService = audioService;
            _picturePicker = picturePicker;
            SetL10NResources();
        }

        protected override void Prepare(int userId)
        {
            _userId = userId;
        }

        public override async Task Appearing()
        {
            if (_isPickingImage || IsBusy || IsLoading)
                return;
               
            IsBusy = true;

            ChatHandlers();
            CheckConnection();
            NavigationService.ChatOpen(_userId);

            _isLoading = true;

            string fromDivision = string.Empty;

            try
            {
                _userChat = Realm.Find<ChatListUserModel>(_userId);
                var users = Realm.All<GetUsersInDivisionModel>();
                _user = users?.First(x => x.UserId == _userId);
                _thisUser = Realm.Find<UserModel>(AppSettings.UserId);
            }
            catch (Exception ex)
            {
                _dialogService.ShowAlert(UserNotFound, AlertType.Error, 4f);
                CloseView();
                return;
            }

            if (_user == null || _thisUser == null)
            {
                _dialogService.ShowAlert(UserNotFound, AlertType.Error, 4f);
                CloseView();
                return;
            }

            MemberName = $"{_user?.FirstName} {_user?.LastName}";
            MemberDetails = _user?.Position;

            if (string.IsNullOrEmpty(_user?.PushNotificationToken))
                await GetUserPushToken();
            else 
                GetUserPushToken();

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

            MessagesLogic(_userChat?.MessagesList, false);

            var chat = new ChatModel
            {
                MemberId = _userId,
                MemberName = $"{_user?.FirstName} {_user?.LastName}",
                MemberDetails = string.IsNullOrEmpty(fromDivision) ? _user?.Position : $"{fromDivision} - {_user?.Position}",
                MemberPhoto = _user.Picture,
                MemberEmail = _user.Email,
                MemberMuted = _userChat != null && _userChat.IsMemberMuted,
                MemberSeenMyLastMessage = _userChat != null && _userChat.MemberSeenMyLastMessage,
                MemberPresence = Connectivity.NetworkAccess == NetworkAccess.Internet && _userChat != null ? (MemberPresence)_userChat.MemberPresence : MemberPresence.Offline,
                Messages = _chatMessages ?? new List<ChatMessagesModel>(),
                MessageEvent = MessageClickEvent
            };

            _chat = chat;

            StatusLogic();

            await LoadMessagesAndUpdateReadReceipt(true, true);
            SaveChat();

            IsBusy = false;
        }

        private async Task LoadRecentMessages(bool loadOldMessages)
        {
            IsLoading = true;

            Debug.WriteLine("LoadRecentMessages");

            if (SendBirdClient.GetConnectionState() != SendBirdClient.ConnectionState.OPEN)
            {
                if(!await MessageServiceConnection())
                {
                    IsLoading = false;
                    return;
                }
            }
            else
            {
                if (!await GetChannel())
                {
                    IsLoading = false;
                    return;
                }
            }

            if (!loadOldMessages)
            {
                _prevMessageListQuery = _channel?.CreatePreviousMessageListQuery();
                _messagesModel = new List<MessagesModel>();
            }

            try
            {
                var result = await _messengerService.LoadMessages(_prevMessageListQuery, 30);

                if (result != null || result.Count > 0)
                {
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
                                MessageDateTicks = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(message.CreatedAt.ToString())).ToLocalTime().Ticks
                            };

                            _messagesModel.Add(newMessage);
                        }
                        else if(message is FileMessage mensg)
                        {
                            var newMessage = new MessagesModel
                            {
                                MessageId = mensg.MessageId,
                                MessageType = mensg.Type == "image/jpeg" ? 1 : 2,
                                MessageData = mensg.Url,
                                MessageSenderId = mensg.Sender.UserId,
                                MessageDateTicks = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(message.CreatedAt.ToString())).ToLocalTime().Ticks,
                                CustomData = mensg.Name
                            };

                            _messagesModel.Add(newMessage);
                        }
                    }
                }
                else
                {
                    IsLoading = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Ui.Handle(ex as dynamic);
                return;
            }

            if(_messagesModel?.Count > 0)
                _chat.Messages = new List<ChatMessagesModel>();

            MessagesLogic(_messagesModel, false);

            if(_chatMessages?.Count > 0)
                _chat.Messages = _chatMessages?.OrderBy(x => x?.MessageDateTime)?.ToList();
                
            RaisePropertyChanged(nameof(Chat));
        }

        private void MessagesLogic(IList<MessagesModel> messagesList, bool shouldKeepOldMessages)
        {
            if (messagesList == null || messagesList?.Count == 0)
                return;
                
            if (shouldKeepOldMessages == false || _chatMessages == null) {
                _chatMessages = new List<ChatMessagesModel>();
            }

            var messageOrdered = messagesList.OrderBy(x => x.MessageDateTicks).ToList();

            foreach (MessagesModel message in messageOrdered)
            {
                if (string.IsNullOrEmpty(message.MessageData))
                    continue;

                var newMessage = new ChatMessagesModel();
                var massageDate = new DateTime(message.MessageDateTicks);

                if (_chatMessages.Count == 0)
                {
                    newMessage.ShowHeaderDate = true;
                    newMessage.PresentMessage = (PresentMessageType)message.MessageType;
                }
                else
                {
                    newMessage.ShowHeaderDate = _chatMessages[_chatMessages.Count - 1].MessageDateTime.Date != massageDate.Date;

                    if (newMessage.ShowHeaderDate || _chatMessages[_chatMessages.Count - 1].MessageSenderId != message.MessageSenderId)
                        newMessage.PresentMessage = (PresentMessageType)message.MessageType;
                    else if (DateTime.Compare(_chatMessages[_chatMessages.Count - 1].MessageDateTime.AddHours(1), new DateTime(message.MessageDateTicks)) <= 0 )
                        newMessage.PresentMessage = (PresentMessageType)message.MessageType;
                    else
                        newMessage.PresentMessage = (PresentMessageType)(message.MessageType + 3);
                }

                if (newMessage.ShowHeaderDate)
                    newMessage.HeaderDate = DateUtils.DateForChatHeader(massageDate).ToUpper();

                string dateForMessage = L10N.Locale() == "en-US" ? massageDate.ToString("hh:mm tt") : massageDate.ToString("HH:mm");

                newMessage.Name = message.MessageSenderId == _finalUserId ? $"{_thisUser.FirstName} {_thisUser.LastName}" : $"{_user.FirstName} {_user.LastName}";
                newMessage.Picture = message.MessageSenderId == _finalUserId ? _thisUser.Picture : _user.Picture;
                newMessage.MessageId = message.MessageId;
                newMessage.MessageData = message.MessageData;
                newMessage.MessageSenderId = message.MessageSenderId;
                newMessage.CustomData = message.CustomData;
                newMessage.MessageDate = $"  •  {dateForMessage}";
                newMessage.MessageDateTime = massageDate;
                newMessage.MessageType = (MessageType)message.MessageType;
                newMessage.ShowPresense = message.MessageSenderId != _finalUserId;
                newMessage.IsFakeMessage = message.IsFakeMessage;

                _chatMessages.Add(newMessage);
            }
        }

        private async Task OpenImages()
        {
            IsBusy = true;
            _isPickingImage = true;

            try
            {
                var result = await _picturePicker.GetImageFilePath();

                if (result != null)
                {
                    if (await _dialogService.ShowPicture(result, SendMessageButton, CancelButton))
                        await SendMessage(new Tuple<string, MessageType>(result, MessageType.Image), false);
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                IsBusy = false;
                _isPickingImage = false;
            }
        }

        private async Task SendMessage(Tuple<string, MessageType> message, bool sendingFailedMessages = false)
        {
            string messageToSend = message.Item1;

            while(messageToSend.EndsWith(Environment.NewLine)) {
                messageToSend = messageToSend.TrimEnd(Environment.NewLine.ToCharArray());
            }

            try
            {
                var _sendedMessageDateTime = DateTime.UtcNow;

                var fakeMessage = new MessagesModel
                {
                    MessageId = _sendedMessageDateTime.Ticks,
                    MessageData = messageToSend,
                    MessageType = (int)message.Item2,
                    MessageSenderId = _finalUserId,
                    MessageDateTicks = _sendedMessageDateTime.ToLocalTime().Ticks,
                    IsFakeMessage = true
                };

                MessagesLogic(new List<MessagesModel> { fakeMessage }, true);

                _chat.Messages = _chatMessages;
                SendedMessages.Add(fakeMessage);

                if (!sendingFailedMessages)
                {
                    RaisePropertyChanged(nameof(SendedMessages));
                    _audioService.PlaySendMessage();
                }

                _status = string.Empty;
                RaisePropertyChanged(nameof(Status));

                BaseMessage result;

                if(SendBirdClient.GetConnectionState() != SendBirdClient.ConnectionState.OPEN && Connectivity.NetworkAccess == NetworkAccess.Internet)
                    await WaitForConnection();

                if ((MessageType)fakeMessage.MessageType == MessageType.Text)
                {
                    result = await _messengerService.SendMessage(_channel, _thisUser.UserID.ToString(), $"{_thisUser.FirstName} {_thisUser.LastName}",
                                                                 _user.PushNotificationToken, messageToSend, _sendedMessageDateTime.ToString());
                }
                else
                {
                    result = await _messengerService.SendImage(_channel, _thisUser.UserID.ToString(), $"{_thisUser.FirstName} {_thisUser.LastName}",
                                                                 _user.PushNotificationToken, messageToSend, _sendedMessageDateTime.ToString());
                }

                if(result != null)
                {
                    if (_messagesModel == null)
                        _messagesModel = new List<MessagesModel>();

                    if ((MessageType)fakeMessage.MessageType == MessageType.Text)
                    {
                        var res = result as UserMessage;

                        var msgTime = DateTime.Parse(res.Data).ToLocalTime().ToString("d hh:mm:ss");

                        var sendedmsg = SendedMessages.LastOrDefault(x => new DateTime(x.MessageDateTicks).ToString("d hh:mm:ss") == msgTime);
                        SendedMessages.Remove(sendedmsg);

                        var msg = new MessagesModel
                        {
                            MessageId = res.MessageId,
                            MessageData = res.Message,
                            MessageType = (int)message.Item2,
                            MessageSenderId = res.Sender.UserId,
                            MessageDateTicks = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(res.CreatedAt.ToString())).ToLocalTime().Ticks
                        };

                        _messagesModel.Add(msg);

                        if (_chat.Messages.Last().MessageDateTime.ToString("d hh:mm:ss") != msgTime && sendingFailedMessages == false)
                        {
                            if(!_chat.Messages.Any(x => x.MessageId == res.MessageId))
                            {
                                MessagesLogic(new List<MessagesModel> { fakeMessage }, true);
                                _chat.Messages = _chatMessages;
                                RaisePropertyChanged(nameof(SendedMessages));
                            }
                        }
                    }
                    else
                    {
                        var res = result as FileMessage;

                        var msgTime = DateTime.Parse(res.Data).ToLocalTime().ToString("d hh:mm:ss");

                        var sendedmsg = SendedMessages.LastOrDefault(x => new DateTime(x.MessageDateTicks).ToString("d hh:mm:ss") == msgTime);
                        SendedMessages.Remove(sendedmsg);

                        var msg = new MessagesModel
                        {
                            MessageId = res.MessageId,
                            MessageData = res.Data,
                            MessageType = (int)message.Item2,
                            MessageSenderId = res.Sender.UserId,
                            MessageDateTicks = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(res.CreatedAt.ToString())).ToLocalTime().Ticks
                        };

                        _messagesModel.Add(msg);

                        if (_chat.Messages.Last().MessageDateTime.ToString("d hh:mm:ss") != msgTime && sendingFailedMessages == false)
                        {
                            if (!_chat.Messages.Any(x => x.MessageId == res.MessageId))
                            {
                                MessagesLogic(new List<MessagesModel> { fakeMessage }, true);
                                _chat.Messages = _chatMessages;
                                RaisePropertyChanged(nameof(SendedMessages));
                            }
                        }
                    }

                    SaveLastMessage();

                    Debug.WriteLine("Send Message Successefully:" + SendedMessages?.Count);
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);

                if (_failedMessages?.Count == 0)
                    _dialogService.ShowAlert(SendMessageError, AlertType.Error, 4f);

                foreach (var messageId in SendedMessages) {
                    var msg = _chat.Messages.FindLast(x => x.MessageId == messageId.MessageId);

                    if (msg != null)
                    {
                        msg.FailedToSend = true;
                        _failedMessages.Add(msg);
                    }
                }

                if (!sendingFailedMessages)
                    RaisePropertyChanged(nameof(Chat));
            }
        }

        #region Messenger Sender Handler

        TaskCompletionSource<bool> _messengerReconnectHandler;

        private Task WaitForConnection()
        {
            _messengerReconnectHandler = new TaskCompletionSource<bool>();

            try
            {
               _messengerService.ConnectMessenger();

                //if (SendBirdClient.GetConnectionState() == SendBirdClient.ConnectionState.OPEN)
                    //_messengerReconnectHandler.TrySetResult(true);
            }
            catch (Exception ex) {}

            return _messengerReconnectHandler.Task;
        }

        private void MessengerReconnected()
        {
            _messengerReconnectHandler?.TrySetResult(true);
        }

        #endregion

        private async Task RetrySendMessages()
        {
            if (_isSendingFailedMessage == false)
            {
                _isSendingFailedMessage = true;
                IsBusy = true;

                try
                {
                    await _messengerService.ConnectMessenger();

                    if (SendBirdClient.GetConnectionState() == SendBirdClient.ConnectionState.OPEN)
                    {
                        SendedMessages = new List<MessagesModel>();

                        var failedMessageList = _failedMessages.ToList();

                        foreach (var msg in failedMessageList)
                        {
                            _failedMessages.Remove(msg);
                            _chat.Messages.Remove(msg);
                            await Task.Delay(200);
                            await SendMessage(new Tuple<string, MessageType>(msg.MessageData, msg.MessageType), true);
                        }

                        RaisePropertyChanged(nameof(Chat));
                    }
                }
                catch (Exception ex)
                {
                    Ui.Handle(ex as dynamic);
                }
                finally
                {
                    _isSendingFailedMessage = false;
                    IsLoading = false;
                    IsBusy = false;
                }
            }
        }

        private async Task FetchOldMessages()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet ||  _chat?.Messages?.Count < 30)
                return;

            IsBusy = true;

            await LoadRecentMessages(true);

            RaisePropertyChanged(nameof(ScrollToMiddle));

            IsBusy = false;
        }

        private async Task LoadMessagesAndUpdateReadReceipt(bool loadMessages, bool forceLoad = false)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsLoading = false;
                return;
            }

            IsBusy = true;

            try
            {
                bool needsToUpdatedMessages = false;

                if (SendBirdClient.GetConnectionState() != SendBirdClient.ConnectionState.OPEN)
                {
                    needsToUpdatedMessages = true;
                    await MessageServiceConnection();
                }

                if (!await GetChannel())
                {
                    IsBusy = false;
                    return;
                }

                _failedMessages = new List<ChatMessagesModel>();

                foreach (var failedMessage in _chat?.Messages?.Where(x => x.FailedToSend == true)?.ToList()) {
                    _failedMessages.Add(failedMessage);
                }

                if ((loadMessages && needsToUpdatedMessages) || forceLoad)
                    await LoadRecentMessages(false);

                _messengerService.MarkMessageAsRead(_channel);

                StatusLogic();

                foreach (var failmsg in _failedMessages) {
                    _chat?.Messages.Add(failmsg);
                }

                if (_failedMessages.Count > 0)
                {
                    RaisePropertyChanged(nameof(Chat));
                    IsLoading = true;
                    await Task.Delay(2000);
                    await RetrySendMessages();
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                IsLoading = false;
                IsBusy = false;
            }
        }

        private async Task<bool> GetChannel()
        {
            try
            {
                if (_channel != null)
                    return true;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    return false;
                    
                if(SendBirdClient.GetConnectionState() == SendBirdClient.ConnectionState.CLOSED || 
                   SendBirdClient.GetConnectionState() == SendBirdClient.ConnectionState.CLOSING)
                {
                    await Task.Delay(2000);
                    await _messengerService.ConnectMessenger();
                }

                Debug.WriteLine("Getting Channel");

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
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
                await CloseView();
                return false;
            }

            return true;
        }
        #region MessageServiceConnection

        private async Task<bool> MessageServiceConnection()
        {
            Debug.WriteLine("Checking Message Service Connection: " + SendBirdClient.GetConnectionState());

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                return false;

            IsBusy = true;

            var tcs = new TaskCompletionSource<bool>();

            try
            {
                if (SendBirdClient.GetConnectionState() != SendBirdClient.ConnectionState.OPEN)
                {
                    await _messengerService.ConnectMessenger();

                    if (!await GetChannel())
                        return false;
                }

                return true;
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

        #endregion

        #region MessageClickEvent & GetUserPushToken

        private async Task GetUserPushToken()
        {
            try
            {
                var result = await _contactService.GetUserPushToken(_userId);

                if (result?.StatusCode == 200)
                    Realm.Write(() => { _user.PushNotificationToken = result.NotificationToken; });
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private void MessageClickEvent(object sender, long messageId)
        {
            if (_failedMessages.Count > 0 && IsBusy == false)
            {
                IsLoading = true;
                RetrySendMessages();
            }
            else
                OpenChatImage(messageId);
        }

        private async Task OpenChatImage(long messageId)
        {
            try
            {
                var mensagem = _chatMessages?.Find(x => x.MessageId == messageId);

                if (mensagem?.MessageType == MessageType.Image)
                {
                    await _dialogService.ShowChatImage(mensagem.MessageData, SaveImage);
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }


        #endregion

        #region ChatHandlers

        public void ChatHandlers(bool enable = true)
        {
            if (enable)
            {
                SendBirdClient.ChannelHandler ch = new SendBirdClient.ChannelHandler
                {
                    OnMessageReceived = MessageReceived,
                    OnTypingStatusUpdated = TypingStatus,
                    OnReadReceiptUpdated = ReadReceipt
                };

                SendBirdClient.ConnectionHandler cch = new SendBirdClient.ConnectionHandler
                {
                    OnReconnectSucceeded = MessengerReconnected
                };

                SendBirdClient.AddChannelHandler("ChatHandler", ch);
                SendBirdClient.AddConnectionHandler("ChatConnectionHandler", cch);
            }
            else
            {
                SendBirdClient.RemoveChannelHandler("ChatHandler");
                SendBirdClient.RemoveConnectionHandler("ChatConnectionHandler");
            }
        }

        #endregion

        #region Status

        private void TypingStatus(GroupChannel channel)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (channel?.Url == _channel?.Url)
                {
                    _status = channel.IsTyping() ? TypingMessage : string.Empty;
                    StatusLogic();
                }
            });
        }

        private void TypingStatus(bool isTyping)
        {
            if (isTyping)
                _messengerService.TypingMessage(_channel);
            else
                _messengerService.TypingMessageEnded(_channel);
        }

        private void MessageReceived(BaseChannel baseChannel, BaseMessage baseMessage)
        {
            MainThread.BeginInvokeOnMainThread(() => {
            
                Debug.WriteLine("Message Received");

                if (_chat == null)
                    return;

                _chat.MemberPresence = MemberPresence.Online;

                var channel = baseChannel as GroupChannel;

                if (channel?.Url != _channel?.Url)
                    return;

                var newMessage = new MessagesModel();

                if (baseMessage is UserMessage msg)
                {
                    newMessage.MessageId = msg.MessageId;
                    newMessage.MessageType = 0;
                    newMessage.MessageData = msg.Message;
                    newMessage.MessageSenderId = msg.Sender.UserId;
                    newMessage.MessageDateTicks = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(msg.CreatedAt.ToString())).ToLocalTime().Ticks;
                }
                else if(baseMessage is FileMessage m)
                {
                    newMessage.MessageId = m.MessageId;
                    newMessage.MessageType = 1;
                    newMessage.MessageData = m.Url;
                    newMessage.MessageSenderId = m.Sender.UserId;
                    newMessage.MessageDateTicks = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(m.CreatedAt.ToString())).ToLocalTime().Ticks;
                }

                MessagesLogic(new List<MessagesModel> { newMessage }, true);

             
                _chat.Messages = _chatMessages;
                RaisePropertyChanged(nameof(Chat));
                _status = string.Empty;

                RaisePropertyChanged(nameof(Status));
                RaisePropertyChanged(nameof(NewMessageAlert));

                _audioService.PlayReceivedMessage();

                SaveLastMessage(newMessage);
            });
        }

        private void ReadReceipt(GroupChannel channel)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (_chat.MemberPresence != MemberPresence.Online)
                {
                    _chat.MemberPresence = MemberPresence.Online;
                    RaisePropertyChanged("Chat");
                }

                StatusLogic();
            });
        }

        private void StatusLogic()
        {
            if (_chat?.Messages?.Count > 0)
            {
                if (_channel == null)
                {
                    _status = _chat.Messages.Last().MessageSenderId == _finalUserId && _chat.MemberSeenMyLastMessage ? SeenMessage : string.Empty;
                }
                else if (_status != TypingMessage && _channel != null)
                {
                    if (_chat.Messages.Last().MessageSenderId == _finalUserId)
                    {
                        _status = _channel?.GetReadReceipt(_channel?.LastMessage) <= 0 ? SeenMessage : string.Empty;
                        _chat.MemberSeenMyLastMessage = !string.IsNullOrEmpty(_status);
                    }
                    else
                    {
                        _chat.MemberSeenMyLastMessage = false;
                        _status = string.Empty;
                    }
                }

                RaisePropertyChanged(nameof(Status));
            }
        }

        #endregion

        #region CheckConnection

        private async Task CheckConnection()
        {
            Connectivity.ConnectivityChanged -= ConnectivityChanged;
            Connectivity.ConnectivityChanged += ConnectivityChanged;
        }

        private void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                ConnectionOn();
            }
            else
            {
                _chat.MemberPresence = MemberPresence.Offline;
                RaisePropertyChanged(nameof(Chat));
                ChatHandlers(false);
            }
        }

        private async Task ConnectionOn()
        {
            if (IsBusy)
                return;
                
            await LoadMessagesAndUpdateReadReceipt(true);
            ChatHandlers();
        }

        #endregion

        #region NavigationBar

        private async Task Call()
        {
            try
            {
                var callType = await _dialogService.ShowContactOptions(LocationResources, _user.ShowNumber);

                switch (callType)
                {
                    case CallingType.Letter:
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                            await NavigationService.NavigateAsync<CallViewModel, Tuple<int, bool>>(new Tuple<int, bool>(_user.UserId, true));
                            
                        break;
                    case CallingType.Cellphone:
                        CallUtils.Call(_user.ContactNumber);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private async Task Options()
        {
            try
            {
                string[] resources = { SeeUserProfile, SendEmail, UserMuted, ArchiveChat, InAppNotifications, Close };
                var options = await _dialogService.ShowChatOptions(MemberName, _chat.MemberPhoto, _chat.MemberMuted, resources);

                _chat.MemberMuted = options.Item2;

                switch (options.Item1)
                {
                    case ChatOptions.SeeProfile:
                        await NavigationService.NavigateAsync<MemberViewModel, int>(_userId);
                        break;
                    case ChatOptions.SendEmail:
                        await EmailUtils.SendEmail(_chat.MemberEmail);
                        break;
                    case ChatOptions.ArchiveChat:
                        _chat.MemberArchived = true;
                        await CloseView();
                        break;
                }

            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        #endregion

        #region Close

        private async Task CloseView() => await NavigationService.Close(this);

        public override async Task Disappearing()
        {
            if (_isPickingImage)
                return;

            NavigationService.ChatOpen(-1);
            ChatHandlers(false);
            SaveChat();
        }

        private void SaveLastMessage(MessagesModel newMessage = null)
        {
            try
            {
                if (_userChat == null)
                {
                    SaveChat();
                    _userChat = Realm.Find<ChatListUserModel>(_userId);
                }

                if (_userChat != null)
                {
                    Realm.Write(() =>
                    {
                        if (newMessage == null)
                            newMessage = _messagesModel.Last();

                        _userChat?.MessagesList?.Add(newMessage);

                        if ((MessageType)newMessage.MessageType == MessageType.Text)
                            _userChat.LastMessage = newMessage.MessageSenderId == _finalUserId ? $"{YouChatLabel} {newMessage.MessageData}" : newMessage.MessageData;
                        else
                            _userChat.LastMessage = newMessage.MessageSenderId == _finalUserId ? $"{YouChatLabel} {SentImage}" : SentImage;
                    });
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private void SaveChat()
        {
            try
            {
                if (_chat?.Messages?.Count > 0)
                {
                    Realm?.Write(() =>
                    {
                        var lastMessage = _chat.Messages.Last();

                        var userChat = new ChatListUserModel
                        {
                            MemberId = _user.UserId,
                            MemberName = $"{_user.FirstName} {_user.LastName} - {_user.Position}",
                            MemberPhoto = _user.Picture,
                            IsMemberMuted = _chat.MemberMuted,
                            IsNewMessage = false,
                            MemberPresence = (int)_chat.MemberPresence,
                            MemberPresenceConnectionDate = _userChat != null ? _userChat.MemberPresenceConnectionDate : default(DateTime).Ticks,
                            LastMessageDateTimeTicks = lastMessage.MessageDateTime.Ticks,
                            IsArchived = _chat.MemberArchived,
                            ArchivedTime = _chat.MemberMuted ? DateTime.Now.Ticks : default(DateTime).Ticks,
                            UnreadMessagesCount = 0,
                            MemberSeenMyLastMessage = _chat.MemberSeenMyLastMessage,
                        };

                        if (lastMessage.MessageType == MessageType.Text)
                            userChat.LastMessage = _chat.Messages.Last().MessageSenderId == _finalUserId ? $"{YouChatLabel} {lastMessage.MessageData}" : lastMessage.MessageData;
                        else
                            userChat.LastMessage = _chat.Messages.Last().MessageSenderId == _finalUserId ? $"{YouChatLabel} {SentImage}" : SentImage;

                        List<MessagesModel> historyMessages;

                        if (_messagesModel?.Count > 0)
                            historyMessages = _messagesModel?.OrderByDescending(x => x.MessageDateTicks).Take(30).ToList();
                        else
                            historyMessages = _userChat?.MessagesList?.OrderByDescending(x => x.MessageDateTicks).Take(30).ToList();

                        foreach (var msg in historyMessages.ToList())
                            userChat.MessagesList.Add(msg);

                        Realm?.Add(userChat, true);
                    });
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        #endregion

        #region CanExecute

        private bool CanExecute() => !IsBusy;
        private bool CanExecuteMsg(Tuple<string, MessageType> msg) => !IsBusy;
        private bool CanExecuteLoad(bool arg) => !IsBusy;

        #endregion

        #region Resources 

        private string YouChatLabel => L10N.Localize("ChatList_You");
        private string UserNotRegistered => L10N.Localize("ChatList_UserNotRegistered");
        private string SendMessageError => L10N.Localize("ChatList_MessageError");
        private string UserNotFound => L10N.Localize("ChatList_UserNotFound");
        private string SaveImage => L10N.Localize("Chat_SaveImage");

        public string TypeSomething => L10N.Localize("OnBoardingViewModel_LetterSlogan");
        public string SendingMessage => L10N.Localize("Chat_SendingMessage");
        public string TypingMessage => L10N.Localize("Chat_TypingMessage");
        public string SeenMessage => L10N.Localize("Chat_SeenMessage");
        public string SendMessageButton => L10N.Localize("Chat_SendMessage");
        public string CancelButton => L10N.Localize("Cancel");

        private string SeeUserProfile => L10N.Localize("Chat_UserProfile");
        private string UserMuted => L10N.Localize("Chat_UserMuted");
        private string ArchiveChat => L10N.Localize("Chat_Archive");
        private string SendEmail => L10N.Localize("Division_SendEmail");
        private string Close => L10N.Localize("Chat_Close");
        private string InAppNotifications => L10N.Localize("Chat_MuteInApp");
        private string SentImage => L10N.Localize("ChatList_SentImage");

        private Dictionary<string, string> LocationResources = new Dictionary<string, string>();
        private string TitleDialog => L10N.Localize("ContactDialog_Title");
        private string LetterDialog => L10N.Localize("ContactDialog_TitleLetter");
        private string LetterDescriptionDialog => L10N.Localize("ContactDialog_DescriptionLetter");
        private string PhoneDialog => L10N.Localize("ContactDialog_TitlePhone");
        private string PhoneDescriptionDialog => L10N.Localize("ContactDialog_DescriptionPhone");

        private void SetL10NResources()
        {
            LocationResources.Add("Title", TitleDialog);
            LocationResources.Add("TitleLetter", LetterDialog);
            LocationResources.Add("DescriptionLetter", LetterDescriptionDialog);
            LocationResources.Add("TitlePhone", PhoneDialog);
            LocationResources.Add("DescriptionPhone", PhoneDescriptionDialog);
        }

        #endregion
    }
}
