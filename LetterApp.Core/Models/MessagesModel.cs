using System;
using Realms;

namespace LetterApp.Core.Models
{
    public class MessagesModel : RealmObject
    {
        public int MessageSenderId { get; set; }
        public string MessageData { get; set; }
        public int MessageType { get; set; }
        public long MessageDateTicks { get; set; }
        public string CustomData { get; set; }
    }
}
