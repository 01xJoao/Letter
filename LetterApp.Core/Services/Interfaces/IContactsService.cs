using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IContactsService
    {
        Task<List<GetUsersInDivisionModel>> GetUsersFromAllDivisions();
        Task<NotificationTokenModel> GetUserPushToken(int userId);
    }
}
