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

        public async Task<BaseModel> AllowPhoneCalls(bool AllowCalls)
        {
            return await _webService.GetAsync<BaseModel>($"users/update/showcellphone/{AllowCalls}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> ChangePassword(string oldPassword, string newPassword)
        {
            var passwordModel = new UserChangePassword(oldPassword, newPassword);
            return await _webService.PostAsync<BaseModel>($"users/update/password", passwordModel, needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> ChangePhoneNumber(string phoneNumber)
        {
            var updatedNumber = new UserUpdateNumber(phoneNumber);
            return await _webService.PostAsync<BaseModel>($"users/update/cellphone", updatedNumber, needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> DeleteAccount(string pass)
        {
            var password = new UserDeleteAccount(pass);
            return await _webService.PostAsync<BaseModel>("users/deleteaccount", password, needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> LeaveDivision(int divisionId)
        {
            return await _webService.GetAsync<BaseModel>($"users/division/leave/{divisionId}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> LeaveOrganization(int organizationId)
        {
            return await _webService.GetAsync<BaseModel>($"organization/leaveorganization/{organizationId}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> UpdateUserDescription(string description)
        {
            var descriptionModel = new UserUpdateDescription(description);
            return await _webService.PostAsync<BaseModel>("users/update/description", descriptionModel, needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> UpdateUserPicture(string picture)
        {
            var pictureModel = new UserUpdatePicture(picture);
            return await _webService.PostAsync<BaseModel>("users/update/picture", pictureModel, needsHeaderCheck: true).ConfigureAwait(false);
        }
    }
}
