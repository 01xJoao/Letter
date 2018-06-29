using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<OrganizationAccessModel> VerifyOrganization(string organizationName);
        Task<OrganizationAccessModel> AccessCodeOrganization(OrganizationRequestModel organizationCode);
        Task<List<DivisionModel>> GetDivisions(int organizationId);
    }
}
