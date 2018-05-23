using System;
using System.Collections.Generic;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class OrganizationAccessModel : BaseModel
    {
        public int OrganizationID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool RequiresAccessCode { get; set; }
        public bool RequiresUserDomain { get; set; }
        public List<DivisionModel> Divisions {get;set;}
    }
}
