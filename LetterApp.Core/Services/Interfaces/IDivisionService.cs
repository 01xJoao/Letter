using System;
using System.Threading.Tasks;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IDivisionService
    {
        Task<DivisionModel> GetDivisionProfile(int divisionId);
    }
}
