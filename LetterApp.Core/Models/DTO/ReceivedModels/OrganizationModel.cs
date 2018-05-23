using System;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class OrganizationModel
    {
        public int OrganizationID { get; set; }
        public int AdminID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ContactNumber { get; set; }
        public string LastUpdateTime { get; set; }
        public string Divisions { get; set; }
        public string Picture { get; set; }
        public string Address { get; set; }
    }
}
