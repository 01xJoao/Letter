using System;
using System.Runtime.InteropServices;
using AVFoundation;
using ObjCRuntime;
using UIKit;

namespace SinchBinding
{
    [Native]
    public enum SINAPSEnvironment : ulong
    {
        Development = 1,
        Production = 2
    }

    [Native]
    public enum SINLogSeverity : ulong
    {
        Trace = 0,
        Info,
        Warn,
        Critical
    }

    [Native]
    public enum SINCallState : ulong
    {
        Initiating = 0,
        Progressing,
        Established,
        Ended
    }

    [Native]
    public enum SINCallDirection : ulong
    {
        Incoming = 0,
        Outgoing
    }

    [Native]
    public enum SINCallEndCause : ulong
    {
        None = 0,
        Timeout = 1,
        Denied = 2,
        NoAnswer = 3,
        Error = 4,
        HungUp = 5,
        Canceled = 6,
        OtherDeviceAnswered = 7
    }

    //static class CFunctions
    //{
    //    // extern AVCaptureDevicePosition SINToggleCaptureDevicePosition (AVCaptureDevicePosition position) __attribute__((visibility("default")));
    //    [DllImport ("__Internal")]
    //    static extern AVCaptureDevicePosition SINToggleCaptureDevicePosition (AVCaptureDevicePosition position);
    //
    //    // extern UIImage * SINUIImageFromVideoFrame (id<SINVideoFrame> videoFrame) __attribute__((visibility("default")));
    //    [DllImport ("__Internal")]
    //    static extern UIImage SINUIImageFromVideoFrame (SINVideoFrame videoFrame);
    //}
}
