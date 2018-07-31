using System;
using Realms;

namespace LetterApp.Core.Models.DTO.ReceivedModels
{
    public class MembersProfileModel : RealmObject
    {
        [PrimaryKey]
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string ContactNumber { get; set; }
        public bool ShowContactNumber { get; set; }
        public string Divisions { get; }
        public string Picture { get; set; }
        public DateTimeOffset UserLastSeen { get; set; }
        public DateTimeOffset TimeCreated { get; set; }
        public int StatusCode { get; set; }
    }
}
