using System;
using Realms;

namespace LetterApp.Models.DTO.ReceivedModels
{
    public class TokensModel : BaseModel
    {
        public string AuthToken { get; set; }
        public DateTime AuthTokenExpirationDate { get; set; }
        public string PubNubToken { get; set; }
        public DateTime PubNubTokenExpirationDate { get; set; }
    }
}
