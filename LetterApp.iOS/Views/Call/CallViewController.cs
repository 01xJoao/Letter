using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
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

        private string _backgroundImg;
        private int _callTime;
        private CancellationTokenSource _timerCTS;

        public AgoraRtcDelegate AgoraDelegate { get; set; }

        private ActiveCall _activeCall;
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
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (ViewModel.MemberProfileModel != null)
                SetupView();
            
            if (ViewModel.StartedCall)
                _activeCall = CallProvider.CallManager.StartCall(ViewModel.MemberFullName, ViewModel.CallerId);
            else
            {
                _activeCall = CallProvider.CallManager.Calls.LastOrDefault();

                if(_activeCall == null)
                {
                    CallProvider.AgoraCallEnded();

                    if (ViewModel.EndCallCommand.CanExecute())
                        ViewModel.EndCallCommand.Execute();
                }
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.MemberProfileModel):
                    SetupView();
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

            CallStarted();
            _callDetailLabel.Text = ViewModel.CallingLabel;

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
                SetAgoraIO();
        }

        private async Task SetAgoraIO()
        {
            AgoraDelegate = new AgoraRtcDelegate(this);
            bool callIsActive = await CallProvider.SetupAgoraIO(AgoraDelegate, ViewModel.RoomName, ViewModel.SpeakerOn, ViewModel.MutedOn);

            if(!callIsActive)
            {
                CallProvider.AgoraCallEnded();

                if (ViewModel.EndCallCommand.CanExecute())
                    ViewModel.EndCallCommand.Execute();
            }
        }

        private void OnLeftButton_TouchUpInside(object sender, EventArgs e)
        {
            //ViewModel.LeftButtonCommand.Execute();

            CallProvider.AgoraSetSpeaker(!ViewModel.SpeakerOn);
            //_speakerIcon.Image = ViewModel.SpeakerOn ? UIImage.FromBundle("speaker_on") : UIImage.FromBundle("speaker_off");
        }

        private void OnRightButton_TouchUpInside(object sender, EventArgs e)
        {
            //ViewModel.RightButtonCommand.Execute();

            _muteIcon.Image = !ViewModel.MutedOn ? UIImage.FromBundle("micro_off") : UIImage.FromBundle("micro_on");
            CallProvider.AgoraSetMute(!ViewModel.MutedOn);
            //_muteIcon.Image = ViewModel.MutedOn ? UIImage.FromBundle("micro_off") : UIImage.FromBundle("micro_on");
        }

        public void AudioMuted(bool muted)
        {
            ViewModel.RightButtonCommand.Execute(muted);
        }

        private void OnEndCallButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.EndCallCommand.CanExecute())
            {
                CallProvider.AgoraCallEnded();
                CallProvider.CallManager.EndCall(_activeCall);
                _activeCall = null;
                ViewModel.EndCallCommand.Execute();
            }
        }

        public void DidEnterRoom()
        {
            CallProvider.AgoraCallStarted();
            StartTimer();
        }

        public void DidOfflineOfUid()
        {
            CallProvider.AgoraCallEnded();

            if(ViewModel.EndCallCommand.CanExecute())
                ViewModel.EndCallCommand.Execute();
        }

        public void UserEndedCallOnCallKitScreen()
        {
            if(CallProvider.CallManager.Calls.Count > 0)
                CallProvider.CallManager.Calls.Remove(_activeCall);

            _activeCall = null;

            if (ViewModel.EndCallCommand.CanExecute())
                ViewModel.EndCallCommand.Execute();
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
                catch (Exception ex){}
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
            UIDevice.CurrentDevice.ProximityMonitoringEnabled = false;

            StopTimer();
            _backgroundImg = null;

            if(_activeCall != null)
                CallProvider.CallManager.EndCall(_activeCall);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (this.IsMovingFromParentViewController)
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

