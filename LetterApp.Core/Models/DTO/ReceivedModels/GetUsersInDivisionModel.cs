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
        public string Picture { get; set; }
        public int DivisionId { get; set; }
    }
}
