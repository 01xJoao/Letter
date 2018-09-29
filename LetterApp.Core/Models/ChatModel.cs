using System;
using System.Collections.Generic;

namespace LetterApp.Core.Models
{
    public class ChatModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberDetails { get; set; }
        public string MemberPhoto { get; set; }
        public string MemberEmail { get; set; }
        public bool MemberMuted { get; set; }
        public bool MemberArchived { get; set; }
        public bool MemberSeenMyLastMessage { get; set; }
        public MemberPresence MemberPresence { get; set; }
        public List<ChatMessagesModel> Messages { get; set; }
        public EventHandler<long> MessageEvent { get; set; }
        public Dictionary<int,Tuple<string,int>> SectionsAndRowsCount { get; set; }
    }
}
