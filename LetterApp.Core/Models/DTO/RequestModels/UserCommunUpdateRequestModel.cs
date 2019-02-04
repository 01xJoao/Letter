using System;
using Newtonsoft.Json;

namespace LetterApp.Models.DTO.RequestModels
{
    public class UserCommunUpdateRequestModel
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("old_password")]
        public string OldPassword { get; set; }

        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }
}
