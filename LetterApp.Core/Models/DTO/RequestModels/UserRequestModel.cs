using System;
using Newtonsoft.Json;

namespace LetterApp.Models.DTO.RequestModels
{
    public class UserRequestModel
    {
        public UserRequestModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
