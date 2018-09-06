using System;
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
    }
}
