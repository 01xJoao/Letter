using System;
using System.Threading.Tasks;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileModel> GetUserProfile(UserProfileRequestModel userRequested);
    }
}
