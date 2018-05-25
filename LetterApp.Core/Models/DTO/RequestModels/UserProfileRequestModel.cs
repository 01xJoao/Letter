using System;
using Newtonsoft.Json;

namespace LetterApp.Models.DTO.RequestModels
{
    public class UserProfileRequestModel
    {
        [JsonProperty("userid")]
        public int UserID { get; set; }
        [JsonProperty("lastupdate")]
        public string LastUpdate { get; set; }
    }
}
