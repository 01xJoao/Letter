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

        Task<Tuple<BaseChannel, BaseMessage>> InitializeHandlers();
        Task<GroupChannel> CreateChannel(List<string> users);
        Task<List<GroupChannel>> GetChannels(List<string> users);
        Task<List<GroupChannel>> GetAllChannels();
        Task<GroupChannel> GetCurrentChannel(string userId);
        Task<UserMessage> SendMessage(GroupChannel channel, string message);
        Task<List<User>> CheckUsersInGroupPresence(GroupChannel channel);
        void TypingMessage(GroupChannel channel);
        void TypingMessageEnded(GroupChannel channel);
        void MarkMessageAsRead(GroupChannel channel);
    }
}
