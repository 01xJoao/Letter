using System;
using System.Collections.Generic;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class UserModel : RealmObject
    {
        [PrimaryKey]
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public bool ShowContactNumber { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public DateTimeOffset TimeCreated { get; set; }
        public long LastUpdateTime { get; set; }
        public IList<DivisionModel> Divisions { get; }
        public int? OrganizationID { get; set; }
    }
}
