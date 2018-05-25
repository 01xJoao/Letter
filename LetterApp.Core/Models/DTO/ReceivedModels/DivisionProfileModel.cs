using System;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class DivisionProfileModel : BaseModel
    {
        public int DivisionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int NumberUsers { get; set; }
        public string AccessCode { get; set; }
        public string Picture { get; set; }
        public string ContactNumber { get; set; }
        public string LastUpdateTime { get; set; }
        public int OrgID { get; set; }
        public string OrgName { get; set; }
        public string OrgPic { get; set; }
        public int OrgAdminID { get; set; }
    }
}
