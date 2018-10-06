using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SendBird;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IMessengerService
    {
        void InitializeMessenger();
        Task<bool> ConnectMessenger();
        void DisconnectMessenger();
        void RegisterMessengerToken();
        Task<GroupChannel> GetUsersInChannel(string channelUrl);
        Task<GroupChannel> CreateChannel(List<string> users);
        Task<List<GroupChannel>> GetChannels(List<string> users);
        Task<List<GroupChannel>> GetAllChannels();
        Task<GroupChannel> GetCurrentChannel(string userId);
        Task<UserMessage> SendMessage(GroupChannel channel, string userId, string userName, string memberToken, string message, string date);
        Task<List<User>> CheckUsersInGroupPresence(GroupChannel channel);
        Task<List<BaseMessage>> LoadMessages(PreviousMessageListQuery channelQuery, int messagesCount);
        void RemoveChannel(GroupChannel channel);
        void TypingMessage(GroupChannel channel);
        void TypingMessageEnded(GroupChannel channel);
        void MarkMessageAsRead(GroupChannel channel);
        void SubscribeToNotifications();
        void SendPushNotification(string userId, string userName, string memberTokenId, string message, bool isCall);
    }
}
