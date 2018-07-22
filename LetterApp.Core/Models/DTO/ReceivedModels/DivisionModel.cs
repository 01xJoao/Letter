using System;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class DivisionModel : RealmObject
    {
        [PrimaryKey]
        public int DivisionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int UserCount { get; set; }
        public string AccessCode { get; set; }
        public string Picture { get; set; }
        public string ContactNumber { get; set; }
        public DateTimeOffset LastUpdate { get; set; }
        public int LastUpdateTicks { get; set; }
        public bool IsDivisonActive { get; set; }
        public bool IsUserInDivisionActive { get; set; }
        public bool IsUnderReview { get; set; }
        public int? StatusCode { get; set; }
    }

    public class DivisionModelProfile : RealmObject
    {
        [PrimaryKey]
        public int DivisionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int UserCount { get; set; }
        public string AccessCode { get; set; }
        public string Picture { get; set; }
        public string ContactNumber { get; set; }
        public DateTimeOffset LastUpdate { get; set; }
        public int LastUpdateTicks { get; set; }
        public bool IsDivisonActive { get; set; }
        public int OrgID { get; set; }
        public string OrgName { get; set; }
        public string OrgPic { get; set; }
        public int? StatusCode { get; set; }
    }

}
