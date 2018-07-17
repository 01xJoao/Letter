using System;
using System.Threading.Tasks;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileModel> GetUserProfile(UserProfileRequestModel userRequested);
        Task<BaseModel> UpdateUserDescription(string description);
        Task<BaseModel> UpdateUserPicture(string picture);
        Task<BaseModel> ChangePhoneNumber(int phoneNumber);
        Task<BaseModel> AllowPhoneCalls(bool AllowCalls);
        Task<BaseModel> ChangePassword(string oldPassword, string newPassword);
        Task<BaseModel> LeaveDivision(int divisionId);
        Task<BaseModel> LeaveOrganization(int organizationId);
        Task<BaseModel> DeleteAccount();
    }
}
