using System;
using System.Threading.Tasks;
using LetterApp.Core.Models.DTO.RequestModels;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IWebService _webService;

        public UserService(IWebService webService)
        {
            _webService = webService;
        }

        public Task<UserProfileModel> GetUserProfile(UserProfileRequestModel userRequested)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseModel> UpdateUserDescription(string description)
        {
            var descriptionModel = new UserUpdateDescription(description);
            return await _webService.PostAsync<BaseModel> ("users/update/description", descriptionModel, needsHeaderCheck: true).ConfigureAwait(false);
        }
    }
}
