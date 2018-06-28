using System;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.Services
{
    public class OrganizationService : IOrganizationSerivce
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
    }
}
