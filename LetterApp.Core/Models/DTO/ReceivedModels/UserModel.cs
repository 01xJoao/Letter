using System;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class UserModel : RealmObject
    {
        [PrimaryKey]
        public int UserId { get; set; }
        public string Email { get; set; }
        public string AuthToken { get; set; }
        [Indexed]
        public string FirstName { get; set; }
        [Indexed]
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        [Indexed]
        public bool ShowContactNumber { get; set; }
        [Indexed]
        public string Position { get; set; }
        public string Picture { get; set; }
        public string PubNubToken { get; set; }
        public string TimeCreated { get; set; }
        public int StatusCode { get; set; }
    }
}
