using System;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class DivisionModel : RealmObject
    {
        [PrimaryKey]
        public int DivisionID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string UserCount { get; set; }
        public bool IsDivisonActive { get; set; }
        public bool IsUserInDivisionActive { get; set; }
        public bool IsUnderReview { get; set; }
        [Indexed]
        public bool IsUserAdmin { get; set; }
        public DateTimeOffset LastUpdate { get; set; }
        public int? StatusCode { get; set; }
    }
}
