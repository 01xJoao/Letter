using System;
using System.Threading.Tasks;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IOrganizationSerivce
    {
        Task<OrganizationAccessModel> VerifyOrganization(string orgName);
    }
}
