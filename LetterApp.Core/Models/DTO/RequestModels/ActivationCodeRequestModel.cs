using System;
using Newtonsoft.Json;

namespace LetterApp.Models.DTO.RequestModels
{
    public class ActivationCodeRequestModel
    {
        public ActivationCodeRequestModel(string email, string activationCode)
        {
            Email = email;
            ActivationCode = activationCode;
        }

        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("activation_code")]
        public string ActivationCode { get; set; }
    }
}
