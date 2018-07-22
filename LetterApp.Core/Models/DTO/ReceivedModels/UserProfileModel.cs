using System;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class UserProfileModel : RealmObject
    {
        [PrimaryKey]
        public int UserID { get; set; }
        [Indexed]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string ContactNumber { get; set; }
        public bool ShowContactNumber { get; set; }
        public string Divisions { get; set; }
        public string Picture { get; set; }
        public string UserLastSeen { get; set; }
        public DateTimeOffset TimeCreated { get; set; }
        public DateTimeOffset LastUpdateTime { get; set; }
        public int StatusCode { get; set; }
    }
}
