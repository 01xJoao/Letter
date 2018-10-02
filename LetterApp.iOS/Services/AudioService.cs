using System;
using AudioToolbox;
using AVFoundation;
using Foundation;
using LetterApp.Core.Services.Interfaces;
using Xamarin.Essentials;

namespace LetterApp.iOS.Services
{
    public class AudioService : IAudioService
    {
        private AVAudioPlayer receiveMessagePlayer;
        private AVAudioPlayer sentMessagePlayer;

        public void PlayReceivedMessage()
        {
            if (DeviceInfo.DeviceType != DeviceType.Physical)
                return;

            var audioSession = AVAudioSession.SharedInstance();

            audioSession.SetCategory(AVAudioSessionCategory.SoloAmbient);
            audioSession.SetActive(true);

            if(receiveMessagePlayer == null)
                receiveMessagePlayer = new AVAudioPlayer(new NSUrl("/System/Library/Audio/UISounds/ReceivedMessage.caf"), "caf", out NSError err);

            receiveMessagePlayer.Volume = audioSession.OutputVolume;
            receiveMessagePlayer.Play();
        }

        public void PlaySendMessage()
        {
            if (DeviceInfo.DeviceType != DeviceType.Physical)
                return;

            var audioSession = AVAudioSession.SharedInstance();

            audioSession.SetCategory(AVAudioSessionCategory.SoloAmbient);
            audioSession.SetActive(true);

            if (sentMessagePlayer == null)
                sentMessagePlayer = new AVAudioPlayer(new NSUrl("/System/Library/Audio/UISounds/SentMessage.caf"), "caf", out NSError err);

            sentMessagePlayer.Volume = audioSession.OutputVolume;
            sentMessagePlayer.Play();
        }
    }
}
