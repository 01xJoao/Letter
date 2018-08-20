using System;
using System.IO;
using System.Linq;
using AVFoundation;
using CallKit;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.Helpers;
using LetterApp.Core.Services.Interfaces;
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
            // Save connection to call manager
            CallManager = callManager;

            // Define handle types
            var handleTypes = new[] {(NSNumber)(int)CXHandleType.Generic};

            // Get Image Mask
            var maskImage = UIImage.FromBundle("letter_curved");

            // Setup the initial configurations
            Configuration = new CXProviderConfiguration("Letter")
            {
                SupportsVideo = false,
                MaximumCallsPerCallGroup = 1,
                SupportedHandleTypes = new NSSet<NSNumber>(handleTypes),
                IconTemplateImageData = maskImage.AsPNG(),
                RingtoneSound = "Audio/iphone_call.mp3"
            };

            // Create a new provider
            Provider = new CXProvider(Configuration);

            // Attach this delegate
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
            activeCall.StartingConnectionChanged += (call) => {
                if (call.isConnecting)
                {
                    // Inform system that the call is starting
                    Provider.ReportConnectingOutgoingCall(call.UUID, (NSDate) call.StartedConnectingOn);
                }
            };

            activeCall.ConnectedChanged += (call) => {
                if (call.isConnected)
                {
                    // Inform system that the call has connected
                    provider.ReportConnectedOutgoingCall(call.UUID, (NSDate) call.ConnectedOn);
                }
            };

            activeCall.StartCall();
            AudioController.StartPlayingSoundFile(PathForSound("ringback.wav"), true);
        }

        public override void PerformAnswerCallAction(CXProvider provider, CXAnswerCallAction action)
        {
            //Find requested call
            var call = CallManager.FindCall(action.CallUuid);

            AudioController.StopPlayingSoundFile();

            // Found?
            if (call == null)
            {
                // No, inform system and exit
                action.Fail();
                return;
            }
            else
            {
                if (!call.IsOutgoing)
                    App.StartCall(call.CallerId);
            }

            // Attempt to answer call
            call.AnswerCall((successful) => {
                // Was the call successfully answered?
                if (successful)
                {

                    // Yes, inform system
                    action.Fulfill();
                }
                else
                {
                    // No, inform system
                    action.Fail();
                }
            });
        }

        public override void PerformEndCallAction(CXProvider provider, CXEndCallAction action)
        {
            // Find requested call
            var call = CallManager.FindCall(action.CallUuid);

            AudioController.StopPlayingSoundFile();

            AudioController.StartPlayingSoundFile(PathForSound("EndCallSound.wav"), false);


            // Found?
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
            // Find requested call
            var call = CallManager.FindCall(action.CallUuid);

            AudioController.StopPlayingSoundFile();

            // Found?
            if (call == null)
            {
                // No, inform system and exit
                action.Fail();
                return;
            }

            // Update hold status
            call.isOnhold = action.OnHold;

            // Inform system of success
            action.Fulfill();
        }

        public override void TimedOutPerformingAction(CXProvider provider, CXAction action)
        {
            // Inform user that the action has timed out
            action.Fulfill();
        }

        public override void DidActivateAudioSession(CXProvider provider, AVFoundation.AVAudioSession audioSession)
        {
            audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            audioSession.SetActive(true);
        }

        public override void DidDeactivateAudioSession(CXProvider provider, AVFoundation.AVAudioSession audioSession)
        {
           audioSession.SetActive(false);
        }

        string PathForSound(string soundName)
        {
            return Path.Combine(NSBundle.MainBundle.ResourcePath, soundName);
        }
        #endregion

        #region Public Methods
        public void ReportIncomingCall(NSUuid uuid, string handle)
        {
            var callerId = Int32.Parse(handle);
            var callerName = RealmUtils.GetCallerName(callerId);

            // Create update to describe the incoming call and caller
            var update = new CXCallUpdate();

            update.RemoteHandle = new CXHandle(CXHandleType.Generic, callerName);

            // Report incoming call to system
            Provider.ReportNewIncomingCall(uuid, update, (error) => {
                // Was the call accepted
                if (error == null)
                {
                    // Yes, report to call manager
                    CallManager.Calls.Add(new ActiveCall(uuid, callerName, callerId, false, Call));
                }
                else
                {
                    // Report error to user here
                    Console.WriteLine("Error: {0}", error);
                }
            });
        }

        public void StartCallClientDelegate()
        {
            Client.CallClient.WeakDelegate = this;
        }

        #endregion

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
    }
}