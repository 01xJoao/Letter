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

        public async Task<BaseModel> ChangePassword(PasswordChangeRequestModel passwordChange)
        {
            return await _webService.PostAsync<BaseModel>("users/forgotpassword", passwordChange, needsHeaderCheck: false).ConfigureAwait(false);
        }

        public async Task<BaseModel> ActivateAccount(ActivationCodeRequestModel activationCode)
        {
            return await _webService.PostAsync<BaseModel>("registration/activate", activationCode, needsHeaderCheck: false).ConfigureAwait(false);
        }

        public async Task<BaseModel> CreateAccount(UserRegistrationRequestModel userAccount)
        {
            return await _webService.PostAsync<BaseModel>("registration/add", userAccount, needsHeaderCheck: false).ConfigureAwait(false);
        }
    }
}
