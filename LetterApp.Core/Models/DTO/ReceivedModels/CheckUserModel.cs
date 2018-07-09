using System;
using System.Collections.Generic;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class CheckUserModel : BaseModel
    {
        public int? UserID { get; set; }
        public string Position { get; set; }
        public int? OrganizationID { get; set; }
        public List<DivisionModel> Divisions { get; set; }
    }
}