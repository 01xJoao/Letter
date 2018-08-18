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
        private SystemSound[] _systemSound = new SystemSound[(int)AudioTypes.Count];
        private TaskCompletionSource<bool> _callWaitingTask = new TaskCompletionSource<bool>();

        private bool _callActive; 
        private int _callWaitingCount;
        private int _callEndedCount;

        public AudioPlayerService()
        {
            _systemSound[(int)AudioTypes.CallReceiving] = new SystemSound(NSUrl.FromFilename("Audio/iphone_call.mp3"));
            _systemSound[(int)AudioTypes.CallWaiting] = new SystemSound(NSUrl.FromFilename("Audio/waiting_call.aiff"));
            _systemSound[(int)AudioTypes.CallEnded] = new SystemSound(1071);
            _systemSound[(int)AudioTypes.MessageReceivedInApp] = new SystemSound(1003);
            _systemSound[(int)AudioTypes.MessageReceivedOutApp] = new SystemSound(1004);
            _systemSound[(int)AudioTypes.MessageSend] = new SystemSound(1007);

            _systemSound[(int)AudioTypes.CallEnded].AddSystemSoundCompletion(CallEndedEvent);
            _systemSound[(int)AudioTypes.CallWaiting].AddSystemSoundCompletion(async () => await CallWaintingEvent());

        }

        public void CallReceiving()
        {
            _systemSound[(int)AudioTypes.CallReceiving].PlayAlertSound();
        }

        public Task<bool> CallWaiting()
        {
            _callActive = true;
            _systemSound[(int)AudioTypes.CallWaiting].PlaySystemSound();
            return _callWaitingTask.Task;
        }

        public void CallEnded()
        {
            _systemSound[(int)AudioTypes.CallEnded].PlaySystemSound();
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
                CallWaiting();
            }
            else
            {
                CleanCall();
                _callWaitingTask.TrySetResult(true);
            }
        }

        private void CallEndedEvent()
        {
            _callEndedCount++;

            Debug.WriteLine(_callEndedCount);

            if (_callEndedCount < 3)
                CallEnded();
            else
                StopAudio();
        }

        public void StopAudio()
        {
            _systemSound[(int)AudioTypes.CallReceiving].Close();
            _systemSound[(int)AudioTypes.CallWaiting].Close();

            _callWaitingTask.TrySetResult(false);

            _callWaitingTask = new TaskCompletionSource<bool>();

            _systemSound[(int)AudioTypes.CallReceiving] = new SystemSound(NSUrl.FromFilename("Audio/iphone_call.mp3"));
            _systemSound[(int)AudioTypes.CallWaiting] = new SystemSound(NSUrl.FromFilename("Audio/waiting_call.aiff"));

            _systemSound[(int)AudioTypes.CallWaiting].AddSystemSoundCompletion(async () => await CallWaintingEvent());

            Debug.WriteLine("Audio Stoped");

            CleanCall();
        }

        private void CleanCall()
        {
            _callActive = false;
            _callWaitingCount = 0;
            _callEndedCount = 0;
        }
    }
}
