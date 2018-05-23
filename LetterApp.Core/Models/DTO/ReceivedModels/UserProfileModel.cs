using System;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class UserProfileModel : BaseModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string ContactNumber { get; set; }
        public bool ShowContactNumber { get; set; }
        public string Divisions { get; set; }
        public string Picture { get; set; }
        public string UserLastSeen { get; set; }
        public string TimeCreated { get; set; }
        public string LastUpdateTime { get; set; }
    }
}
