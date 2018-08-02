using System;
using System.Threading.Tasks;
using LetterApp.Core.Models.DTO.ReceivedModels;
using LetterApp.Core.Services.Interfaces;

namespace LetterApp.Core.Services
{
    public class MemberSerive : IMemberService
    {
        private IWebService _webService;
        public MemberSerive(IWebService webService)
        {
            _webService = webService;
        }

        public async Task<MembersProfileModel> GetMemberProfile(int userId)
        {
            return await _webService.GetAsync<MembersProfileModel>($"profiles/user/{userId}", needsHeaderCheck: true).ConfigureAwait(false);
        }
    }
}
