using System;
using System.Collections.Generic;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class CheckUserModel : BaseModel
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string ContactNumber { get; set; }
        public string Picture { get; set; }
        public bool ShowContactNumber { get; set; }
        public int? OrganizationID { get; set; }
        public DateTimeOffset LastUpdateTime { get; set; }
        public List<DivisionModel> Divisions { get; set; }
    }
}