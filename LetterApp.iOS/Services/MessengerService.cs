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
            SendBirdClient.ChannelHandler ch = new SendBirdClient.ChannelHandler();
            SendBirdClient.ConnectionHandler cnh = new SendBirdClient.ConnectionHandler();

            //Handlers

            SendBirdClient.Init("46497603-C6C5-4E64-9E05-DCCAF5ED66D1");
            SendBirdClient.AddChannelHandler(_handlerId, ch);
            SendBirdClient.AddConnectionHandler(_handlerId, cnh);

            //TODO Remove handlers?
        }

        public void ConnectMessenger()
        {
            SendBirdClient.Connect($"{AppSettings.UserId}-{AppSettings.OrganizationId}", (User user, SendBirdException e) => {
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
            SendBirdClient.RegisterAPNSPushTokenForCurrentUser(AppDelegate.DeviceToken, (SendBirdClient.PushTokenRegistrationStatus status, SendBirdException e) => {
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

            GroupChannel.CreateChannelWithUserIds(users, true, (channel, e) => {
                if (e != null)
                    tcs.TrySetCanceled();

                tcs.TrySetResult(channel);
            });

            return tcs.Task;
        }

        public Task<List<GroupChannel>> GetAllChannels()
        {
            var tcs = new TaskCompletionSource<List<GroupChannel>>();

            GroupChannelListQuery mQuery = GroupChannel.CreateMyGroupChannelListQuery();

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

        public Task<UserMessage> SendMessage(Tuple<GroupChannel,string> data)
        {
            var tcs = new TaskCompletionSource<UserMessage>();

            var channel = data.Item1;
            var message = data.Item2;

            channel.SendUserMessage(message, (msg, e) =>
            {
                if (e != null)
                    tcs.TrySetCanceled();
                   
                tcs.TrySetResult(msg);
            });

            return tcs.Task;
        }
    }
}
