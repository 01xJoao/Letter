using System;
using System.Threading;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IWebService _webService;

        public AuthenticationService(IWebService webService)
        {
            _webService = webService;
        }

        public async Task<UserModel> LoginAsync(UserRequestModel userLoginRequest)
        {
            return await _webService.PostAsync<UserModel>("users/login", userLoginRequest, needsHeaderCheck: false).ConfigureAwait(false);
        }

        public async Task<BaseModel> SendActivationCode(string email, string isActivation)
        {
            return await _webService.GetAsync<BaseModel>($"registration/resendcode/{email}/{isActivation}", needsHeaderCheck: false).ConfigureAwait(false);
        }
    }
}
