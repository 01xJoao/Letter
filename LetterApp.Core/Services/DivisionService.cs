using System;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.Services
{
    public class DivisionService : IDivisionService
    {
        private IWebService _webService;
        public DivisionService(IWebService webService)
        {
            _webService = webService;
        }

        public async Task<DivisionModelProfile> GetDivisionProfile(int divisionId)
        {
            return await _webService.GetAsync<DivisionModelProfile>($"profiles/division/{divisionId}", needsHeaderCheck: true).ConfigureAwait(false);
        }
    }
}
