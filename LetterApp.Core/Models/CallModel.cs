using System;
using Realms;

namespace LetterApp.Core.Models
{
    public class CallModel : RealmObject
    {
        [PrimaryKey]
        public int CallId { get; set; }
        public int CallerId { get; set; }
        public long CallDate { get; set; }
        public int CallType { get; set; }
        public bool Success { get; set; }
        public bool IsNew { get; set; }
    }

    public enum CallType
    {
        Outgoing,
        Incoming
    } 
}
