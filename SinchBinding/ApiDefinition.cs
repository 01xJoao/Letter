using System;
using AVFoundation;
using CallKit;
using Foundation;
using ObjCRuntime;
using UIKit;
using CoreVideo;

namespace SinchBinding {

    partial interface ISINClientDelegate { }
    partial interface ISINClientRegistration { }
    partial interface ISINClient { }
    partial interface ISINCallClient { }
    partial interface ISINCallClientDelegate { }
    partial interface ISINCall { }
    partial interface ISINCallDelegate { }
    partial interface ISINCallDetails { }
    partial interface ISINMessageClient { }
    partial interface ISINMessageClientDelegate { }
    partial interface ISINMessage { }
    partial interface ISINMessageDeliveryInfo { }
    partial interface ISINMessageFailureInfo { }
    partial interface ISINAudioController { }
    partial interface ISINAudioControllerDelegate { }
    partial interface ISINVideoController { }
    partial interface ISINPushPair { }
    partial interface ISINManagedPush { }
    partial interface ISINManagedPushDelegate { }
    partial interface ISINNotificationResult { }
    partial interface ISINCallNotificationResult { }
    partial interface ISINMessageNotificationResult { }
    partial interface ISINVideoFrameCallback { }
    partial interface ISINVideoFrame { }

    [Static]
    partial interface Constants {
        // extern NSString *const SINClientDidStartNotification __attribute__((visibility("default")));
        [Field("SINClientDidStartNotification", "__Internal")]
        NSString SINClientDidStartNotification { get; }

        // extern NSString *const SINClientDidFailNotification __attribute__((visibility("default")));
        [Field("SINClientDidFailNotification", "__Internal")]
        NSString SINClientDidFailNotification { get; }

        // extern NSString *const SINClientWillTerminateNotification __attribute__((visibility("default")));
        [Field("SINClientWillTerminateNotification", "__Internal")]
        NSString SINClientWillTerminateNotification { get; }

        // extern NSString *const SINIncomingCallNotification __attribute__((visibility("default")));
        [Field("SINIncomingCallNotification", "__Internal")]
        NSString SINIncomingCallNotification { get; }

        // extern NSString *const SINCallDidProgressNotification __attribute__((visibility("default")));
        [Field("SINCallDidProgressNotification", "__Internal")]
        NSString SINCallDidProgressNotification { get; }

        // extern NSString *const SINCallDidEstablishNotification __attribute__((visibility("default")));
        [Field("SINCallDidEstablishNotification", "__Internal")]
        NSString SINCallDidEstablishNotification { get; }

        // extern NSString *const SINCallDidEndNotification __attribute__((visibility("default")));
        [Field("SINCallDidEndNotification", "__Internal")]
        NSString SINCallDidEndNotification { get; }

        // extern NSString *const SINCallKey __attribute__((visibility("default")));
        [Field("SINCallKey", "__Internal")]
        NSString SINCallKey { get; }

        // extern NSString *const SINPushTypeVoIP __attribute__((visibility("default"))) __attribute__((availability(ios, introduced=8_0)));
        [iOS(8, 0)]
        [Field("SINPushTypeVoIP", "__Internal")]
        NSString SINPushTypeVoIP { get; }

        // extern NSString *const SINPushTypeRemote __attribute__((visibility("default"))) __attribute__((availability(ios, introduced=6_0)));
        [iOS(6, 0)]
        [Field("SINPushTypeRemote", "__Internal")]
        NSString SINPushTypeRemote { get; }

        // extern NSString *const SINApplicationDidReceiveRemoteNotification __attribute__((visibility("default")));
        [Field("SINApplicationDidReceiveRemoteNotification", "__Internal")]
        NSString SINApplicationDidReceiveRemoteNotification { get; }

        // extern NSString *const SINRemoteNotificationKey __attribute__((visibility("default")));
        [Field("SINRemoteNotificationKey", "__Internal")]
        NSString SINRemoteNotificationKey { get; }

        // extern NSString *const SINPushTypeKey __attribute__((visibility("default")));
        [Field("SINPushTypeKey", "__Internal")]
        NSString SINPushTypeKey { get; }

        // extern NSString *const SINErrorDomainNetwork __attribute__((visibility("default")));
        [Field("SINErrorDomainNetwork", "__Internal")]
        NSString SINErrorDomainNetwork { get; }

        // extern NSString *const SINErrorDomainCapability __attribute__((visibility("default")));
        [Field("SINErrorDomainCapability", "__Internal")]
        NSString SINErrorDomainCapability { get; }

        // extern NSString *const SINErrorDomainOther __attribute__((visibility("default")));
        [Field("SINErrorDomainOther", "__Internal")]
        NSString SINErrorDomainOther { get; }
    }

    // @protocol SINClient <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINClient {
        //[Wrap("WeakDelegate"), Abstract]
        //SINClientDelegate Delegate { get; set; }

        // @required @property (nonatomic, weak) id<SINClientDelegate> delegate;
        [Abstract]
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // @required @property (readonly, copy, nonatomic) NSString * userId;
        [Abstract]
        [Export("userId")]
        string UserId { get; }

        // @required -(void)setSupportCalling:(BOOL)supported;
        [Abstract]
        [Export("setSupportCalling:")]
        void SetSupportCalling(bool supported);

        // @required -(void)setDataProtectionType:(NSFileProtectionType)type;
        [Abstract]
        [Export("setDataProtectionType:")]
        void SetDataProtectionType(string type);

        // @required -(void)setSupportMessaging:(BOOL)supported;
        [Abstract]
        [Export("setSupportMessaging:")]
        void SetSupportMessaging(bool supported);

        // @required -(void)setSupportPushNotifications:(BOOL)supported;
        [Abstract]
        [Export("setSupportPushNotifications:")]
        void SetSupportPushNotifications(bool supported);

        // @required -(void)enableManagedPushNotifications;
        [Abstract]
        [Export("enableManagedPushNotifications")]
        void EnableManagedPushNotifications();

        // @required -(void)setSupportActiveConnectionInBackground:(BOOL)supported __attribute__((availability(ios, introduced=4_0, deprecated=9_0)));
        [Introduced(PlatformName.iOS, 4, 0, message: "Please use PushKit and SINManagedPush")]
        [Deprecated(PlatformName.iOS, 9, 0, message: "Please use PushKit and SINManagedPush")]
        [Abstract]
        [Export("setSupportActiveConnectionInBackground:")]
        void SetSupportActiveConnectionInBackground(bool supported);

        // @required -(void)start;
        [Abstract]
        [Export("start")]
        void Start();

        // @required -(void)terminate;
        [Abstract]
        [Export("terminate")]
        void Terminate();

        // @required -(void)terminateGracefully;
        [Abstract]
        [Export("terminateGracefully")]
        void TerminateGracefully();

        // @required -(void)stop;
        [Abstract]
        [Export("stop")]
        void Stop();

        // @required -(BOOL)isStarted;
        [Abstract]
        [Export("isStarted")]
        bool IsStarted { get; }

        // @required -(void)startListeningOnActiveConnection;
        [Abstract]
        [Export("startListeningOnActiveConnection")]
        void StartListeningOnActiveConnection();

        // @required -(void)stopListeningOnActiveConnection;
        [Abstract]
        [Export("stopListeningOnActiveConnection")]
        void StopListeningOnActiveConnection();

        // @required -(id<SINNotificationResult>)relayRemotePushNotificationPayload:(NSString *)payload;
        [Abstract]
        [Export("relayRemotePushNotificationPayload:")]
        ISINNotificationResult RelayRemotePushNotificationPayload(string payload);

        // @required -(id<SINNotificationResult>)relayRemotePushNotification:(NSDictionary *)userInfo;
        [Abstract]
        [Export("relayRemotePushNotification:")]
        ISINNotificationResult RelayRemotePushNotification(NSDictionary userInfo);

        // @required -(id<SINNotificationResult>)relayLocalNotification:(UILocalNotification *)notification;
        [Abstract]
        [Export("relayLocalNotification:")]
        ISINNotificationResult RelayLocalNotification(UILocalNotification notification);

        // @required -(void)registerPushNotificationData:(NSData *)pushNotificationData;
        [Abstract]
        [Export("registerPushNotificationData:")]
        void RegisterPushNotificationData(NSData pushNotificationData);

        // @required -(void)unregisterPushNotificationData;
        [Abstract]
        [Export("unregisterPushNotificationData")]
        void UnregisterPushNotificationData();

        // @required -(void)registerPushNotificationDeviceToken:(NSData *)deviceToken type:(NSString *)pushType apsEnvironment:(SINAPSEnvironment)apsEnvironment;
        [Abstract]
        [Export("registerPushNotificationDeviceToken:type:apsEnvironment:")]
        void RegisterPushNotificationDeviceToken(NSData deviceToken, string pushType, SINAPSEnvironment apsEnvironment);

        // @required -(void)unregisterPushNotificationDeviceToken;
        [Abstract]
        [Export("unregisterPushNotificationDeviceToken")]
        void UnregisterPushNotificationDeviceToken();

        // @required -(void)setPushNotificationDisplayName:(NSString *)displayName;
        [Abstract]
        [Export("setPushNotificationDisplayName:")]
        void SetPushNotificationDisplayName(string displayName);

        // @required -(id<SINCallClient>)callClient;
        [Abstract]
        [Export("callClient")]
        ISINCallClient CallClient { get; }

        // @required -(id<SINMessageClient>)messageClient;
        [Abstract]
        [Export("messageClient")]
        ISINMessageClient MessageClient { get; }

        // @required -(id<SINAudioController>)audioController;
        [Abstract]
        [Export("audioController")]
        ISINAudioController AudioController { get; }

        // @required -(id<SINVideoController>)videoController;
        [Abstract]
        [Export("videoController")]
        ISINVideoController VideoController { get; }
    }

    // @protocol SINClientDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINClientDelegate {
        // @required -(void)clientDidStart:(id<SINClient>)client;
        [Abstract]
        [Export("clientDidStart:")]
        void ClientDidStart(ISINClient client);

        // @required -(void)clientDidFail:(id<SINClient>)client error:(NSError *)error;
        [Abstract]
        [Export("clientDidFail:error:")]
        void ClientDidFail(ISINClient client, NSError error);

        // @optional -(void)clientDidStop:(id<SINClient>)client;
        [Export("clientDidStop:")]
        void ClientDidStop(ISINClient client);

        // @optional -(void)client:(id<SINClient>)client requiresRegistrationCredentials:(id<SINClientRegistration>)registrationCallback;
        [Export("client:requiresRegistrationCredentials:")]
        void Client(ISINClient client, ISINClientRegistration registrationCallback);

        // @optional -(void)client:(id<SINClient>)client logMessage:(NSString *)message area:(NSString *)area severity:(SINLogSeverity)severity timestamp:(NSDate *)timestamp;
        [Export("client:logMessage:area:severity:timestamp:")]
        void Client(ISINClient client, string message, string area, SINLogSeverity severity, NSDate timestamp);
    }

    // @protocol SINClientRegistration <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINClientRegistration {
        // @required -(void)registerWithSignature:(NSString *)signature sequence:(uint64_t)sequence;
        [Abstract]
        [Export("registerWithSignature:sequence:")]
        void RegisterWithSignature(string signature, ulong sequence);

        // @required -(void)registerDidFail:(NSError *)error;
        [Abstract]
        [Export("registerDidFail:")]
        void RegisterDidFail(NSError error);
    }

   

    // @protocol SINCallClient <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINCallClient {
        //[Wrap("WeakDelegate"), Abstract]
        //SINCallClientDelegate Delegate { get; set; }

        // @required @property (nonatomic, weak) id<SINCallClientDelegate> delegate;
        [Abstract]
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // @required -(id<SINCall>)callUserWithId:(NSString *)userId;
        [Abstract]
        [Export("callUserWithId:")]
        ISINCall CallUserWithId(string userId);

        // @required -(id<SINCall>)callUserWithId:(NSString *)userId headers:(NSDictionary *)headers;
        [Abstract]
        [Export("callUserWithId:headers:")]
        ISINCall CallUserWithId(string userId, NSDictionary headers);

        // @required -(id<SINCall>)callUserVideoWithId:(NSString *)userId;
        [Abstract]
        [Export("callUserVideoWithId:")]
        ISINCall CallUserVideoWithId(string userId);

        // @required -(id<SINCall>)callUserVideoWithId:(NSString *)userId headers:(NSDictionary *)headers;
        [Abstract]
        [Export("callUserVideoWithId:headers:")]
        ISINCall CallUserVideoWithId(string userId, NSDictionary headers);

        // @required -(id<SINCall>)callPhoneNumber:(NSString *)phoneNumber;
        [Abstract]
        [Export("callPhoneNumber:")]
        ISINCall CallPhoneNumber(string phoneNumber);

        // @required -(id<SINCall>)callPhoneNumber:(NSString *)phoneNumber headers:(NSDictionary *)headers;
        [Abstract]
        [Export("callPhoneNumber:headers:")]
        ISINCall CallPhoneNumber(string phoneNumber, NSDictionary headers);

        // @required -(id<SINCall>)callSIP:(NSString *)sipIdentity;
        [Abstract]
        [Export("callSIP:")]
        ISINCall CallSIP(string sipIdentity);

        // @required -(id<SINCall>)callSIP:(NSString *)sipIdentity headers:(NSDictionary *)headers;
        [Abstract]
        [Export("callSIP:headers:")]
        ISINCall CallSIP(string sipIdentity, NSDictionary headers);

        // @required -(id<SINCall>)callConferenceWithId:(NSString *)conferenceId;
        [Abstract]
        [Export("callConferenceWithId:")]
        ISINCall CallConferenceWithId(string conferenceId);

        // @required -(id<SINCall>)callConferenceWithId:(NSString *)conferenceId headers:(NSDictionary *)headers;
        [Abstract]
        [Export("callConferenceWithId:headers:")]
        ISINCall CallConferenceWithId(string conferenceId, NSDictionary headers);

        // @required -(void)provider:(CXProvider *)provider didActivateAudioSession:(AVAudioSession *)audioSession;
        [Abstract]
        [Export("provider:didActivateAudioSession:")]
        void Provider(CXProvider provider, AVAudioSession audioSession);
    }

    // @protocol SINCallClientDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINCallClientDelegate {
        // @optional -(void)client:(id<SINCallClient>)client willReceiveIncomingCall:(id<SINCall>)call;
        [Export("client:willReceiveIncomingCall:")]
        void WillReceiveIncomingCall(ISINCallClient client, ISINCall call);

        // @optional -(void)client:(id<SINCallClient>)client didReceiveIncomingCall:(id<SINCall>)call;
        [Export("client:didReceiveIncomingCall:")]
        void DidReceiveIncomingCall(ISINCallClient client, ISINCall call);

        // @optional -(SINLocalNotification *)client:(id<SINCallClient>)client localNotificationForIncomingCall:(id<SINCall>)call;
        [Export("client:localNotificationForIncomingCall:")]
        SINLocalNotification LocalNotificationForIncomingCall(ISINCallClient client, ISINCall call);
    }

    // @protocol SINCall <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINCall {
        //[Wrap("WeakDelegate"), Abstract]
        //SINCallDelegate Delegate { get; set; }

        // @required @property (nonatomic, weak) id<SINCallDelegate> delegate;
        [Abstract]
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // @required @property (readonly, copy, nonatomic) NSString * callId;
        [Abstract]
        [Export("callId")]
        string CallId { get; }

        // @required @property (readonly, copy, nonatomic) NSString * remoteUserId;
        [Abstract]
        [Export("remoteUserId")]
        string RemoteUserId { get; }

        // @required @property (readonly, nonatomic, strong) id<SINCallDetails> details;
        [Abstract]
        [Export("details", ArgumentSemantic.Strong)]
        ISINCallDetails Details { get; }

        // @required @property (readonly, assign, nonatomic) SINCallState state;
        [Abstract]
        [Export("state", ArgumentSemantic.Assign)]
        SINCallState State { get; }

        // @required @property (readonly, assign, nonatomic) SINCallDirection direction;
        [Abstract]
        [Export("direction", ArgumentSemantic.Assign)]
        SINCallDirection Direction { get; }

        // @required @property (readonly, nonatomic) NSDictionary * headers;
        [Abstract]
        [Export("headers")]
        NSDictionary Headers { get; }

        // @required @property (nonatomic, strong) id userInfo;
        [Abstract]
        [Export("userInfo", ArgumentSemantic.Strong)]
        NSObject UserInfo { get; set; }

        // @required -(void)answer;
        [Abstract]
        [Export("answer")]
        void Answer();

        // @required -(void)hangup;
        [Abstract]
        [Export("hangup")]
        void Hangup();

        // @required -(void)sendDTMF:(NSString *)key;
        [Abstract]
        [Export("sendDTMF:")]
        void SendDTMF(string key);

        // @required -(void)pauseVideo;
        [Abstract]
        [Export("pauseVideo")]
        void PauseVideo();

        // @required -(void)resumeVideo;
        [Abstract]
        [Export("resumeVideo")]
        void ResumeVideo();
    }

    // @protocol SINCallDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINCallDelegate {
        // @optional -(void)callDidEnd:(id<SINCall>)call;
        [Export("callDidEnd:")]
        void CallDidEnd(ISINCall call);

        // @optional -(void)callDidProgress:(id<SINCall>)call;
        [Export("callDidProgress:")]
        void CallDidProgress(ISINCall call);

        // @optional -(void)callDidEstablish:(id<SINCall>)call;
        [Export("callDidEstablish:")]
        void CallDidEstablish(ISINCall call);

        // @optional -(void)call:(id<SINCall>)call shouldSendPushNotifications:(NSArray *)pushPairs;
        [Export("call:shouldSendPushNotifications:")]
        //[Verify(StronglyTypedNSArray)]
        void Call(ISINCall call, NSArray[] pushPairs);

        // @optional -(void)callDidAddVideoTrack:(id<SINCall>)call;
        [Export("callDidAddVideoTrack:")]
        void CallDidAddVideoTrack(ISINCall call);

        // @optional -(void)callDidPauseVideoTrack:(id<SINCall>)call;
        [Export("callDidPauseVideoTrack:")]
        void CallDidPauseVideoTrack(ISINCall call);

        // @optional -(void)callDidResumeVideoTrack:(id<SINCall>)call;
        [Export("callDidResumeVideoTrack:")]
        void CallDidResumeVideoTrack(ISINCall call);
    }

    // @protocol SINCallDetails <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINCallDetails {
        // @required @property (readonly, nonatomic, strong) NSDate * startedTime;
        [Abstract]
        [Export("startedTime", ArgumentSemantic.Strong)]
        NSDate StartedTime { get; }

        // @required @property (readonly, nonatomic, strong) NSDate * establishedTime;
        [Abstract]
        [Export("establishedTime", ArgumentSemantic.Strong)]
        NSDate EstablishedTime { get; }

        // @required @property (readonly, nonatomic, strong) NSDate * endedTime;
        [Abstract]
        [Export("endedTime", ArgumentSemantic.Strong)]
        NSDate EndedTime { get; }

        // @required @property (readonly, nonatomic) SINCallEndCause endCause;
        [Abstract]
        [Export("endCause")]
        SINCallEndCause EndCause { get; }

        // @required @property (readonly, nonatomic, strong) NSError * error;
        [Abstract]
        [Export("error", ArgumentSemantic.Strong)]
        NSError Error { get; }

        // @required @property (readonly, nonatomic) UIApplicationState applicationStateWhenReceived;
        [Abstract]
        [Export("applicationStateWhenReceived")]
        UIApplicationState ApplicationStateWhenReceived { get; }

        // @required @property (readonly, getter = isVideoOffered, nonatomic) BOOL videoOffered;
        [Abstract]
        [Export("videoOffered")]
        bool VideoOffered { [Bind("isVideoOffered")] get; }
    }

    // @protocol SINMessageClient <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINMessageClient {
        //[Wrap("WeakDelegate"), Abstract]
        //SINMessageClientDelegate Delegate { get; set; }

        // @required @property (nonatomic, weak) id<SINMessageClientDelegate> delegate;
        [Abstract]
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // @required -(void)sendMessage:(SINOutgoingMessage *)message;
        [Abstract]
        [Export("sendMessage:")]
        void SendMessage(SINOutgoingMessage message);
    }

    // @protocol SINMessageClientDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINMessageClientDelegate {
        // @required -(void)messageClient:(id<SINMessageClient>)messageClient didReceiveIncomingMessage:(id<SINMessage>)message;
        [Abstract]
        [Export("messageClient:didReceiveIncomingMessage:")]
        void MessageClient(ISINMessageClient messageClient, ISINMessage message);

        // @required -(void)messageSent:(id<SINMessage>)message recipientId:(NSString *)recipientId;
        [Abstract]
        [Export("messageSent:recipientId:")]
        void MessageSent(ISINMessage message, string recipientId);

        // @required -(void)messageDelivered:(id<SINMessageDeliveryInfo>)info;
        [Abstract]
        [Export("messageDelivered:")]
        void MessageDelivered(ISINMessageDeliveryInfo info);

        // @required -(void)messageFailed:(id<SINMessage>)message info:(id<SINMessageFailureInfo>)messageFailureInfo;
        [Abstract]
        [Export("messageFailed:info:")]
        void MessageFailed(ISINMessage message, ISINMessageFailureInfo messageFailureInfo);

        // @optional -(void)message:(id<SINMessage>)message shouldSendPushNotifications:(NSArray *)pushPairs;
        [Export("message:shouldSendPushNotifications:")]
        //[Verify(StronglyTypedNSArray)]
        void Message(ISINMessage message, NSArray[] pushPairs);
    }

    // @protocol SINMessage <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINMessage {
        // @required @property (readonly, nonatomic) NSString * messageId;
        [Abstract]
        [Export("messageId")]
        string MessageId { get; }

        // @required @property (readonly, nonatomic) NSArray * recipientIds;
        [Abstract]
        [Export("recipientIds")]
        //[Verify(StronglyTypedNSArray)]
        NSArray[] RecipientIds { get; }

        // @required @property (readonly, nonatomic) NSString * senderId;
        [Abstract]
        [Export("senderId")]
        string SenderId { get; }

        // @required @property (readonly, nonatomic) NSString * text;
        [Abstract]
        [Export("text")]
        string Text { get; }

        // @required @property (readonly, nonatomic) NSDictionary * headers;
        [Abstract]
        [Export("headers")]
        NSDictionary Headers { get; }

        // @required @property (readonly, nonatomic) NSDate * timestamp;
        [Abstract]
        [Export("timestamp")]
        NSDate Timestamp { get; }
    }

    // @interface SINOutgoingMessage : NSObject
    [BaseType(typeof(NSObject))]
    interface SINOutgoingMessage {
        // @property (readonly, nonatomic) NSString * messageId;
        [Export("messageId")]
        string MessageId { get; }

        // @property (readonly, nonatomic) NSArray * recipientIds;
        [Export("recipientIds")]
        //[Verify(StronglyTypedNSArray)]
        NSArray[] RecipientIds { get; }

        // @property (readonly, nonatomic) NSString * text;
        [Export("text")]
        string Text { get; }

        // @property (readonly, nonatomic) NSDictionary * headers;
        [Export("headers")]
        NSDictionary Headers { get; }

        // +(SINOutgoingMessage *)messageWithRecipient:(NSString *)recipientId text:(NSString *)text;
        [Static]
        [Export("messageWithRecipient:text:")]
        SINOutgoingMessage MessageWithRecipient(string recipientId, string text);

        // +(SINOutgoingMessage *)messageWithRecipients:(NSArray *)recipientIds text:(NSString *)text;
        [Static]
        [Export("messageWithRecipients:text:")]
        //[Verify(StronglyTypedNSArray)]
        SINOutgoingMessage MessageWithRecipients(NSArray[] recipientIds, string text);

        // +(SINOutgoingMessage *)messageWithMessage:(id<SINMessage>)message;
        [Static]
        [Export("messageWithMessage:")]
        SINOutgoingMessage MessageWithMessage(ISINMessage message);

        // -(void)addHeaderWithValue:(NSString *)value key:(NSString *)key;
        [Export("addHeaderWithValue:key:")]
        void AddHeaderWithValue(string value, string key);
    }

    // @protocol SINMessageDeliveryInfo <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINMessageDeliveryInfo {
        // @required @property (readonly, copy, nonatomic) NSString * messageId;
        [Abstract]
        [Export("messageId")]
        string MessageId { get; }

        // @required @property (readonly, copy, nonatomic) NSString * recipientId;
        [Abstract]
        [Export("recipientId")]
        string RecipientId { get; }

        // @required @property (readonly, copy, nonatomic) NSDate * timestamp;
        [Abstract]
        [Export("timestamp", ArgumentSemantic.Copy)]
        NSDate Timestamp { get; }
    }

    // @protocol SINMessageFailureInfo <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINMessageFailureInfo {
        // @required @property (readonly, copy, nonatomic) NSString * messageId;
        [Abstract]
        [Export("messageId")]
        string MessageId { get; }

        // @required @property (readonly, copy, nonatomic) NSString * recipientId;
        [Abstract]
        [Export("recipientId")]
        string RecipientId { get; }

        // @required @property (readonly, copy, nonatomic) NSError * error;
        [Abstract]
        [Export("error", ArgumentSemantic.Copy)]
        NSError Error { get; }
    }

    // @protocol SINAudioController <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINAudioController {
        //[Wrap("WeakDelegate"), Abstract]
        //SINAudioControllerDelegate Delegate { get; set; }

        // @required @property (nonatomic, weak) id<SINAudioControllerDelegate> delegate;
        [Abstract]
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // @required -(void)mute;
        [Abstract]
        [Export("mute")]
        void Mute();

        // @required -(void)unmute;
        [Abstract]
        [Export("unmute")]
        void Unmute();

        // @required -(void)enableSpeaker;
        [Abstract]
        [Export("enableSpeaker")]
        void EnableSpeaker();

        // @required -(void)disableSpeaker;
        [Abstract]
        [Export("disableSpeaker")]
        void DisableSpeaker();

        // @required -(void)startPlayingSoundFile:(NSString *)path loop:(BOOL)loop;
        [Abstract]
        [Export("startPlayingSoundFile:loop:")]
        void StartPlayingSoundFile(string path, bool loop);

        // @required -(void)stopPlayingSoundFile;
        [Abstract]
        [Export("stopPlayingSoundFile")]
        void StopPlayingSoundFile();
    }

    // @protocol SINAudioControllerDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINAudioControllerDelegate {
        // @optional -(void)audioControllerMuted:(id<SINAudioController>)audioController;
        [Export("audioControllerMuted:")]
        void AudioControllerMuted(ISINAudioController audioController);

        // @optional -(void)audioControllerUnmuted:(id<SINAudioController>)audioController;
        [Export("audioControllerUnmuted:")]
        void AudioControllerUnmuted(ISINAudioController audioController);

        // @optional -(void)audioControllerSpeakerEnabled:(id<SINAudioController>)audioController;
        [Export("audioControllerSpeakerEnabled:")]
        void AudioControllerSpeakerEnabled(ISINAudioController audioController);

        // @optional -(void)audioControllerSpeakerDisabled:(id<SINAudioController>)audioController;
        [Export("audioControllerSpeakerDisabled:")]
        void AudioControllerSpeakerDisabled(ISINAudioController audioController);
    }

    // @protocol SINVideoFrame <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINVideoFrame {
        // @required @property (readonly) int width;
        [Abstract]
        [Export("width")]
        int Width { get; }

        // @required @property (readonly) int height;
        [Abstract]
        [Export("height")]
        int Height { get; }

        // @required -(CVPixelBufferRef)createCVPixelBuffer;
        [Abstract]
        [Export("createCVPixelBuffer")]
        unsafe CVPixelBuffer CreateCVPixelBuffer { get; }

        // @required -(void)releaseFrame;
        [Abstract]
        [Export("releaseFrame")]
        void ReleaseFrame();
    }

    // @protocol SINVideoFrameCallback <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINVideoFrameCallback {
        // @required -(void)onFrame:(id<SINVideoFrame>)videoFrame callId:(NSString *)callId;
        [Abstract]
        [Export("onFrame:callId:")]
        void CallId(ISINVideoFrame videoFrame, string callId);
    }

    // @protocol SINLocalVideoFrameCallback <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINLocalVideoFrameCallback {
        // @required -(void)onFrame:(CVPixelBufferRef)cvPixelBuffer completionHandler:(void (^)(CVPixelBufferRef))completionHandler;
        //[Abstract]
        //[Export("onFrame:completionHandler:")]
        //unsafe void CompletionHandler(CVPixelBuffer cvPixelBuffer, Action<CoreVideo.CVPixelBuffer> completionHandler);
    }

    // @protocol SINVideoController <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINVideoController {
        // @required @property (assign, readwrite, nonatomic) AVCaptureDevicePosition captureDevicePosition;
        [Abstract]
        [Export("captureDevicePosition", ArgumentSemantic.Assign)]
        AVCaptureDevicePosition CaptureDevicePosition { get; set; }

        // @required @property (assign, readwrite, nonatomic) BOOL disableIdleTimerOnCapturing;
        [Abstract]
        [Export("disableIdleTimerOnCapturing")]
        bool DisableIdleTimerOnCapturing { get; set; }

        // @required -(UIView *)remoteView;
        [Abstract]
        [Export("remoteView")]
        UIView RemoteView { get; }

        // @required -(UIView *)localView;
        [Abstract]
        [Export("localView")]
        UIView LocalView { get; }

        // @required -(void)setVideoFrameCallback:(id<SINVideoFrameCallback>)callback;
        [Abstract]
        [Export("setVideoFrameCallback:")]
        void SetVideoFrameCallback(ISINVideoFrameCallback callback);

        // @required -(void)setLocalVideoFrameCallback:(id<SINLocalVideoFrameCallback>)callback;
        [Abstract]
        [Export("setLocalVideoFrameCallback:")]
        void SetLocalVideoFrameCallback(SINLocalVideoFrameCallback callback);
    }

    // @protocol SINPushPair
    [Protocol, Model]
    interface SINPushPair {
        // @required @property (retain, nonatomic) NSData * pushData;
        [Abstract]
        [Export("pushData", ArgumentSemantic.Retain)]
        NSData PushData { get; set; }

        // @required @property (retain, nonatomic) NSString * pushPayload;
        [Abstract]
        [Export("pushPayload", ArgumentSemantic.Retain)]
        string PushPayload { get; set; }
    }



    // @protocol SINManagedPush <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINManagedPush {
        //[Wrap("WeakDelegate"), Abstract]
        //SINManagedPushDelegate Delegate { get; set; }

        // @required @property (readwrite, nonatomic, weak) id<SINManagedPushDelegate> delegate;
        [Abstract]
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // @required @property (assign, readwrite, nonatomic) UIUserNotificationType userNotificationTypes;
        [Abstract]
        [Export("userNotificationTypes", ArgumentSemantic.Assign)]
        UIUserNotificationType UserNotificationTypes { get; set; }

        // @required -(void)setDesiredPushType:(NSString *)pushType;
        [Abstract]
        [Export("setDesiredPushType:")]
        void SetDesiredPushType(string pushType);

        // @required -(void)setDesiredPushTypeAutomatically;
        [Abstract]
        [Export("setDesiredPushTypeAutomatically")]
        void SetDesiredPushTypeAutomatically();

        // @required -(void)registerUserNotificationSettings;
        [Abstract]
        [Export("registerUserNotificationSettings")]
        void RegisterUserNotificationSettings();

        // @required -(void)setDisplayName:(NSString *)displayName;
        [Abstract]
        [Export("setDisplayName:")]
        void SetDisplayName(string displayName);

        // @required -(void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken;
        [Abstract]
        [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
        void Application(UIApplication application, NSData deviceToken);

        // @required -(void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo;
        [Abstract]
        [Export("application:didReceiveRemoteNotification:")]
        void Application(UIApplication application, NSDictionary userInfo);
    }

    // @protocol SINManagedPushDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINManagedPushDelegate {
        // @required -(void)managedPush:(id<SINManagedPush>)managedPush didReceiveIncomingPushWithPayload:(NSDictionary *)payload forType:(NSString *)pushType;
        [Abstract]
        [Export("managedPush:didReceiveIncomingPushWithPayload:forType:")]
        void DidReceiveIncomingPushWithPayload(ISINManagedPush managedPush, NSDictionary payload, string pushType);
    }

    // @interface SINRemoteNotificationAdditions (NSDictionary)
    [Category]
    [BaseType(typeof(NSDictionary))]
    interface NSDictionary_SINRemoteNotificationAdditions {
        // -(BOOL)sin_isSinchPushPayload;
        [Static]
        [Export("sin_isSinchPushPayload")]
        bool Sin_isSinchPushPayload { get; }
    }

    // @interface SINPushHelper : NSObject
    [BaseType(typeof(NSObject))]
    interface SINPushHelper {
        // +(id<SINNotificationResult>)queryPushNotificationPayload:(NSDictionary *)userInfo;
        [Static]
        [Export("queryPushNotificationPayload:")]
        ISINNotificationResult QueryPushNotificationPayload(NSDictionary userInfo);
    }

    // @interface SINLocalNotification : NSObject
    [BaseType(typeof(NSObject))]
    interface SINLocalNotification {
        // @property (copy, nonatomic) NSString * alertBody;
        [Export("alertBody")]
        string AlertBody { get; set; }

        // @property (nonatomic) BOOL hasAction;
        [Export("hasAction")]
        bool HasAction { get; set; }

        // @property (copy, nonatomic) NSString * alertAction;
        [Export("alertAction")]
        string AlertAction { get; set; }

        // @property (copy, nonatomic) NSString * alertLaunchImage;
        [Export("alertLaunchImage")]
        string AlertLaunchImage { get; set; }

        // @property (copy, nonatomic) NSString * soundName;
        [Export("soundName")]
        string SoundName { get; set; }

        // @property (nonatomic) NSInteger applicationIconBadgeNumber;
        [Export("applicationIconBadgeNumber")]
        nint ApplicationIconBadgeNumber { get; set; }

        // @property (copy, nonatomic) NSString * category __attribute__((availability(ios, introduced=8_0)));
        [iOS(8, 0)]
        [Export("category")]
        string Category { get; set; }
    }

    // @interface SINLocalNotificationSinchAdditions (UILocalNotification)
    [Category]
    [BaseType(typeof(UILocalNotification))]
    interface UILocalNotification_SINLocalNotificationSinchAdditions {
        // -(BOOL)sin_isSinchNotification;
        [Static]
        [Export("sin_isSinchNotification")]
        bool Sin_isSinchNotification { get; }

        // -(BOOL)sin_isIncomingCall;
        [Static]
        [Export("sin_isIncomingCall")]
        bool Sin_isIncomingCall { get; }

        // -(BOOL)sin_isMissedCall;
        [Static]
        [Export("sin_isMissedCall")]
        bool Sin_isMissedCall { get; }
    }

    // @protocol SINNotificationResult <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINNotificationResult {
        // @required @property (readonly, assign, nonatomic) BOOL isValid;
        [Abstract]
        [Export("isValid")]
        bool IsValid { get; }

        // @required -(BOOL)isCall;
        [Abstract]
        [Export("isCall")]
        bool IsCall { get; }

        // @required -(id<SINCallNotificationResult>)callResult;
        [Abstract]
        [Export("callResult")]
        ISINCallNotificationResult CallResult { get; }

        // @required -(BOOL)isMessage;
        [Abstract]
        [Export("isMessage")]
        bool IsMessage { get; }

        // @required -(id<SINMessageNotificationResult>)messageResult;
        [Abstract]
        [Export("messageResult")]
        ISINMessageNotificationResult MessageResult { get; }
    }

    // @protocol SINCallNotificationResult <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINCallNotificationResult {
        // @required @property (readonly, assign, nonatomic) BOOL isTimedOut;
        [Abstract]
        [Export("isTimedOut")]
        bool IsTimedOut { get; }

        // @required @property (readonly, copy, nonatomic) NSString * remoteUserId;
        [Abstract]
        [Export("remoteUserId")]
        string RemoteUserId { get; }

        // @required @property (readonly, copy, nonatomic) NSString * callId;
        [Abstract]
        [Export("callId")]
        string CallId { get; }

        // @required @property (readonly, getter = isVideoOffered, nonatomic) BOOL videoOffered;
        [Abstract]
        [Export("videoOffered")]
        bool VideoOffered { [Bind("isVideoOffered")] get; }

        // @required @property (readonly, getter = isCallCanceled, nonatomic) BOOL callCanceled;
        [Abstract]
        [Export("callCanceled")]
        bool CallCanceled { [Bind("isCallCanceled")] get; }

        // @required @property (readonly, copy, nonatomic) NSDictionary * headers;
        [Abstract]
        [Export("headers", ArgumentSemantic.Copy)]
        NSDictionary Headers { get; }
    }

    // @protocol SINMessageNotificationResult <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SINMessageNotificationResult {
        // @required @property (readonly, copy, nonatomic) NSString * messageId;
        [Abstract]
        [Export("messageId")]
        string MessageId { get; }

        // @required @property (readonly, copy, nonatomic) NSString * senderId;
        [Abstract]
        [Export("senderId")]
        string SenderId { get; }
    }



    // @interface Sinch : NSObject
    [BaseType(typeof(NSObject))]
    interface Sinch {
        // +(id<SINClient>)clientWithApplicationKey:(NSString *)applicationKey environmentHost:(NSString *)environmentHost userId:(NSString *)userId;
        [Static]
        [Export("clientWithApplicationKey:environmentHost:userId:")]
        ISINClient ClientWithApplicationKey(string applicationKey, string environmentHost, string userId);

        // +(id<SINClient>)clientWithApplicationKey:(NSString *)applicationKey applicationSecret:(NSString *)applicationSecret environmentHost:(NSString *)environmentHost userId:(NSString *)userId;
        [Static]
        [Export("clientWithApplicationKey:applicationSecret:environmentHost:userId:")]
        ISINClient ClientWithApplicationKey(string applicationKey, string applicationSecret, string environmentHost, string userId);

        // +(id<SINClient>)clientWithApplicationKey:(NSString *)applicationKey environmentHost:(NSString *)environmentHost userId:(NSString *)userId cli:(NSString *)cli;
        [Static]
        [Export("clientWithApplicationKey:environmentHost:userId:cli:")]
        ISINClient ClientWithApplicationKeyCLI(string applicationKey, string environmentHost, string userId, string cli);

        // +(id<SINClient>)clientWithApplicationKey:(NSString *)applicationKey applicationSecret:(NSString *)applicationSecret environmentHost:(NSString *)environmentHost userId:(NSString *)userId cli:(NSString *)cli;
        [Static]
        [Export("clientWithApplicationKey:applicationSecret:environmentHost:userId:cli:")]
        ISINClient ClientWithApplicationKeyCLIAndSecret(string applicationKey, string applicationSecret, string environmentHost, string userId, string cli);

        // +(id<SINManagedPush>)managedPushWithAPSEnvironment:(SINAPSEnvironment)apsEnvironment;
        [Static]
        [Export("managedPushWithAPSEnvironment:")]
        ISINManagedPush ManagedPushWithAPSEnvironment(SINAPSEnvironment apsEnvironment);

        // +(NSString *)version;
        [Static]
        [Export("version")]
        string Version { get; }
    }
}
