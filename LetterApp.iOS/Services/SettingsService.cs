﻿using System;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using LetterApp.Core.Services.Interfaces;
using UIKit;
using UserNotifications;

namespace LetterApp.iOS.Services
{
    public class SettingsService : ISettingsService
    {
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
            using (var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate)
            {
                appDelegate.UnregisterTokens();
            }
        }
    }
}