using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CallKit;
using DT.Xamarin.Agora;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.AgoraIO;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.AgoraIO;
using LetterApp.iOS.CallKit;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using SinchSdk;
using UIKit;

namespace LetterApp.iOS.Views.Call
{
    public partial class CallViewController : XViewController<CallViewModel>, ISINCallDelegate, ISINCallClientDelegate
    {
        public override bool ShowAsPresentView => true;

        private string _backgroundImg;
        private CancellationTokenSource _timerCTS;
        private int _callTime;

        uint _localId = 0;
        uint _remoteId = 0;

        public AgoraRtcDelegate AgoraDelegate;
        public AgoraRtcEngineKit AgoraKit;


        ISINCall call;
        public ISINCall Call
        {
            get => call;

            set
            {
                call = value;
                call.WeakDelegate = this;
            }
        }

        ISINClient Client
        {
            get
            {
                var appDelgate = (AppDelegate)UIApplication.SharedApplication.WeakDelegate;
                return appDelgate.Client;
            }
        }

        ProviderDelegate CallProvider
        {
            get
            {
                var appDelgate = (AppDelegate)UIApplication.SharedApplication.WeakDelegate;
                return appDelgate.CallProviderDelegate;
            }
        }

        public CallViewController() : base("CallViewController", null) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = Colors.Black;

            _speakerButton.TouchUpInside -= OnLeftButton_TouchUpInside;
            _speakerButton.TouchUpInside += OnLeftButton_TouchUpInside;

            _muteButton.TouchUpInside -= OnRightButton_TouchUpInside;
            _muteButton.TouchUpInside += OnRightButton_TouchUpInside;

            _endCallButton.TouchUpInside -= OnEndCallButton_TouchUpInside;
            _endCallButton.TouchUpInside += OnEndCallButton_TouchUpInside;

            UILabelExtensions.SetupLabelAppearance(_letterInfoLabel, ViewModel.LetterLabel, Colors.White, 16f);
            UILabelExtensions.SetupLabelAppearance(_callDetailLabel, ViewModel.ConnectingLabel, Colors.White.ColorWithAlpha(0.8f), 18f);

            _letterIconImage.Image = UIImage.FromBundle("letter_curved");
            _endCallImage.Image = UIImage.FromBundle("end_call");

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            if(ViewModel.MemberProfileModel != null)
                SetupView();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (ViewModel.StartedCall)
                ConfigureCallStarter();
        }


        private void ConfigureCallStarter()
        {
            Client.CallClient.CallUserWithId(ViewModel.CallerId.ToString());
            CallProvider.CallManager.StartCall(ViewModel.MemberFullName);
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.MemberProfileModel):
                    SetupView();
                    break;
                case nameof(ViewModel.InCall):
                    CallStarted();
                    SetupAgoraIO();
                    break;
                default:
                    break;
            }
        }

        private void SetupView()
        {
            if (PhoneModelExtensions.IsIphoneX())
                _nameLabelHeightConstraint.Constant = _nameLabelHeightConstraint.Constant + 30;

            int nameSize = 33;

            if(PhoneModelExtensions.IsSmallIphone())
            {
                nameSize = 25;
                _callDetailsHeightConstraint.Constant = 20;
            }

            UILabelExtensions.SetupLabelAppearance(_fullNameLabel, ViewModel.MemberFullName, Colors.White, nameSize);

            if (ViewModel.StartedCall)
            {
                CallStarted();
                _callDetailLabel.Text = ViewModel.CallingLabel;
            }
            else
            {
                ViewModel.RightButtonCommand.Execute();
            }
            //else
            //{
            //    _speakerIcon.Hidden = true;
            //    _muteIcon.Hidden = true;

            //    _speakerBackgroundImage.Image = UIImage.FromBundle("decline_call");
            //    _muteBackgroundImage.Image = UIImage.FromBundle("accept_call");

            //    UILabelExtensions.SetupLabelAppearance(_speakerLabel, ViewModel.DeclineLabel, Colors.White, 12f);
            //    UILabelExtensions.SetupLabelAppearance(_muteLabel, ViewModel.AcceptLabel, Colors.White, 12f);

            //    _callDetailLabel.Text = $"{ViewModel.MemberProfileModel.FirstName} {ViewModel.IsCallingLabel}";
            //}

            if(string.IsNullOrEmpty(ViewModel.MemberProfileModel.Picture))
            {
                _pictureImage.Hidden = true;
                _backgroundImage.Hidden = true;
            }
            else
            {
                _pictureImage.Image?.Dispose();
                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, ViewModel.MemberProfileModel.Picture);
                }).ErrorPlaceholder("letter_round_big", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_pictureImage);

                _backgroundImg = string.Copy(ViewModel.MemberProfileModel.Picture);

                _backgroundImage.Image?.Dispose();
                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, _backgroundImg);
                }).ErrorPlaceholder("letter_round_big", ImageSource.CompiledResource).Transform(new BlurredTransformation(14f)).Into(_backgroundImage);
            }

            if(ViewModel.StartedCall)
                SetupAgoraIO();
        }

        private void CallStarted()
        {
            _speakerIcon.Hidden = false;
            _muteIcon.Hidden = false;

            _speakerIcon.Image = ViewModel.SpeakerOn ? UIImage.FromBundle("speaker_on") : UIImage.FromBundle("speaker_off");
            _muteIcon.Image = ViewModel.MutedOn ? UIImage.FromBundle("micro_off") : UIImage.FromBundle("micro_on");

            _speakerBackgroundImage.Image = new UIImage();
            _muteBackgroundImage.Image = new UIImage();

            _speakerBackgroundImage.BackgroundColor = Colors.White.ColorWithAlpha(0.3f);
            _muteBackgroundImage.BackgroundColor = Colors.White.ColorWithAlpha(0.3f);

            CustomUIExtensions.RoundView(_speakerBackgroundImage);
            CustomUIExtensions.RoundView(_muteBackgroundImage);

            UILabelExtensions.SetupLabelAppearance(_speakerLabel, ViewModel.SpeakerLabel, Colors.White, 12f);
            UILabelExtensions.SetupLabelAppearance(_muteLabel, ViewModel.MuteLabel, Colors.White, 12f);

            _callDetailLabel.Text = "";

            if (AgoraDelegate == null)
                SetupAgoraIO();
        }

        private void SetupAgoraIO()
        {
            AgoraDelegate = new AgoraRtcDelegate(this);
            AgoraKit = AgoraRtcEngineKit.SharedEngineWithAppIdAndDelegate(AgoraSettings.AgoraAPI, AgoraDelegate);
            AgoraKit.SetChannelProfile(ChannelProfile.Communication);
            AgoraKit.JoinChannelByToken(AgoraSettings.AgoraAPI, ViewModel.RoomName, null, 0, JoiningCompleted);

            AgoraKit.SetEnableSpeakerphone(ViewModel.SpeakerOn);
            AgoraKit.MuteLocalAudioStream(ViewModel.MutedOn);
        }

        private void OnLeftButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.LeftButtonCommand.Execute();

            if(ViewModel.StartedCall || ViewModel.InCall)
            {
                AgoraKit?.SetEnableSpeakerphone(ViewModel.SpeakerOn);
                _speakerIcon.Image = ViewModel.SpeakerOn ? UIImage.FromBundle("speaker_on") : UIImage.FromBundle("speaker_off");
            }
        }

        private void OnRightButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.RightButtonCommand.Execute();

            if (ViewModel.StartedCall || ViewModel.InCall)
            {
                AgoraKit?.MuteLocalAudioStream(ViewModel.MutedOn);
                _muteIcon.Image = ViewModel.MutedOn ? UIImage.FromBundle("micro_off") : UIImage.FromBundle("micro_on");
            }
        }

        private void OnEndCallButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.EndCallCommand.CanExecute())
                ViewModel.EndCallCommand.Execute();
        }

        private void JoiningCompleted(NSString channel, nuint uid, nint elapsed)
        {
            _localId = (uint)uid;
            UIApplication.SharedApplication.IdleTimerDisabled = true;
            RefreshDebug();
        }

        public void DidEnterRoom(AgoraRtcEngineKit engine, nuint uid, nint elapsed)
        {
            ViewModel.StopAudioCommand.Execute();

            Call?.Hangup();

            _remoteId = (uint)uid;
            RefreshDebug();

            StartTimer();
        }

        public void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            if(ViewModel.EndCallCommand.CanExecute())
                ViewModel.EndCallCommand.Execute();
        }

        private void RefreshDebug()
        {
            Debug.WriteLine($"local: {_localId}\nremote: {_remoteId}");
        }

        public void StartTimer()
        {
            _timerCTS?.Cancel();
            _timerCTS = new CancellationTokenSource();

            var ignore = UpdateAsync(_timerCTS.Token);
        }

        private async Task<object> UpdateAsync(CancellationToken token)
        {
            while (!_timerCTS.IsCancellationRequested)
            {
                _callDetailLabel.Text = TimeSpan.FromSeconds(_callTime++).ToString();

                try
                {
                    await Task.Delay(1000, token);
                }
                catch (Exception ex) {}
            }

            return null;
        }

        public void StopTimer()
        {
            _timerCTS?.Cancel();
            _timerCTS = null;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UIDevice.CurrentDevice.ProximityMonitoringEnabled = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            UIApplication.SharedApplication.IdleTimerDisabled = false;
            UIDevice.CurrentDevice.ProximityMonitoringEnabled = false;
            StopTimer();

            AgoraKit?.LeaveChannel(null);
            AgoraKit?.Dispose();
            AgoraKit = null;
            _backgroundImg = null;

            CallProvider.CallManager.EndCall();

            Call?.Hangup();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (this.IsMovingFromParentViewController)
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }

        [Export("callDidEnd:")]
        void CallDidEnd(ISINCall xcall)
        {
        }
    }
}

