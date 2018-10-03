using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using AVFoundation;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.AgoraIO;
using LetterApp.iOS.CallKit;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Call
{
    public partial class CallViewController : XViewController<CallViewModel>
    {
        public override bool ShowAsPresentView => true;
        public AgoraRtcDelegate AgoraDelegate { get; set; }

        private string _backgroundImg;
        private ActiveCall _activeCall;
        private int _callTime;
        private Timer _callTimer = new Timer(1000);

        private ProviderDelegate CallProvider
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

            //var audioSession = AVAudioSession.SharedInstance();
            //audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            //audioSession.SetActive(true);

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

            _callTimer.Elapsed += (s, e) => UpdateTime();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.MemberProfileModel):
                    if (_activeCall == null)
                        SetupView();
                    break;

                case nameof(ViewModel.EndCallForSettings):
                    EndCall();
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

            if (PhoneModelExtensions.IsSmallIphone())
            {
                nameSize = 25;
                _callDetailsHeightConstraint.Constant = 20;
            }

            UILabelExtensions.SetupLabelAppearance(_fullNameLabel, ViewModel.MemberFullName, Colors.White, nameSize);

            if (ViewModel.StartedCall)
                _callDetailLabel.Text = ViewModel.CallingLabel;

            _pictureImage.Image?.Dispose();
            _backgroundImage.Image?.Dispose();

            if (string.IsNullOrEmpty(ViewModel.MemberProfileModel.Picture))
            {
                _backgroundImage.BackgroundColor = Colors.BlueSetup;
                _pictureImage.Image = UIImage.FromBundle("profile_noimage");
            }
            else
            {
                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, ViewModel.MemberProfileModel.Picture);
                }).ErrorPlaceholder("letter_round_big", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_pictureImage);

                _backgroundImg = string.Copy(ViewModel.MemberProfileModel.Picture);

                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, _backgroundImg);
                }).ErrorPlaceholder("letter_round_big", ImageSource.CompiledResource).Transform(new BlurredTransformation(14f)).Into(_backgroundImage);
            }

            StartCall();
        }

        private void StartCall()
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

            AgoraDelegate = new AgoraRtcDelegate(this);
            CallProvider.SetupAgoraIO(this, ViewModel.RoomName, ViewModel.SpeakerOn, ViewModel.MutedOn, ViewModel.StartedCall);

            _activeCall = ViewModel.StartedCall
                ? CallProvider.CallManager.StartCall(ViewModel.MemberFullName, ViewModel.CallerId)
                : CallProvider.CallManager.Calls.LastOrDefault();
        }

        private void OnLeftButton_TouchUpInside(object sender, EventArgs e)
        {
            CallProvider.AgoraSetSpeaker(!ViewModel.SpeakerOn);
            ViewModel.LeftButtonCommand.Execute();
            _speakerIcon.Image = ViewModel.SpeakerOn ? UIImage.FromBundle("speaker_on") : UIImage.FromBundle("speaker_off");
        }

        private void OnRightButton_TouchUpInside(object sender, EventArgs e)
        {
            CallProvider.AgoraSetMute(!ViewModel.MutedOn, true);
        }

        public void AudioMuted(bool value)
        {
            ViewModel.RightButtonCommand.Execute(value);

            if (NSThread.Current.IsMainThread)
                _muteIcon.Image = value ? UIImage.FromBundle("micro_off") : UIImage.FromBundle("micro_on");
        }

        private void OnEndCallButton_TouchUpInside(object sender, EventArgs e)
        {
            EndCall();
        }

        private void EndCall()
        {
            CallProvider.CallManager.EndCall(_activeCall);
            _activeCall = null;
        }

        public void DidOfflineOfUid()
        {
            CallProvider.CallManager.EndCall(_activeCall);
        }

        public void UserEndedCall()
        {
            if (ViewModel.EndCallCommand.CanExecute())
                ViewModel.EndCallCommand.Execute();
        }

        public void JoinCompleted()
        {
            //if(ViewModel.StartedCall)
                //PlayAudio("ringback.wav", 100);
        }

        public async Task DidEnterRoom()
        {
            //if (ViewModel.StartedCall)
                //players.LastOrDefault()?.Stop();

            _callTimer.Start();

            if (!ViewModel.StartedCall)
                await Task.Delay(TimeSpan.FromSeconds(1));

            CallProvider.AgoraCallStarted();
        }

        #region Views

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UIDevice.CurrentDevice.ProximityMonitoringEnabled = true;

            if (ViewModel.MemberProfileModel != null)
                SetupView();
        }

        public override void ViewWillDisappear(bool animated)
        {
            //players.LastOrDefault()?.Stop();
            PlayAudio("EndCallSound.wav", 0);
            _callTimer.Stop();

            _callTimer = null;
            AgoraDelegate = null;
            _backgroundImg = null;
            _activeCall = null;

            UIDevice.CurrentDevice.ProximityMonitoringEnabled = false;
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            ViewModel.MicrophoneAlertCommand.Execute();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (this.IsMovingFromParentViewController)
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }

        #endregion

        #region Timers

        private void UpdateTime()
        {
            BeginInvokeOnMainThread(() => {
                _callDetailLabel.Text = TimeSpan.FromSeconds(_callTime++).ToString();
            });
        }

        #endregion

        #region Helpers

        Queue<AVAudioPlayer> players = new Queue<AVAudioPlayer>(1);

        void PlayAudio(string fileName, int loops)
        {
            NSUrl url = NSUrl.FromFilename(fileName);
            AVAudioPlayer player = AVAudioPlayer.FromUrl(url);
            player.NumberOfLoops = 0;
            player.Volume = 1;

            if (players.Count == 1)
                players.Dequeue().Dispose();

            players.Enqueue(player);

            player.Play();
        }

        #endregion
    }
}

