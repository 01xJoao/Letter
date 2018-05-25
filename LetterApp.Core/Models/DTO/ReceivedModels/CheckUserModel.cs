using System;
using System.Collections.Generic;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class CheckUserModel : BaseModel
    {
        public int UserID { get; set; }
        public string AuthToken { get; set; }
        public bool IsUserActive { get; set; }
        public string Position { get; set; }
        public List<Division> Divisions;

    }

    public class Division
    {
        public int DivisionID { get; set; }
        public string DivisionName { get; set; }
        public bool IsDivisonActive { get; set; }
        public string DivisionLastUpdate { get; set; }
        public bool IsUserAdmin { get; set; }
        public bool IsUserInDivisionActive { get; set; }
    }
}