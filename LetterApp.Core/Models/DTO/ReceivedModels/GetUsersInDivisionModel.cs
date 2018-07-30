using System;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class GetUsersInDivisionModel : RealmObject
    {
        [PrimaryKey]
        public string UniqueKey { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public bool ShowNumber { get; set; }
        public string ContactNumber { get; set; }
        public string Picture { get; set; }
        public int DivisionId { get; set; }
        public int MainDivisionId { get; set; }
        public string SearchContainer { get; set; }
    }
}
