using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core;
using LetterApp.Core.Services.Interfaces;
using SendBird;
using Xamarin.Essentials;

namespace LetterApp.iOS.Services
{
    public class MessengerService : IMessengerService
    {
        const string _handlerId = "SendBirdHandler";

        public void InitializeMessenger()
        {
            SendBirdClient.Init("46497603-C6C5-4E64-9E05-DCCAF5ED66D1");
        }

        public Task<Tuple<BaseChannel, BaseMessage>> InitializeHandlers()
        {
            var tcs = new TaskCompletionSource<Tuple<BaseChannel, BaseMessage>>();

            SendBirdClient.ChannelHandler ch = new SendBirdClient.ChannelHandler
            {
                OnMessageReceived = (BaseChannel baseChannel, BaseMessage baseMessage) =>
                {
                    tcs.TrySetResult(new Tuple<BaseChannel, BaseMessage>(baseChannel, baseMessage));
                }

                //OnReadReceiptUpdated = (GroupChannel groupChannel) =>
                //{
                //},

                //OnTypingStatusUpdated = (GroupChannel groupChannel) =>
                //{
                //}
            };

            SendBirdClient.AddChannelHandler(_handlerId, ch);

            return tcs.Task;
        }

        public void ConnectMessenger()
        {
            SendBirdClient.Connect(AppSettings.UserAndOrganizationIds, (User user, SendBirdException e) =>
            {
                if (e != null)
                    return;

                RegisterMessengerToken();
            });
        }

        public void DisconnectMessenger()
        {
            SendBirdClient.Disconnect(null);
        }

        public void RegisterMessengerToken()
        {
            SendBirdClient.RegisterAPNSPushTokenForCurrentUser(AppSettings.UserAndOrganizationIds, 
                                       (SendBirdClient.PushTokenRegistrationStatus status, SendBirdException e) =>
            {
                if (e != null)
                    return;

                if (status == SendBirdClient.PushTokenRegistrationStatus.PENDING)
                    CheckConnection();
            });
        }

        private void CheckConnection()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                RegisterMessengerToken();
                Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            }
        }

        public Task<GroupChannel> CreateChannel(List<string> users)
        {
            var tcs = new TaskCompletionSource<GroupChannel>();

            GroupChannel.CreateChannelWithUserIds(users, true, (channel, e) =>
            {
                if (e != null)
                    tcs.TrySetCanceled();
                else
                {
                    channel.SetPushPreference(true, (error) => { });
                    tcs.TrySetResult(channel);
                }
            });

            return tcs.Task;
        }

        public Task<List<GroupChannel>> GetAllChannels()
        {
            var tcs = new TaskCompletionSource<List<GroupChannel>>();

            GroupChannelListQuery mQuery = GroupChannel.CreateMyGroupChannelListQuery();
            mQuery.IncludeEmpty = false;
            mQuery.Next((List<GroupChannel> list, SendBirdException e) =>
            {
                if (e != null)
                    tcs.TrySetCanceled();

                tcs.TrySetResult(list);
            });

            return tcs.Task;
        }

        public Task<List<GroupChannel>> GetChannels(List<string> users)
        {
            var tcs = new TaskCompletionSource<List<GroupChannel>>();

            GroupChannelListQuery filteredQuery = GroupChannel.CreateMyGroupChannelListQuery();
            filteredQuery.IncludeEmpty = false;
            filteredQuery.SetUserIdsIncludeFilter(users, GroupChannelListQuery.QueryType.OR);
            filteredQuery.Next((List<GroupChannel> queryResult, SendBirdException e) =>
            {
                if (e != null)
                    tcs.TrySetCanceled();

                tcs.TrySetResult(queryResult);
            });

            return tcs.Task;
        }

        public Task<GroupChannel> GetCurrentChannel(string userId)
        {
            var tcs = new TaskCompletionSource<GroupChannel>();

            GroupChannelListQuery filteredQuery = GroupChannel.CreateMyGroupChannelListQuery();
            filteredQuery.SetUserIdsExactFilter(new List<string> { userId });
            filteredQuery.Next((List<GroupChannel> queryResult, SendBirdException e) =>
            {
                if (e != null)
                    tcs.TrySetCanceled();

                tcs.TrySetResult(queryResult.FirstOrDefault());
            });

            return tcs.Task;
        }

        public Task<List<User>> CheckUsersInGroupPresence(GroupChannel channel)
        {
            var tcs = new TaskCompletionSource<List<User>>();

            channel.Refresh((e) =>
            {
                if (e != null)
                    tcs.TrySetCanceled();

                tcs.TrySetResult(channel.Members);
            });

            return tcs.Task;
        }

        public Task<UserMessage> SendMessage(GroupChannel channel, string message)
        {
            var tcs = new TaskCompletionSource<UserMessage>();

            channel.SendUserMessage(message, (msg, e) =>
            {
                if (e != null)
                    tcs.TrySetCanceled();

                tcs.TrySetResult(msg);
            });

            return tcs.Task;
        }

        public void TypingMessage(GroupChannel channel)
        {
            channel.StartTyping();
        }

        public void TypingMessageEnded(GroupChannel channel)
        {
            channel.EndTyping();
        }

        public void MarkMessageAsRead(GroupChannel channel)
        {
            channel.MarkAsRead();
        }
    }
}
