using System;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class TokensModel : BaseModel
    {
        public string AuthToken { get; set; }
        public string PubNubToken { get; set; }
    }
}
