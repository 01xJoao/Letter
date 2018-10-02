using System;
using System.IO;
using System.Linq;
using CallKit;
using DT.Xamarin.Agora;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.AgoraIO;
using LetterApp.Core.Helpers;
using LetterApp.Core.Models;
using LetterApp.iOS.Views.Base;
using LetterApp.iOS.Views.Call;
using SinchBinding;
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

        #region private Properties
        private bool _comesFromBackground;
        private string _roomName;
        private CallViewController _callViewController;
        private AgoraRtcEngineKit _agoraKit;
        #endregion

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

            Configuration = new CXProviderConfiguration("Letter")
            {
                SupportsVideo = false,
                MaximumCallGroups = 1,
                MaximumCallsPerCallGroup = 1,
                SupportedHandleTypes = new NSSet<NSNumber>((NSNumber)(int)CXHandleType.Generic),
                IconTemplateImageData = UIImage.FromBundle("letter_voip").AsPNG()
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

            if (activeCall == null)
                return;
                
            SINCall = Client.CallClient.CallUserWithId($"{activeCall.CallerId.ToString()}-{AppSettings.OrganizationId}");

            activeCall.SINCall = SINCall;
            activeCall.StartCall();

            Provider.ReportConnectingOutgoingCall(activeCall.UUID, NSDate.Now);
            action.Fulfill();
        }

        public override void PerformAnswerCallAction(CXProvider provider, CXAnswerCallAction action)
        {
            var call = CallManager.FindCall(action.CallUuid);

            if (call == null)
            {
                action.Fail();
                return;
            }

            if (!call.IsOutgoing)
                App.StartCall(call.CallerId);

            provider.ReportConnectedOutgoingCall(call.UUID, NSDate.Now);
            action.Fulfill();
        }

        public override void PerformEndCallAction(CXProvider provider, CXEndCallAction action)
        {
            var call = CallManager.FindCall(action.CallUuid);

            if (call == null)
                return;

            AgoraCallEnded();

            if (call.IsConnecting && call.Ended == false)
                call.SINCall?.Hangup();

            var callHistory = new CallModel
            {
                CallId = call.SINCall.CallId,
                CallDate = DateTime.Now.Ticks,
                CallerId = call.CallerId,
                CallType = call.IsOutgoing ? 0 : 1,
                Success = call.IsConnected,
                IsNew = true
            };

            RealmUtils.AddCallToHistory(callHistory);

            CallManager.Calls.Remove(call);

            provider.ReportConnectedOutgoingCall(call.UUID, NSDate.Now);
            action.Fulfill();
        }

        public override void PerformSetHeldCallAction(CXProvider provider, CXSetHeldCallAction action)
        {
            var call = CallManager.FindCall(action.CallUuid);

            if (call == null)
            {
                action.Fail();
                return;
            }

            call.IsOnHold = action.OnHold;

            action.Fulfill();
        }

        public override void TimedOutPerformingAction(CXProvider provider, CXAction action) => action.Fulfill();

        public override void PerformSetMutedCallAction(CXProvider provider, CXSetMutedCallAction action)
        {
            AgoraSetMute(action.Muted);
            action.Fulfill();
        }

        public override void PerformSetGroupCallAction(CXProvider provider, CXSetGroupCallAction action) { }


        #endregion

        #region Public Methods

        public void StartCallClientDelegate()
        {
            Client.CallClient.WeakDelegate = this;
        }

        public ActiveCall ReportIncomingCall(NSUuid uuid, ISINCall handle)
        {
            if(CallManager.Calls.LastOrDefault() != null)
                return null;
                
            var callerId = StringUtils.GetUserId(handle.RemoteUserId);

            if (callerId == 0)
                return null;

            var callerName = RealmUtils.GetCallerName(callerId);

            ActiveCall call = new ActiveCall(uuid, callerName, callerId, false, handle, true);

          
            var update = new CXCallUpdate();
            update.RemoteHandle = new CXHandle(CXHandleType.Generic, callerName);
            update.SupportsDtmf = true;
            update.HasVideo = false;
            update.SupportsGrouping = false;
            update.SupportsHolding = false;
            update.SupportsUngrouping = false;

            Provider.ReportNewIncomingCall(uuid, update, (error) =>
            {
                if (error == null)
                {
                 
                    CallManager.Calls.Add(call);
                }
                else
                {
                    Console.WriteLine("Error: {0}", error);
                    //AgoraCallEnded();
                }
            });

            return call;
        }

        #endregion

        //#region SINClientDelegate Methods

        //[Export("client:willReceiveIncomingCall:")]
        //public void WillReceiveIncomingCall(ISINCallClient client, ISINCall call)
        //{
        //    var xcall = CallManager.Calls.LastOrDefault();

        //    if (xcall?.SINCall == null)
        //    {
        //        _comesFromBackground = true;
        //        SINCall = call;
        //    }
        //    else
        //    {
        //        if (xcall.IsConnected)
        //            call.Hangup();
        //        else
        //            SINCall = call;
        //    }
        //}

        //[Export("client:didReceiveIncomingCall:")]
        //public void DidReceiveIncomingCall(ISINCallClient client, ISINCall call)
        //{
        //    if (_comesFromBackground)
        //        return;

        //    var xcall = CallManager.Calls.LastOrDefault();

        //    if (xcall == null)
        //    {
        //        SINCall = call;
        //    }
        //    else
        //    {
        //        if (xcall.IsConnected)
        //            call.Hangup();
        //        else
        //            SINCall = call;
        //    }
        //}

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

            _comesFromBackground = false;
            _sinCall = null;
        }

        //#endregion


        #region AgoraIO

        public void SetupAgoraIO(CallViewController callViewController, string roomName, bool speaker, bool muted)
        {
            _roomName = roomName;
            _callViewController = callViewController;
            _agoraKit = AgoraRtcEngineKit.SharedEngineWithAppIdAndDelegate(AgoraSettings.AgoraAPI, _callViewController.AgoraDelegate);
            _agoraKit.SetChannelProfile(ChannelProfile.Communication);
            _agoraKit.SetEnableSpeakerphone(speaker);
            _agoraKit.MuteLocalAudioStream(muted);
            _agoraKit.SetAudioProfile(AudioProfile.MusicHighQualityStereo, AudioScenario.GameStreaming);
            _agoraKit.EnableExternalAudioSourceWithSampleRate(48, 2);
            _agoraKit.SetEffectsVolume(100);
            _agoraKit.PlayEffect(0, PathForSound("ringback.wav"), 100, 1, 0, 100);

            _agoraKit?.JoinChannelByToken(AgoraSettings.AgoraAPI, _roomName, null, 0,(arg1, arg2, arg3) => {
                _callViewController.JoinCompleted(); 
            });
        }

        public void AgoraCallStarted()
        {
            _agoraKit?.StopAllEffects();

            var call = CallManager.Calls.LastOrDefault();

            if (call == null)
                return;

            call.AnswerCall();

            if (call.IsOutgoing)
                CallManager.AnswerCall(call);
            else
                call?.SINCall?.Hangup();
        }

        public void AgoraCallEnded()
        {
            _agoraKit?.StopAllEffects();
            _agoraKit?.LeaveChannel(AgoraLeftChannelCompleted);
        }

        private void AgoraLeftChannelCompleted(AgoraChannelStats stats)
        {
            _callViewController.UserEndedCall();
            _agoraKit?.Dispose();
            _agoraKit = null;
            _callViewController = null;
            _roomName = string.Empty;
        }

        public void AgoraSetSpeaker(bool speakerOn)
        {
            _agoraKit?.SetEnableSpeakerphone(speakerOn);
        }

        public void AgoraSetMute(bool mutedOn, bool fromView = false)
        {
            if (fromView)
            {
                CallManager.MuteCall(CallManager.Calls.LastOrDefault(), mutedOn);
            }
            else
            {
                _agoraKit?.MuteLocalAudioStream(mutedOn);
                _callViewController.AudioMuted(mutedOn);
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
