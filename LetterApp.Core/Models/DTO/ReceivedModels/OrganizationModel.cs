using System;
using System.Collections.Generic;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class OrganizationModel : RealmObject
    {
        [PrimaryKey]
        public int OrganizationID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ContactNumber { get; set; }
        public DateTimeOffset LastUpdatetime { get; set; }
        public int LastUpdateTicks { get; set; }
        public IList<DivisionModel> Divisions { get; }
        public string Picture { get; set; }
        public string Address { get; set; }
        public int StatusCode { get; set; }
    }

    public class OrganizationReceivedModel : BaseModel
    {
        public int OrganizationID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ContactNumber { get; set; }
        public DateTimeOffset LastUpdatetime { get; set; }
        public int LastUpdateTicks { get; set; }
        public List<DivisionModel> Divisions { get; set; }
        public string Picture { get; set; }
        public string Address { get; set; }
    }
}
