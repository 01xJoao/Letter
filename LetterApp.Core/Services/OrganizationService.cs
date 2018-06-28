using System;
using System.Threading.Tasks;
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
    }
}
