using System;
using System.Diagnostics;
using DT.Xamarin.Agora;
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
            _controller.DidEnterRoom(engine, uid, elapsed);
        }

        public override void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            Debug.WriteLine($"DidOfflineOfUid {uid}");
            _controller.DidOfflineOfUid(engine, uid, reason);
        }
    }
}
