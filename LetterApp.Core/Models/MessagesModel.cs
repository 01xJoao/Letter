﻿using System;
using Realms;

namespace LetterApp.Core.Models
{
    public class MessagesModel : RealmObject
    {
        public string MessageSenderId { get; set; }
        public long MessageId { get; set; }
        public string MessageData { get; set; }
        public int MessageType { get; set; }
        public long MessageDateTicks { get; set; }
        public string CustomData { get; set; }
        public bool IsFakeMessage { get; set; }
    }
}
