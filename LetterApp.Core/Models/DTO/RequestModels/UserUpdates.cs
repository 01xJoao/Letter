using System;
using Newtonsoft.Json;

namespace LetterApp.Core.Models.DTO.RequestModels
{
    public class UserUpdateDescription
    {
        public UserUpdateDescription(string description)
        {
            Description = description;
        }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
