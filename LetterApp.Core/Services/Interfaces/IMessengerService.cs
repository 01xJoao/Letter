using System;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IMessengerService
    {
        void InitializeMessenger();
        void ConnectMessenger();
        void DisconnectMessenger();
        void RegisterMessengerToken();
    }
}
