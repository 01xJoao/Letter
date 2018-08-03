using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DT.Xamarin.Agora;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.AgoraIO;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.AgoraIO;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Call
{
    public partial class CallViewController : XViewController<CallViewModel>
    {
        public override bool ShowAsPresentView => true;

        private CancellationTokenSource _timerCTS;
        private int _callTime;

        uint _localId = 0;
        uint _remoteId = 0;

        public AgoraRtcDelegate AgoraDelegate;
        public AgoraRtcEngineKit AgoraKit;

        private bool _audioMuted;
        public bool AudioMuted
        {
            get => _audioMuted;
            set
            {
                _audioMuted = value;

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

            _letterIconImage.Image = UIImage.FromBundle("letter_curved");
            _endCallImage.Image = UIImage.FromBundle("end_call");

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            if(ViewModel.MemberProfileModel != null)
                SetupView();
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
                    break;
                default:
                    break;
            }
        }

        private void SetupView()
        {
            if (PhoneModelExtensions.IsIphoneX())
                _nameLabelHeightConstraint.Constant = _nameLabelHeightConstraint.Constant + 30;

            UILabelExtensions.SetupLabelAppearance(_fullNameLabel, ViewModel.MemberFullName, Colors.White, 33f);
            UILabelExtensions.SetupLabelAppearance(_letterInfoLabel, ViewModel.LetterLabel, Colors.White, 16f);

            if (ViewModel.StartedCall)
            {
                CallStarted();
                UILabelExtensions.SetupLabelAppearance(_callDetailLabel, ViewModel.CallingLabel, Colors.White, 18f);
            }
            else
            {
                _speakerIcon.Hidden = true;
                _muteIcon.Hidden = true;

                _speakerBackgroundImage.Image = UIImage.FromBundle("decline_call");
                _muteBackgroundImage.Image = UIImage.FromBundle("accept_call");

                UILabelExtensions.SetupLabelAppearance(_speakerLabel, ViewModel.DeclineLabel, Colors.White, 12f);
                UILabelExtensions.SetupLabelAppearance(_muteLabel, ViewModel.AcceptLabel, Colors.White, 12f);
                UILabelExtensions.SetupLabelAppearance(_callDetailLabel, $"{ViewModel.MemberProfileModel.FirstName} {ViewModel.IsCallingLabel}", Colors.White, 18f);
            }

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
                }).ErrorPlaceholder("warning_image", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_pictureImage);


                _pictureImage.Image?.Dispose();
                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, ViewModel.MemberProfileModel.Picture);
                }).ErrorPlaceholder("warning_image", ImageSource.CompiledResource).Transform(new BlurredTransformation(3)).Into(_backgroundImage);

            }
        }

        private void CallStarted()
        {
            _speakerIcon.Hidden = false;
            _muteIcon.Hidden = false;

            _speakerIcon.Image = UIImage.FromBundle("speaker_off");
            _muteIcon.Image = UIImage.FromBundle("micro_on");

            _speakerBackgroundImage.Image = new UIImage();
            _muteBackgroundImage.Image = new UIImage();

            _speakerBackgroundImage.BackgroundColor = Colors.White.ColorWithAlpha(0.3f);
            _muteBackgroundImage.BackgroundColor = Colors.White.ColorWithAlpha(0.3f);

            CustomUIExtensions.RoundView(_speakerBackgroundImage);
            CustomUIExtensions.RoundView(_muteBackgroundImage);

            UILabelExtensions.SetupLabelAppearance(_speakerLabel, ViewModel.SpeakerLabel, Colors.White, 12f);
            UILabelExtensions.SetupLabelAppearance(_muteLabel, ViewModel.MuteLabel, Colors.White, 12f);

            UILabelExtensions.SetupLabelAppearance(_callDetailLabel, "", Colors.White, 16f, UIFontWeight.Semibold);
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
            //ChangeContent
            _remoteId = (uint)uid;
            RefreshDebug();
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

        public void StartUpdate()
        {
            if (_timerCTS != null) _timerCTS.Cancel();
            _timerCTS = new CancellationTokenSource();
            var ignore = UpdateAsync(_timerCTS.Token);
        }

        private async Task<object> UpdateAsync(CancellationToken token)
        {
            while (!_timerCTS.IsCancellationRequested)
            {
                _callDetailLabel.Text = TimeSpan.FromSeconds(_callTime++).ToString();
                await Task.Delay(1000, token);
            }
            return null;
        }

        public void StopUpdate()
        {
            if (_timerCTS != null) _timerCTS.Cancel();
            _timerCTS = null;
        }

        //public async Task UpdaterAsync(CancellationToken cts)
        //{
        //    while (!_timerCTS.IsCancellationRequested)
        //    {
        //        _nameLabel.Text = TimeSpan.FromSeconds(_callTime++).ToString();
        //        await Task.Delay(1000, cts);   
        //    }
        //}

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

