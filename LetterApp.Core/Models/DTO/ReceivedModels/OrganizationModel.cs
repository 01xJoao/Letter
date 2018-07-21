using System;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class OrganizationModel : RealmObject
    {
        public int OrganizationID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ContactNumber { get; set; }
        public DateTimeOffset LastUpdatetime { get; set; }
        public string LastUpdateTicks { get; set; }
        public string Divisions { get; set; }
        public string Picture { get; set; }
        public string Address { get; set; }
        public int StatusCode { get; set; }
    }
}
