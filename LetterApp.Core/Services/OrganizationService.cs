using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Models.DTO.RequestModels;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IWebService _webService;

        public OrganizationService(IWebService webService)
        {
            _webService = webService;
        }

        public async Task<OrganizationAccessModel> VerifyOrganization(string orgName)
        {
            return await _webService.GetAsync<OrganizationAccessModel>($"organization/verifyname/{orgName}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<OrganizationAccessModel> AccessCodeOrganization(OrganizationRequestModel organizationCode)
        {
            return await _webService.PostAsync<OrganizationAccessModel>("organization/verifycode", organizationCode, needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<List<DivisionModel>> GetDivisions(int organizationId)
        {
            return await _webService.GetAsync<List<DivisionModel>>($"organization/getdivisions/{organizationId}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> JoinDivision(int divisionId)
        {
            return await _webService.GetAsync<BaseModel>($"users/joindivision/{divisionId}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<DivisionModel> VerifyDivisionCode(string code)
        {
            return await _webService.GetAsync<DivisionModel>($"users/division/verify/{code}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> LeaveOrganization(int organizationId)
        {
            return await _webService.GetAsync<BaseModel>($"organization/leaveorganization/{organizationId}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<List<PositionModel>> GetPositions(int orgnizationId)
        {
            return await _webService.GetAsync<List<PositionModel>>($"organization/getpositions/{orgnizationId}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> UpdatePosition(int positionId)
        {
            return await _webService.GetAsync<BaseModel>($"users/update/position/{positionId}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> CheckUserInDivision(int divisionId)
        {
            return await _webService.GetAsync<BaseModel>($"users/division/checkuser/{divisionId}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<BaseModel> LeaveDivision(int divisionId)
        {
            return await _webService.GetAsync<BaseModel>($"users/division/leave/{divisionId}", needsHeaderCheck: true).ConfigureAwait(false);
        }

        public async Task<OrganizationReceivedModel> GetOrganizationProfile(int orgnizationId)
        {
            return await _webService.GetAsync<OrganizationReceivedModel>($"profiles/organization/{orgnizationId}", needsHeaderCheck: true).ConfigureAwait(false);
        }
    }
}
