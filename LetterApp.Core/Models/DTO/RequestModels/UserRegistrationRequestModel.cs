using System;
using Newtonsoft.Json;

namespace LetterApp.Models.DTO.RequestModels
{
    public class UserRegistrationRequestModel
    {
        public UserRegistrationRequestModel(string email, string phone, string firstName, string lastName, string password)
        {
            Email = email;
            Phone = phone;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }

        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("firstname")]
        public string FirstName { get; set; }
        [JsonProperty("lastname")]
        public string LastName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
