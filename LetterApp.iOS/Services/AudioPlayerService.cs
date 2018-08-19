using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AudioToolbox;
using AVFoundation;
using Foundation;
using LetterApp.Core.Services.Interfaces;

namespace LetterApp.iOS.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private AVAudioSession _audioSession = AVAudioSession.SharedInstance();
        private AVAudioPlayer _audioSessionWaitingRing;
        private SystemSound[] _systemSound = new SystemSound[(int)AudioTypes.Count];
        private TaskCompletionSource<bool> _callWaitingTask = new TaskCompletionSource<bool>();

        private bool _callActive; 
        private int _callWaitingCount;

        public AudioPlayerService()
        {
            _systemSound[(int)AudioTypes.CallReceiving] = new SystemSound(NSUrl.FromFilename("Audio/iphone_call.mp3"));
            _systemSound[(int)AudioTypes.CallEnded] = new SystemSound(1071);
            _systemSound[(int)AudioTypes.MessageReceivedInApp] = new SystemSound(1003);
            _systemSound[(int)AudioTypes.MessageReceivedOutApp] = new SystemSound(1004);
            _systemSound[(int)AudioTypes.MessageSend] = new SystemSound(1007);

            _audioSessionWaitingRing = new AVAudioPlayer(NSUrl.FromFilename("Audio/waiting_call.aiff"), "aiff", out NSError err);
            _audioSessionWaitingRing.Volume = 1;
        }

        public void CallReceiving()
        {
            _systemSound[(int)AudioTypes.CallReceiving].PlayAlertSound();
        }

        public Task<bool> CallWaiting()
        {
            _callWaitingTask = new TaskCompletionSource<bool>();

            _callActive = true;
            CallWaintingEvent();

            return _callWaitingTask.Task;
        }

        public async Task CallEnded()
        {
            await _systemSound[(int)AudioTypes.CallEnded].PlaySystemSoundAsync();
            await _systemSound[(int)AudioTypes.CallEnded].PlaySystemSoundAsync();
            await _systemSound[(int)AudioTypes.CallEnded].PlaySystemSoundAsync();
        }

        public void MessageReceivedInApp()
        {
            _systemSound[(int)AudioTypes.MessageReceivedInApp].PlaySystemSound();
        }

        public void MessageReceivedOutApp()
        {
            _systemSound[(int)AudioTypes.MessageReceivedOutApp].PlayAlertSound();
        }

        public void MessageSend()
        {
            _systemSound[(int)AudioTypes.MessageSend].PlaySystemSound();
        }

        private async Task CallWaintingEvent()
        {
            await Task.Delay(1000);

            _callWaitingCount++;

            if (_callWaitingCount < 20 && _callActive)
            {
                _audioSessionWaitingRing.Play();
                CallWaintingEvent();
            }
            else
            {
                CleanCall();
                _callWaitingTask.TrySetResult(true);
            }
        }

        public void StopAudio()
        {
            _systemSound[(int)AudioTypes.CallReceiving].Close();
            _audioSessionWaitingRing.Stop();

            _callWaitingTask.TrySetResult(false);

            _systemSound[(int)AudioTypes.CallReceiving] = new SystemSound(NSUrl.FromFilename("Audio/iphone_call.mp3"));

            Debug.WriteLine("Audio Stoped");

            CleanCall();
        }

        private void CleanCall()
        {
            _callActive = false;
            _callWaitingCount = 0;
        }
    }
}
