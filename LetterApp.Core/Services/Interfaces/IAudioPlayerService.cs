using System;
using System.Threading.Tasks;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IAudioPlayerService
    {
        void CallReceiving();
        Task<bool> CallWaiting();
        void CallEnded();

        void MessageSend();
        void MessageReceivedInApp();
        void MessageReceivedOutApp();

        void StopAudio();
    }

    public enum AudioTypes
    {
        CallReceiving,
        CallWaiting,
        CallEnded,
        MessageReceivedInApp,
        MessageReceivedOutApp,
        MessageSend,
        Count
    }
}
