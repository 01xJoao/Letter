using System;
using Newtonsoft.Json;

namespace LetterApp.Models.DTO.RequestModels
{
    public class PasswordChangeRequestModel
    {
        public PasswordChangeRequestModel(string email, string newPassword, string requestedCode)
        {
            Email = email;
            NewPassword = newPassword;
            RequestedCode = requestedCode;
        }

        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
        [JsonProperty("activation_code")]
        public string RequestedCode { get; set; }
    }
}
