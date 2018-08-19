using System;
using System.Diagnostics;
using CoreFoundation;
using Foundation;
using LetterApp.Core;
using LetterApp.iOS.CallKit;
using LetterApp.iOS.Views.Base;
using PushKit;
using SinchSdk;
using UIKit;

namespace LetterApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate, ISINClientDelegate, ISINCallClientDelegate, ISINManagedPushDelegate, ISINCallDelegate
    {
        // class-level declarations
        public override UIWindow Window { get; set; }
        public UINavigationController NavigationController;
        public RootViewController RootController;

        public static NSData DeviceToken;
        public ActiveCallManager CallManager;
        public ProviderDelegate CallProviderDelegate { get; set; }

        public ISINClient Client { get; set; }
        public ISINManagedPush Push { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            CallManager = new ActiveCallManager();
            CallProviderDelegate = new ProviderDelegate(CallManager);

            NavigationController = new UINavigationController();
            RootController = new RootViewController();
            NavigationController.PushViewController(RootController, true);

            Window.RootViewController = NavigationController;
            Window.MakeKeyAndVisible();
            Setup.Initialize();

            Push = SinchSdk.Sinch.ManagedPushWithAPSEnvironment(SINAPSEnvironment.Development);
            Push.WeakDelegate = this;
            Push.SetDesiredPushTypeAutomatically();
            Push.RegisterUserNotificationSettings();

            RegisterRemotePushNotifications(application);

            NSNotificationCenter.DefaultCenter.AddObserver("UserDidLoginNotification", null, null, (obj) => {
                InitSinchClientWithUserId(obj.UserInfo["userId"].ToString());
            });

            if(AppSettings.UserId != 0)
                InitSinchClientWithUserId(AppSettings.UserId.ToString());

            return true;
        }

        static void RegisterRemotePushNotifications(UIApplication app)
        {
            UIUserNotificationType notificationType = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
            var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(notificationType, new NSSet());
            app.RegisterUserNotificationSettings(pushSettings);
            app.RegisterForRemoteNotifications();
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            DeviceToken = deviceToken;
            Debug.WriteLine("Registered For Remote Notifications with Token " + deviceToken?.Description);
            byte[] deviceTokenBytes = deviceToken.ToArray();
        }

        private void InitSinchClientWithUserId(string userId)
        {
            if (Client == null)
            {
                Client = SinchSdk.Sinch.ClientWithApplicationKey("b56256a4-f651-4b4a-a602-69e350b9010e", "1OnpOBkW0k6KL3zAgAaWtA==", "clientapi.sinch.com", userId);
                Client.WeakDelegate = this;
                Client.SetSupportCalling(true);
                Client.EnableManagedPushNotifications();
                Client.SetSupportPushNotifications(true);
                Client.Start();
                Client.StartListeningOnActiveConnection();
            }
        }

        public void DidReceiveIncomingPushWithPayload(ISINManagedPush managedPush, NSDictionary payload, string pushType)
        {

            if (pushType == "PKPushTypeVoIP" && AppSettings.CallNotifications)
            {
                var callInfo = payload["sin"].ToString();
                var caller = Client.RelayRemotePushNotificationPayload(callInfo);

                CallProviderDelegate.ReportIncomingCall(new NSUuid(), caller.CallResult.RemoteUserId);
            }
        }

        //Update Credentials
        public void DidUpdatePushCredentials(PKPushRegistry registry, PKPushCredentials credentials, string type)
        {
            Client.RegisterPushNotificationData(credentials.Token);
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

        [Export("callDidEnd:")]
        void CallDidEnd(ISINCall xcall)
        {
        }

        public override void OnResignActivation(UIApplication application){}
        public override void DidEnterBackground(UIApplication application){}
        public override void WillEnterForeground(UIApplication application){}
        public override void OnActivated(UIApplication application){}
        public override void WillTerminate(UIApplication application){}
    }
}

