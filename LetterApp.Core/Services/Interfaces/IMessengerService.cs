using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SendBird;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IMessengerService
    {
        void InitializeMessenger();
        void ConnectMessenger();
        void DisconnectMessenger();
        void RegisterMessengerToken();

        Task<GroupChannel> CreateChannel(List<string> users);
        Task<List<GroupChannel>> GetChannels(List<string> users);
        Task<List<GroupChannel>> GetAllChannels();
        Task<GroupChannel> GetCurrentChannel(string userId);
        Task<UserMessage> SendMessage(Tuple<GroupChannel, string> message);
    }
}
