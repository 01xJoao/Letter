using System.Diagnostics;
using System.Linq;
using AVFoundation;
using Foundation;
using LetterApp.Core;
using LetterApp.iOS.CallKit;
using LetterApp.iOS.Views.Base;
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

        public static string DeviceToken;
        public ActiveCallManager CallManager { get; set; }
        public ProviderDelegate CallProviderDelegate { get; set; }
        private ActiveCall _call;

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

            Push = Sinch.ManagedPushWithAPSEnvironment(SINAPSEnvironment.Development);
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

            using (var audio = AVAudioSession.SharedInstance())
            {
                if (audio.RecordPermission != AVAudioSessionRecordPermission.Granted)
                {
                    audio.RequestRecordPermission((granted) =>
                    {
                        //Push.RegisterUserNotificationSettings();
                        RegisterRemotePushNotifications(application);
                    });
                }
                else
                {
                    //Push.RegisterUserNotificationSettings();
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

                if(granted)
                {
                    Debug.WriteLine("NOTIFICATIONS GRANTED!!!!");
                }
                else
                {
                    Debug.WriteLine("NOTIFICATIONS NOT GRANTED---!!---");
                }
            });

            application.RegisterForRemoteNotifications();

            return true;
        }

        //TODO This might need to be changed when implementing chat
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
            DeviceToken = deviceToken.Description.TrimStart('<').TrimEnd('>');

            //Debug.WriteLine("Registered For Remote Notifications with Token " + deviceToken?.Description);
            //byte[] deviceTokenBytes = deviceToken.ToArray();
        }

        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, System.Action completionHandler)
        {

        }

        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, System.Action<UNNotificationPresentationOptions> completionHandler)
        {

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

                //if (callType == "SIN_INCOMING_CALL")
                //{
                //    var callInfo = payload["sin"].ToString();
                //    var caller = Client.RelayRemotePushNotificationPayload(callInfo);
                //    //CallProviderDelegate.ReportIncomingCall(new NSUuid(), caller.CallResult);
                //}
            }
        }

        [Export("application:didReceiveRemoteNotification:fetchCompletionHandler:")]
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, System.Action<UIBackgroundFetchResult> completionHandler)
        {
            throw new System.NotImplementedException();
        }

        [Export("application:didReceiveRemoteNotification:")]
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            throw new System.NotImplementedException();
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

        public override void OnResignActivation(UIApplication application) { }
        public override void DidEnterBackground(UIApplication application) { }
        public override void WillEnterForeground(UIApplication application) { }
        public override void OnActivated(UIApplication application) { }
        public override void WillTerminate(UIApplication application) { }

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

