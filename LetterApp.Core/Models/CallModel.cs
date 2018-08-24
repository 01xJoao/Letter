using System;
using Realms;

namespace LetterApp.Core.Models
{
    public class CallModel : RealmObject
    {
        [PrimaryKey]
        public int CallId { get; set; }
        public int CallerId { get; set; }
        public string CallerName { get; set; }
        public string CallerPosition { get; set; }
        public string CallerPicture { get; set; }
        public long CallDate { get; set; }
        public long CallDuration { get; set; }
        public string CallType { get; set; }
    }

    public enum CallType
    {
        Outgoing,
        Incoming,
        Missed
    } 
}
