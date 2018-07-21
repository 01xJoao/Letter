using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Models.DTO.RequestModels;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<OrganizationAccessModel> VerifyOrganization(string organizationName);
        Task<OrganizationAccessModel> AccessCodeOrganization(OrganizationRequestModel organizationCode);
        Task<List<DivisionModel>> GetDivisions(int organizationId);

        Task<BaseModel> JoinDivision(int divisionId);
        Task<DivisionModel> VerifyDivisionCode(string code);
        Task<BaseModel> LeaveOrganization(int organizationId);
        Task<BaseModel> LeaveDivision(int divisionId);

        Task<List<PositionModel>> GetPositions(int orgnizationId);
        Task<BaseModel> UpdatePosition(int position);
        Task<BaseModel> CheckUserInDivision(int divisionId);

        Task<OrganizationReceivedModel> GetOrganizationProfile(int orgnizationId);
    }
}
