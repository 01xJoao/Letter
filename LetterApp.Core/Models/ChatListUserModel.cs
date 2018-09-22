using System;
using System.Collections.Generic;

namespace LetterApp.Core.Models
{
    public class ChatListUserModel : Realms.RealmObject
    {
        [Realms.PrimaryKey]
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberPhoto { get; set; }
        public string LastMessage { get; set; }
        public long LastMessageDateTimeTicks { get; set; }
        public bool ShouldAlertNewMessage { get; set; }
        public bool IsMemeberMuted { get; set; }
        public bool IsArchived { get; set; }
        public long ArchivedTime { get; set; }
        public int MemberPresence { get; set; }
        public long MemberPresenceConnectionDate { get; set; }
        public int UnreadMessagesCount { get; set; }
        public IList<MessagesModel> MessagesList { get; }
    }
}
