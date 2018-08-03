using System;
using System.Diagnostics;
using DT.Xamarin.Agora;
using Foundation;
using LetterApp.Core.AgoraIO;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.AgoraIO;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Call
{
    public partial class CallViewController : XViewController<CallViewModel>
    {
        public override bool ShowAsPresentView => true;

        uint _localId = 0;
        uint _remoteId = 0;

        public AgoraRtcDelegate AgoraDelegate;
        public AgoraRtcEngineKit AgoraKit;

        private bool _audioMuted;
        public bool AudioMuted
        {
            get
            {
                return _audioMuted;
            }
            set
            {
                _audioMuted = value;
                AgoraKit.MuteLocalAudioStream(value);
                UpdateMicrophoneView(value);
            }
        }

        public CallViewController() : base("CallViewController", null) { }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupAgoraIO();
        }

        private void SetupAgoraIO()
        {
            AgoraDelegate = new AgoraRtcDelegate(this);
            AgoraKit = AgoraRtcEngineKit.SharedEngineWithAppIdAndDelegate(AgoraSettings.AgoraAPI, AgoraDelegate);
            AgoraKit.SetChannelProfile(ChannelProfile.Communication);
            AgoraKit.JoinChannelByToken(AgoraSettings.AgoraAPI, "ROOMNAME", null, 0, JoiningCompleted);
        }

        private void JoiningCompleted(NSString channel, nuint uid, nint elapsed)
        {
            _localId = (uint)uid;
            //AgoraKit.SetEnableSpeakerphone(true);
            UIApplication.SharedApplication.IdleTimerDisabled = true;
        }


        private void UpdateMicrophoneView(bool mute)
        {
            //Change Muted Icon
        }

        public void DidEnterRoom(AgoraRtcEngineKit engine, nuint uid, nint elapsed)
        {
            //ChangeContent
        }

        public void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            LeaveChannel();
        }

        public void LeaveChannel()
        {
            AgoraKit.LeaveChannel(null);
            //Leave View
            AgoraKit?.Dispose();
            AgoraKit = null;
        }

        private void RefreshDebug()
        {
            Debug.WriteLine($"local: {_localId}\nremote: {_remoteId}");
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            UIApplication.SharedApplication.IdleTimerDisabled = false;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }
    }
}

