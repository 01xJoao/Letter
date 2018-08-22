using System;
using System.Diagnostics;
using DT.Xamarin.Agora;
using Foundation;
using LetterApp.iOS.Views.Call;

namespace LetterApp.iOS.AgoraIO
{
    public class AgoraRtcDelegate : AgoraRtcEngineDelegate
    {
        private CallViewController _controller;

        public AgoraRtcDelegate(CallViewController controller) : base()
        {
            _controller = controller;
        }

        public override void DidJoinedOfUid(AgoraRtcEngineKit engine, nuint uid, nint elapsed)
        {
            Debug.WriteLine($"DidJoinedOfUid {uid}");
            _controller.DidEnterRoom();
        }

        public override void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            Debug.WriteLine($"DidOfflineOfUid {uid}");
            _controller.DidOfflineOfUid();
        }

        public override void DidLeaveChannelWithStats(AgoraRtcEngineKit engine, AgoraChannelStats stats)
        {
            Debug.WriteLine($"DidLeftChannel {stats}");
            _controller.UserEndedCall();
        }
    }
}
