using System;
using Newtonsoft.Json;

namespace LetterApp.Core.Models.DTO.RequestModels
{
    public class DivisionRequestModel
    {
        public DivisionRequestModel(int division, int position)
        {
            Division = division;
            Position = position;
        }

        [JsonProperty("division")]
        public int Division { get; set; }
        [JsonProperty("position")]
        public int Position { get; set; }
    }
}
