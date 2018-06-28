using System;
using Newtonsoft.Json;

namespace LetterApp.Models.DTO.RequestModels
{
    public class OrganizationRequestModel
    {
        public OrganizationRequestModel(string name, string accessCode)
        {
            Name = name;
            AccessCode = accessCode;
        }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("code")]
        public string AccessCode { get; set; }
    }
}
