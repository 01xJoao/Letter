using System;
using System.Threading;
using System.Threading.Tasks;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserModel> LoginAsync(UserRequestModel userLoginRequest);
        Task<BaseModel> SendActivationCode(string email, string isActivation);
        Task<BaseModel> ChangePassword(PasswordChangeRequestModel passwordChange);
        Task<BaseModel> CreateAccount(UserRegistrationRequestModel userAccount);
        Task<BaseModel> ActivateAccount(ActivationCodeRequestModel activationCode);
        Task<UserModel> CheckUser();
    }
}
