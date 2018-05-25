using System;
using Newtonsoft.Json;

namespace LetterApp.Models.DTO.RequestModels
{
    public class UserDivisionRequestModel
    {
        [JsonProperty("divisionid")]
        public int DivisionId { get; set; }
        [JsonProperty("userid")]
        public int UserId { get; set; }
        [JsonProperty("positionid")]
        public int PositionId { get; set; }
        [JsonProperty("useradmin")]
        public bool IsAdmin { get; set; }
    }
}
