using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public void InitializeMessenger()
        {
            SendBirdClient.Init("46497603-C6C5-4E64-9E05-DCCAF5ED66D1");

            Debug.WriteLine("SendBirdClient Initialized");
        }

        public Task<bool> ConnectMessenger()
        {
            var tcs = new TaskCompletionSource<bool>();

            try
            {
                if (SendBirdClient.GetConnectionState() == SendBirdClient.ConnectionState.OPEN)
                {
                    tcs.TrySetResult(true);
                }
                else if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    Debug.WriteLine("Connecting to SendBird...");

                    SendBirdClient.Connect(AppSettings.UserAndOrganizationIds, (User user, SendBirdException e) =>
                    {
                        if (e != null)
                            tcs.TrySetCanceled();
                        else
                        {
                            Debug.WriteLine("Connected to Sendbird Services");
                            tcs.TrySetResult(true);

                            RegisterMessengerToken();
                        }
                    });
                }
                else
                {
                    tcs.TrySetResult(false);
                }
            }
            catch (Exception)
            {
                tcs.TrySetCanceled();
            }

            return tcs.Task;
        }

        public void DisconnectMessenger()
        {
            SendBirdClient.Disconnect(null);
        }

        public void RegisterMessengerToken()
        {
            Debug.WriteLine("Registering Token");

            SendBirdClient.RegisterAPNSPushTokenForCurrentUser(AppSettings.UserAndOrganizationIds, (SendBirdClient.PushTokenRegistrationStatus status, SendBirdException e) => {
                if (e != null)
                    return;
            });

            Debug.WriteLine("Token Registered");
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

            if (mQuery == null)
            {
                tcs.TrySetCanceled();
                return tcs.Task;
            }

            mQuery.IncludeEmpty = false;
            mQuery.Order = GroupChannelListQuery.ChannelListOrder.LATEST_LAST_MESSAGE;
            mQuery.Next((List<GroupChannel> list, SendBirdException e) =>
            {
                if (e != null)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(list);
            });

            Debug.WriteLine("Getting Sendbird ALL Channels");

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
                else
                    tcs.TrySetResult(queryResult);
            });

            Debug.WriteLine("Getting Channels with users");

            return tcs.Task;
        }

        public Task<GroupChannel> GetCurrentChannel(string userId)
        {
            var tcs = new TaskCompletionSource<GroupChannel>();

            GroupChannelListQuery filteredQuery = GroupChannel.CreateMyGroupChannelListQuery();

            if(filteredQuery == null)
            {
                tcs.TrySetCanceled();
                return tcs.Task;
            }

            filteredQuery.SetUserIdsExactFilter(new List<string> { userId });
            filteredQuery.Next((List<GroupChannel> queryResult, SendBirdException e) =>
            {
                if (e != null)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(queryResult.FirstOrDefault());
            });

            return tcs.Task;
        }

        public Task<GroupChannel> GetUsersInChannel(string channelUrl)
        {
            var tcs = new TaskCompletionSource<GroupChannel>();

            GroupChannel.GetChannel(channelUrl, (channel, e) => 
            { 
                if (e != null)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(channel);
            });

            Debug.WriteLine("Getting users in Channels");

            return tcs.Task;
        }

        public Task<List<User>> CheckUsersInGroupPresence(GroupChannel channel)
        {
            var tcs = new TaskCompletionSource<List<User>>();

            if (channel == null)
            {
                tcs.TrySetCanceled();
                return tcs.Task;
            }

            channel.Refresh((e) =>
            {
                if (e != null)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(channel.Members);
            });

            Debug.WriteLine("UpdateUsers Presence");

            return tcs.Task;
        }

        public Task<UserMessage> SendMessage(GroupChannel channel, string message, string dateTime)
        {
            var tcs = new TaskCompletionSource<UserMessage>();

            if (channel == null)
            {
                tcs.TrySetCanceled();
                return tcs.Task;
            }

            channel.SendUserMessage(message, dateTime, (msg, e) =>
            {
                if (e != null)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(msg);
            });

            return tcs.Task;
        }

        public Task<List<BaseMessage>> LoadMessages(PreviousMessageListQuery channelQuery, int messagesCount)
        {
            var tcs = new TaskCompletionSource<List<BaseMessage>>();

            if (channelQuery == null)
            {
                tcs.TrySetCanceled();
                return tcs.Task;
            }

            channelQuery.Load(messagesCount, true, (List<BaseMessage> queryResult, SendBirdException e) => {
                if (e != null)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(queryResult);
            });

            return tcs.Task;
        }

        public void TypingMessage(GroupChannel channel)
        {
            if (channel != null)
                channel.StartTyping();
        }

        public void TypingMessageEnded(GroupChannel channel)
        {
            if (channel != null)
                channel.EndTyping();
        }

        public void MarkMessageAsRead(GroupChannel channel)
        {
            if (channel != null)
                channel.MarkAsRead();
        }

        public void RemoveChannel(GroupChannel channel)
        {
            if(channel != null)
                channel.Leave((e) => {});
        }
    }
}
