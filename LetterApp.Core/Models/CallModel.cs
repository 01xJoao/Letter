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
        public bool CallType { get; set; }
        public bool Success { get; set; }
    }

    public enum CallType
    {
        Outgoing,
        Incoming
    } 
}
