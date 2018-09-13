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
        public string LastMessageDateTimeTicks { get; set; }
    }
}
