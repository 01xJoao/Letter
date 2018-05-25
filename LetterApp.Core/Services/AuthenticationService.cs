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

        public async Task<UserModel> LoginAsync(UserRequestModel userLoginRequest, CancellationToken ct = default(CancellationToken))
        {
            return await _webService.PostAsync<UserModel>("users/login", userLoginRequest).ConfigureAwait(false);
        }
    }
}
