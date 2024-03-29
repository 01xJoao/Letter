﻿using System;

namespace LetterApp.Core.Models
{
    public class ChatListUserCellModel
    {
        public ChatListUserCellModel() {}

        public ChatListUserCellModel(int memberId, string memberName, string memberPhoto, string lastMessage, string lastMessageDate, bool isNewMessage, bool isMemberMuted, int unreadMessagesCount,
                                     EventHandler<int> openMemberProfile, EventHandler<int> openChat, DateTime lastMessageDateTime, MemberPresence memberPresence = MemberPresence.Offline)
        {
            MemberId = memberId;
            MemberName = memberName;
            MemberPhoto = memberPhoto;
            MemberPresence = memberPresence;
            LastMessage = lastMessage;
            LastMessageDate = lastMessageDate;
            IsNewMessage = isNewMessage;
            OpenMemberProfile = openMemberProfile;
            OpenChat = openChat;
            LastMessageDateTime = lastMessageDateTime;
            IsMemberMuted = isMemberMuted;
            UnreadMessagesCount = unreadMessagesCount;
        }

        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberPhoto { get; set; }
        public MemberPresence MemberPresence { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageDate { get; set; }
        public bool IsNewMessage { get; set; }
        public DateTime LastMessageDateTime { get; set; }
        public bool IsMemberMuted { get; set; }
        public int UnreadMessagesCount { get; set; }
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
