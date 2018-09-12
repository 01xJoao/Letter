using System;
namespace LetterApp.Core.Models
{
    public class ChatListUserModel : Realms.RealmObject
    {
        public ChatListUserModel(string memberName, string memberPhoto, string lastMessage, string lastMessageDate, bool shouldAlert, string lastMessageDateTimeTicks)
        {
            MemberName = memberName;
            MemberPhoto = memberPhoto;
            LastMessage = lastMessage;
            LastMessageDate = lastMessageDate;
            ShouldAlert = shouldAlert;
            LastMessageDateTimeTicks = lastMessageDateTimeTicks;
        }

        public string MemberName { get; set; }
        public string MemberPhoto { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageDate { get; set; }
        public bool ShouldAlert { get; set; }
        public string LastMessageDateTimeTicks { get; set; }
    }
}
