using System;
using Newtonsoft.Json;

namespace LetterApp.Models.DTO.RequestModels
{
    public class OrganizationRequestModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("code")]
        public string AccessCode { get; set; }
    }
}
