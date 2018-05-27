using System;
using System.Threading;
using System.Threading.Tasks;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserModel> LoginAsync(UserRequestModel userLoginRequest, CancellationToken ct = default(CancellationToken));
        Task<BaseModel> SendActivationCode(string email, string isActivation, CancellationToken ct = default(CancellationToken));
    }
}
