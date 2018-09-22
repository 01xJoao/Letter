using System;

namespace LetterApp.Core.Models
{
    public class ChatMessagesModel
    {
        public int MessageSenderId { get; set; }
        public int MessageId { get; set; }
        public string MessageData { get; set; }
        public MessageType MessageType { get; set; }
        public long MessageDateTicks { get; set; }
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
