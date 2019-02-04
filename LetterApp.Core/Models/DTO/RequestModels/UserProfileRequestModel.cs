using System;
using Newtonsoft.Json;

namespace LetterApp.Models.DTO.RequestModels
{
    public class UserProfileRequestModel
    {
        public UserProfileRequestModel(int userID, string lastUpdate)
        {
            UserID = userID;
            LastUpdate = lastUpdate;
        }

        [JsonProperty("userid")]
        public int UserID { get; set; }
        [JsonProperty("lastupdate")]
        public string LastUpdate { get; set; }
    }
}
