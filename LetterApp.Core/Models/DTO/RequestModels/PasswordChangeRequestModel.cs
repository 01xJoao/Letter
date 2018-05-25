using System;
using Newtonsoft.Json;

namespace LetterApp.Models.DTO.RequestModels
{
    public class PasswordChangeRequestModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
        [JsonProperty("activation_code")]
        public string RequestedCode { get; set; }
    }
}
