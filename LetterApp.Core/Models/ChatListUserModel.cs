using System;

namespace LetterApp.Core.Models
{
    public class ChatListUserModel : Realms.RealmObject
    {
        public ChatListUserModel(int memberId, string memberName, string memberPhoto, string lastMessage, string lastMessageDate, bool shouldAlert, string lastMessageDateTimeTicks)
        {
            MemberId = memberId;
            MemberName = memberName;
            MemberPhoto = memberPhoto;
            LastMessage = lastMessage;
            LastMessageDate = lastMessageDate;
            ShouldAlert = shouldAlert;
            LastMessageDateTimeTicks = lastMessageDateTimeTicks;
        }

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
