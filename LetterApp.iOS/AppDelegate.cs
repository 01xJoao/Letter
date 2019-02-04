using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AVFoundation;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.CallKit;
using LetterApp.iOS.Views.Base;
using LetterApp.Models.DTO.ReceivedModels;
using PushKit;
using SinchBinding;
using UIKit;
using UserNotifications;

namespace LetterApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate, ISINClientDelegate, ISINCallClientDelegate, ISINManagedPushDelegate, ISINCallDelegate, IUNUserNotificationCenterDelegate
    {
        // class-level declarations
        public override UIWindow Window { get; set; }
        public UINavigationController NavigationController;
        public RootViewController RootController;

        //public static string DeviceToken;
        public ActiveCallManager CallManager { get; set; }
        public ProviderDelegate CallProviderDelegate { get; set; }
        private ActiveCall _call;
        private bool _inbackGround;

        private UNUserNotificationCenter notificationCenter;

        private ISINCall _sinCall;
        public ISINCall SINCall
        {
            get => _sinCall;
            set
            {
                _sinCall = value;
                _sinCall.WeakDelegate = this;
            }
        }

        public ISINClient Client { get; set; }
        public ISINManagedPush Push { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            CallManager = new ActiveCallManager();
            CallProviderDelegate = new ProviderDelegate(CallManager);

            SendBird.SendBirdClient.Init("46497603-C6C5-4E64-9E05-DCCAF5ED66D1");
            OneSignal.Current.StartInit("0c3dc7b8-fabf-4449-ab16-e07d2091eb47").InFocusDisplaying(OSInFocusDisplayOption.None).EndInit();

            Push = Sinch.ManagedPushWithAPSEnvironment(SINAPSEnvironment.Production);
            Push.WeakDelegate = this;
            Push.SetDesiredPushTypeAutomatically();

            if (!string.IsNullOrEmpty(AppSettings.UserAndOrganizationIds))
            {
                Push.RegisterUserNotificationSettings();
                InitSinchClientWithUserId(AppSettings.UserAndOrganizationIds);
            }

            NavigationController = new UINavigationController();
            RootController = new RootViewController();
            NavigationController.PushViewController(RootController, true);

            Window.RootViewController = NavigationController;
            Window.MakeKeyAndVisible();
            Setup.Initialize();

            OneSignal.Current.RegisterForPushNotifications();
            OneSignal.Current.IdsAvailable(HandleIdsAvailableCallback);

            using (var audio = AVAudioSession.SharedInstance())
            {
                if (audio.RecordPermission != AVAudioSessionRecordPermission.Granted)
                {
                    audio.RequestRecordPermission((granted) => {
                        RegisterRemotePushNotifications(application);
                    });
                }
                else
                {
                    RegisterRemotePushNotifications(application);
                }
            }

            NSNotificationCenter.DefaultCenter.AddObserver("UserDidLoginNotification", null, null, (obj) =>
            {
                Push.RegisterUserNotificationSettings();
                InitSinchClientWithUserId(obj.UserInfo["userId"].ToString());
            });


            notificationCenter = UNUserNotificationCenter.Current;
            notificationCenter.Delegate = this;
            notificationCenter.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,(bool granted, NSError arg2) => {
            });

            application.RegisterForRemoteNotifications();

            return true;
        }

        [Export("application:continueUserActivity:restorationHandler:")]
        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            if (RootController?.CurrentViewController is MainViewController)
            {
                var viewC = RootController.CurrentViewController as MainViewController;
                viewC.SetVisibleView(1);
            }
            return true;
        }

        void RegisterRemotePushNotifications(UIApplication app)
        {
            InvokeOnMainThread(() =>
            {
                UIUserNotificationType notificationType = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(notificationType, new NSSet());
                app.RegisterUserNotificationSettings(pushSettings);
                app.RegisterForRemoteNotifications();
            });
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //Debug.WriteLine("Registered For Remote Notifications with Token " + deviceToken?.Description);
            //byte[] deviceTokenBytes = deviceToken.ToArray();
        }

        void HandleIdsAvailableCallback(string playerID, string pushToken) => AppSettings.MessengerToken = playerID;

        //[Export("application:didReceiveRemoteNotification:fetchCompletionHandler:")]
        //public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        //{
        //    if(application.ApplicationState == UIApplicationState.Active)
        //    {
        //        var info = userInfo["custom"] as NSDictionary;
        //        var info1 = info["a"] as NSDictionary;
        //        var userId = info1["userId"].ToString();
        //        bool result = int.TryParse(userId, out int user);
        //        var navigationService = App.Container.GetInstance<IXNavigationService>();

        //        if (result && navigationService.ChatOpen() != user)
        //        {
        //            navigationService.PopToRoot(false);
        //            navigationService.NavigateAsync<ChatViewModel, int>(user);
        //        }
        //    }
        //}

        
        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            if (_inbackGround)
            {
                var info = response.Notification.Request.Content.UserInfo["custom"] as NSDictionary;
                var info1 = info["a"] as NSDictionary;
                var userId = info1["userId"].ToString();
                bool result = int.TryParse(userId, out int user);
                var navigationService = App.Container.GetInstance<IXNavigationService>();

                if (result && navigationService.ChatOpen() != user)
                {
                    navigationService.PopToRoot(false);
                    navigationService.NavigateAsync<ChatViewModel, int>(user);
                }
            }

            completionHandler?.Invoke();
        }

        private void InitSinchClientWithUserId(string userId)
        {
            Client = Sinch.ClientWithApplicationKey("b56256a4-f651-4b4a-a602-69e350b9010e", "1OnpOBkW0k6KL3zAgAaWtA==", "clientapi.sinch.com", userId);
            Client.WeakDelegate = this;
            Client.CallClient.WeakDelegate = this;

            Client.SetSupportCalling(true);
            //Client.SetSupportPushNotifications(true);
            Client.EnableManagedPushNotifications();

            Client.Start();
            //Client.StartListeningOnActiveConnection();
            //CallProviderDelegate.StartCallClientDelegate();
        }

        public void DidReceiveIncomingPushWithPayload(ISINManagedPush managedPush, NSDictionary payload, string pushType)
        {
            if (pushType == "PKPushTypeVoIP" && AppSettings.CallNotifications)
            {
                var aps = payload["aps"] as NSDictionary;
                var alert = aps["alert"] as NSDictionary;
                var callType = alert["loc-key"].ToString();
                var callInfo = payload["sin"].ToString();

                if (Client == null)
                    InitSinchClientWithUserId(AppSettings.UserAndOrganizationIds);

                Client.RelayRemotePushNotificationPayload(callInfo);
            }
        }

        public void DidUpdatePushCredentials(PKPushRegistry registry, PKPushCredentials credentials, string type)
        {
            UnregisterTokens();
            Client.RegisterPushNotificationData(credentials.Token);
        }

        public void UnregisterTokens()
        {
            Client?.UnregisterPushNotificationDeviceToken();
            Client?.UnregisterPushNotificationData();
        }

        public void ClientDidStart(ISINClient client)
        {
            Debug.WriteLine($"Sinch client started successfully)");
        }

        public void ClientDidFail(ISINClient client, NSError error)
        {
            Debug.WriteLine($"Sinch client error: {error.LocalizedDescription}");
        }

        [Export("client:logMessage:area:severity:timestamp:")]
        void client(ISINClient client, string message, string area, SINLogSeverity severity, NSDate timestamp)
        {
            Debug.WriteLine(message);
        }

        public override void OnResignActivation(UIApplication application) 
        {
            Debug.WriteLine("DID ENTER OnResign:" + application.ApplicationState);
        }
        public override void DidEnterBackground(UIApplication application) 
        {
            _inbackGround = true;

            CleanNotifications();
        }

        public override void WillEnterForeground(UIApplication application) 
        {
            CleanNotifications();
        }
        public override void OnActivated(UIApplication application) 
        {
            CleanNotifications();
        }

        public override void WillTerminate(UIApplication application) 
        {
            CleanNotifications();
        }

        private void CleanNotifications()
        {
            using (var center = UNUserNotificationCenter.Current)
            {
                center.RemoveAllDeliveredNotifications();
                center.RemoveAllPendingNotificationRequests();
            }

            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        #region sinch


        [Export("client:willReceiveIncomingCall:")]
        public void WillReceiveIncomingCall(ISINCallClient client, ISINCall call)
        {
            SINCall = call;
            _call = CallProviderDelegate.ReportIncomingCall(new NSUuid(), call);
        }

        [Export("client:didReceiveIncomingCall:")]
        public void DidReceiveIncomingCall(ISINCallClient client, ISINCall call)
        {
            if (_call == null)
            {
                SINCall = call;
                _call = CallProviderDelegate.ReportIncomingCall(new NSUuid(), call);
            }
        }

        [Export("callDidEnd:")]
        public void CallDidEnd(ISINCall call)
        {
            var xcall = CallManager.Calls.LastOrDefault();

            if (xcall != null)
            {
                xcall.EndCall();

                if (!xcall.IsConnected)
                {
                    CallManager.EndCall(xcall);

                    if (!xcall.IsOutgoing)
                    {
                        AppSettings.BadgeForCalls++;

                        using (var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate)
                        {
                            if (appDelegate.RootController?.CurrentViewController is MainViewController)
                            {
                                var view = appDelegate.RootController.CurrentViewController as MainViewController;
                                if (view.TabBar.Items.Any())
                                    view.TabBar.Items[1].BadgeValue = AppSettings.BadgeForCalls.ToString();
                            }
                        }
                    }
                }
            }
            _call = null;
        }

        #endregion
    }
}

