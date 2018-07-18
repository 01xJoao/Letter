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

    public class UserUpdatePicture
    {
        public UserUpdatePicture(string picture)
        {
            Picture = picture;
        }

        [JsonProperty("picture")]
        public string Picture { get; set; }
    }

    public class UserUpdateNumber
    {
        public UserUpdateNumber(string number)
        {
            CellNumber = number;
        }

        [JsonProperty("phonenumber")]
        public string CellNumber { get; set; }
    }
}
