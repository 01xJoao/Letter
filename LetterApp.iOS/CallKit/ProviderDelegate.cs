using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AVFoundation;
using CallKit;
using DT.Xamarin.Agora;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.AgoraIO;
using LetterApp.Core.Helpers;
using LetterApp.iOS.AgoraIO;
using LetterApp.iOS.Views.Call;
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
        public AgoraRtcEngineKit AgoraKit { get; set; }

        private CallViewController _viewController;
        private AVAudioSession audioSession = AVAudioSession.SharedInstance();

        #endregion

        private ISINCall call;
        public ISINCall Call
        {
            get
            {
                return call;
            }

            set
            {
                call = value;
                call.WeakDelegate = this;
            }
        }

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
            Call = Client.CallClient.CallUserWithId(activeCall.CallerId.ToString());
            activeCall.SINCall = Call;

            // Monitor state changes
            activeCall.StartingConnectionChanged += (call) =>
            {
                if (call.IsConnecting)
                {
                    // Inform system that the call is starting
                    Provider.ReportConnectingOutgoingCall(call.UUID, (NSDate)call.StartedConnectingOn);
                    action.Fulfill();
                }
            };

            activeCall.ConnectedChanged += (call) =>
            {
                if (call.IsConnected)
                {
                    // Inform system that the call has connected
                    provider.ReportConnectedOutgoingCall(call.UUID, (NSDate)call.ConnectedOn);
                    action.Fulfill();
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

            call.AnswerCall();
            action.Fulfill();
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
                AgoraCallEnded();
                call?.SINCall?.Hangup();

                CallManager.Calls.Remove(call);
                action.Fulfill();
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
            //audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            //audioSession.SetActive(true);
        }

        public override void DidDeactivateAudioSession(CXProvider provider, AVAudioSession audioSession)
        {
            //audioSession.SetActive(false);
        }

        public override void PerformSetMutedCallAction(CXProvider provider, CXSetMutedCallAction action)
        {
            AgoraSetMute(action.Muted);
            action.Fulfill();
        }

        public override void PerformSetGroupCallAction(CXProvider provider, CXSetGroupCallAction action) {}


        #endregion

        #region Public Methods

        public void StartCallClientDelegate()
        {
            Client.CallClient.WeakDelegate = this;
        }

        public void ReportIncomingCall(NSUuid uuid, string handle)
        {
            if (CallManager.Calls.LastOrDefault() != null)
                return;

            var callerId = Int32.Parse(handle);
            var callerName = RealmUtils.GetCallerName(callerId);

            var update = new CXCallUpdate();
            update.RemoteHandle = new CXHandle(CXHandleType.Generic, callerName);
            update.SupportsDtmf = true;
            update.HasVideo = false;
            update.SupportsGrouping = false;
            update.SupportsHolding = false;
            update.SupportsUngrouping = false;

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

        #region SINClientDelegate Methods

        [Export("client:didReceiveIncomingCall:")]
        void ClientDidReceiveIncomingCall(ISINClient xclient, ISINCall xcall)
        {
            if (CallManager.Calls.LastOrDefault() != null)
            {
                xcall.Hangup();
                return;
            }
            
            Call = xcall;
        }

        [Export("callDidEnd:")]
        public void CallDidEnd(ISINCall xcall)
        {         
            var call = CallManager.Calls.LastOrDefault();

            if(call != null)
            {
                call.Ended = true;

                if (call.IsConnected == false)
                    CallManager.EndCall(call);
            }
            else
            {
                Call?.Hangup();
            }
        }

        #endregion


        #region AgoraIO

        private TaskCompletionSource<bool> _joinedCompleted;

        public Task<bool> SetupAgoraIO(CallViewController viewController, string roomName, bool speaker, bool muted)
        {
            _viewController = viewController;
            _joinedCompleted = new TaskCompletionSource<bool>();

            AgoraKit = AgoraRtcEngineKit.SharedEngineWithAppIdAndDelegate(AgoraSettings.AgoraAPI, viewController.AgoraDelegate);
            AgoraKit.SetChannelProfile(ChannelProfile.Communication);
            AgoraKit.JoinChannelByToken(AgoraSettings.AgoraAPI, roomName, null, 0, (NSString arg, nuint arg1, nint arg2) =>
            {
                var call = CallManager.Calls.LastOrDefault();

                if (call != null && !call.Ended)
                    _joinedCompleted.TrySetResult(true);
                else
                    _joinedCompleted.TrySetResult(false);

                _joinedCompleted = null;

            });
            AgoraKit.SetEnableSpeakerphone(speaker);
            AgoraKit.MuteLocalAudioStream(muted);

            return _joinedCompleted.Task;
        }

        public void AgoraCallStarted()
        {
            var call = CallManager.Calls.LastOrDefault();


            if (call == null)
                return;

            if(call.IsOutgoing)
            {
                AudioController.StopPlayingSoundFile();
                call.AnswerCall();
                call?.SINCall?.Hangup();
            }
        }

        public void AgoraCallEnded()
        {
            AgoraKit?.LeaveChannel(null);
            AgoraKit?.Dispose();
            AgoraKit = null;
        }

        public void AgoraSetSpeaker(bool speakerOn)
        {
            AgoraKit?.SetEnableSpeakerphone(speakerOn);
        }

        public void AgoraSetMute(bool mutedOn, bool fromView = false)
        {
            if(fromView)
            {
                CallManager.MuteCall(CallManager.Calls.LastOrDefault(), mutedOn);
            }
            else
            {
                AgoraKit?.MuteLocalAudioStream(mutedOn);
                _viewController.AudioMuted(mutedOn);
            }
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