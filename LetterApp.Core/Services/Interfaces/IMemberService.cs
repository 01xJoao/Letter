using System;
using System.Threading.Tasks;
using LetterApp.Core.Models.DTO.ReceivedModels;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IMemberService
    {
        Task<MembersProfileModel> GetMemberProfile(int userId);
    }
}
