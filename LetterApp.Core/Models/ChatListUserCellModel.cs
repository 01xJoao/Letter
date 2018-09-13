using System;

namespace LetterApp.Core.Models
{
    public class ChatListUserCellModel
    {
        public ChatListUserCellModel(int memberId, string memberName, string memberPhoto, string lastMessage, string lastMessageDate, bool shouldAlert, 
                                     EventHandler<int> openMemberProfile, EventHandler<int> openChat, DateTime lastMessageDateTime, MemberPresence memberPresence = MemberPresence.Offline)
        {
            MemberId = memberId;
            MemberName = memberName;
            MemberPhoto = memberPhoto;
            MemberPresence = memberPresence;
            LastMessage = lastMessage;
            LastMessageDate = lastMessageDate;
            ShouldAlert = shouldAlert;
            OpenMemberProfile = openMemberProfile;
            OpenChat = openChat;
            LastMessageDateTime = lastMessageDateTime;
        }

        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberPhoto { get; set; }
        public MemberPresence MemberPresence { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageDate { get; set; }
        public bool ShouldAlert { get; set; }
        public DateTime LastMessageDateTime { get; set; }
        public EventHandler<int> OpenMemberProfile { get; set; }
        public EventHandler<int> OpenChat { get; set; }
    }

    public enum MemberPresence
    {
        Online,
        Recent,
        Offline
    }
}
