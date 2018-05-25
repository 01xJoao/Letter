using System;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class DivisionModel : BaseModel
    {
        public int DivisionID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string UserCount { get; set; }
    }
}
