using System;
using System.Threading.Tasks;
using AVFoundation;
using Com.OneSignal;
using Foundation;
using LetterApp.Core.Helpers;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;
using UserNotifications;

namespace LetterApp.iOS.Services
{
    public class SettingsService : ISettingsService
    {
        private IWebService _webService;
        public SettingsService(IWebService webService)
        {
            _webService = webService;
        }

        public bool CheckMicrophonePermissions()
        {
            return AVAudioSession.SharedInstance().RecordPermission == AVAudioSessionRecordPermission.Granted;
        }

        public Task<bool> CheckNotificationPermissions()
        {
            var tcs = new TaskCompletionSource<bool>();

            try
            {
                UNUserNotificationCenter.Current.GetNotificationSettings((obj) => {
                    tcs.TrySetResult(obj.AuthorizationStatus == UNAuthorizationStatus.Authorized);
                });
            }
            catch (Exception ex) 
            {
                //tcs.TrySetResult(false);
            }

            return tcs.Task;
        }

        public void OpenSettings()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
        }

        public void Logout()
        {
            SendBird.SendBirdClient.UnregisterPushTokenAllForCurrentUser((SendBird.SendBirdException e) => {
                SendBird.SendBirdClient.Disconnect(null);
            });

            RealmUtils.CleanContactsAndCalls();

            using (var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate)
            {
                appDelegate.UnregisterTokens();
            }

            OneSignal.Current.SetSubscription(false);
        }

        public async Task<BaseModel> SendPushNotificationToken(string token)
        {
            return await _webService.GetAsync<BaseModel>($"/api/users/registertoken/{token}", needsHeaderCheck: true).ConfigureAwait(true);
        }
    }
}
