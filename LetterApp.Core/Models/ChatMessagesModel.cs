using System;

namespace LetterApp.Core.Models
{
    public class ChatMessagesModel
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public string MessageSenderId { get; set; }
        public long MessageId { get; set; }
        public string MessageData { get; set; }
        public MessageType MessageType { get; set; }
        public string MessageDate { get; set; }
        public DateTime MessageDateTime { get; set; }
        public string CustomData { get; set; }
        public PresentMessageType PresentMessage { get; set; }
    }

    public enum MessageType 
    {
        Text,
        Image,
        File
    }

    public enum PresentMessageType 
    {
        UserText,
        UserImage,
        UserFile,
        Text,
        Image,
        File
    }
}
