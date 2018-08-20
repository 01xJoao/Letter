using System;
using System.IO;
using System.Linq;
using AVFoundation;
using CallKit;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.Helpers;
using SinchSdk;
using UIKit;

namespace LetterApp.iOS.CallKit
{
    public class ProviderDelegate : CXProviderDelegate, ISINCallClientDelegate, ISINCallDelegate
    {
        #region Computed Properties
        public ActiveCallManager CallManager { get; set; }
        public CXProviderConfiguration Configuration { get; set; }
        public CXProvider Provider { get; set; }
        #endregion

        private ISINCall Call { get; set; }

        private ISINAudioController AudioController
        {
            get
            {
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.WeakDelegate;

                return appDelegate.Client.AudioController;
            }
        }

        private ISINClient Client
        {
            get
            {
                var appDelgate = (AppDelegate)UIApplication.SharedApplication.WeakDelegate;
                return appDelgate.Client;
            }
        }

        #region Constructors

        public ProviderDelegate(ActiveCallManager callManager)
        {
            CallManager = callManager;

            var handleTypes = new[] { (NSNumber)(int)CXHandleType.Generic };

            var maskImage = UIImage.FromBundle("letter_logo");

            Configuration = new CXProviderConfiguration("Letter")
            {
                SupportsVideo = false,
                MaximumCallsPerCallGroup = 1,
                SupportedHandleTypes = new NSSet<NSNumber>(handleTypes),
                IconTemplateImageData = maskImage.AsPNG(),
                RingtoneSound = "Audio/iphone_call.mp3"
            };

            Provider = new CXProvider(Configuration);
            Provider.SetDelegate(this, null);
        }

        #endregion

        #region Override Methods

        public override void DidReset(CXProvider provider)
        {
            CallManager.Calls.Clear();
        }

        public override void PerformStartCallAction(CXProvider provider, CXStartCallAction action)
        {
            var activeCall = CallManager.FindCall(action.CallUuid);

            // Monitor state changes
            activeCall.StartingConnectionChanged += (call) =>
            {
                if (call.IsConnecting)
                {
                    // Inform system that the call is starting
                    Provider.ReportConnectingOutgoingCall(call.UUID, (NSDate)call.StartedConnectingOn);
                }
            };

            activeCall.ConnectedChanged += (call) =>
            {
                if (call.IsConnected)
                {
                    // Inform system that the call has connected
                    provider.ReportConnectedOutgoingCall(call.UUID, (NSDate)call.ConnectedOn);
                }
            };

            activeCall.StartCall();
            AudioController.StartPlayingSoundFile(PathForSound("ringback.wav"), true);
        }

        public override void PerformAnswerCallAction(CXProvider provider, CXAnswerCallAction action)
        {
            var call = CallManager.FindCall(action.CallUuid);

            AudioController.StopPlayingSoundFile();

            if (call == null)
            {
                action.Fail();
                return;
            }
            else
            {
                if (!call.IsOutgoing)
                    App.StartCall(call.CallerId);
            }

            call.AnswerCall((successful) =>
            {
                if (successful)
                    action.Fulfill();
                else
                    action.Fail();
            });
        }

        public override void PerformEndCallAction(CXProvider provider, CXEndCallAction action)
        {
            var call = CallManager.FindCall(action.CallUuid);

            AudioController.StopPlayingSoundFile();
            AudioController.StartPlayingSoundFile(PathForSound("EndCallSound.wav"), false);

            if (call == null)
            {
                return;
            }
            else
            {
                provider.ReportConnectedOutgoingCall(call.UUID, NSDate.Now);
                action.Fulfill();
                call.SINCall?.Hangup();
                CallManager.Calls.Remove(call);
            }
        }

        public override void PerformSetHeldCallAction(CXProvider provider, CXSetHeldCallAction action)
        {
            var call = CallManager.FindCall(action.CallUuid);

            AudioController.StopPlayingSoundFile();

            if (call == null)
            {
                action.Fail();
                return;
            }

            call.IsOnHold = action.OnHold;

            action.Fulfill();
        }

        public override void TimedOutPerformingAction(CXProvider provider, CXAction action)
        {
            action.Fulfill();
        }

        public override void DidActivateAudioSession(CXProvider provider, AVAudioSession audioSession)
        {
            audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            audioSession.SetActive(true);
        }

        public override void DidDeactivateAudioSession(CXProvider provider, AVAudioSession audioSession)
        {
            audioSession.SetActive(false);
        }

        #endregion

        #region Public Methods

        public void StartCallClientDelegate()
        {
            Client.CallClient.WeakDelegate = this;
        }

        public void ReportIncomingCall(NSUuid uuid, string handle)
        {
            var callerId = Int32.Parse(handle);
            var callerName = RealmUtils.GetCallerName(callerId);

            var update = new CXCallUpdate();
            update.RemoteHandle = new CXHandle(CXHandleType.Generic, callerName);

            // Report incoming call to system
            Provider.ReportNewIncomingCall(uuid, update, (error) =>
            {
                if (error == null)
                    CallManager.Calls.Add(new ActiveCall(uuid, callerName, callerId, false, Call));
                else
                    Console.WriteLine("Error: {0}", error);
            });
        }

        #endregion

        #region SIN Client Delegate Methods

        [Export("client:didReceiveIncomingCall:")]
        void ClientDidReceiveIncomingCall(ISINClient xclient, ISINCall xcall)
        {
            Call = xcall;
            Call.WeakDelegate = this;
        }

        [Export("callDidEnd:")]
        public void CallDidEnd(ISINCall xcall)
        {
            var call = CallManager.Calls.Where(x => x?.SINCall?.CallId == xcall.CallId).LastOrDefault();
            CallManager.EndCall(call);
        }

        #endregion

        #region Helpers

        private string PathForSound(string soundName)
        {
            return Path.Combine(NSBundle.MainBundle.ResourcePath, soundName);
        }

        #endregion
    }
}