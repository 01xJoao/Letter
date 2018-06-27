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
        [Indexed]
        public string FirstName { get; set; }
        [Indexed]
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        [Indexed]
        public bool ShowContactNumber { get; set; }
        [Indexed]
        public string Position { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string TimeCreated { get; set; }
        public IList<DivisionModel> Divisions;
        public int? OrganizationID { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool IsUserActive { get; set; }
        public int? StatusCode { get; set; }
    }
}
