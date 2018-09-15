using System;

namespace LetterApp.Core.Models
{
    public class ChatListUserModel : Realms.RealmObject
    {
        [Realms.PrimaryKey]
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberPhoto { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageDate { get; set; }
        public bool ShouldAlert { get; set; }
        public long LastMessageDateTimeTicks { get; set; }
        public bool IsMemeberMuted { get; set; }
        public int MemberPresence { get; set; }
        public long MemberPresenceConnectionDate { get; set; }
        public long LastTimeChatWasOpen { get; set; }
    }
}
