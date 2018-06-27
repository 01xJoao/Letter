using System;
using System.Collections.Generic;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class CheckUserModel : BaseModel
    {
        public int UserID { get; set; }
        public bool IsUserActive { get; set; }
        public string Position { get; set; }
        public List<Division> Divisions;
    }

    public class Division : RealmObject
    {
        public int DivisionID { get; set; }
        public string DivisionName { get; set; }
        public bool IsDivisonActive { get; set; }
        public string DivisionLastUpdate { get; set; }
        public bool IsUserAdmin { get; set; }
        public bool IsUserInDivisionActive { get; set; }
    }
}