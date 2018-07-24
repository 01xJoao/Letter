using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.Services
{
	public class ContactsService : IContactsService
    {
        private IWebService _webService;

        public ContactsService(IWebService webService)
        {
            _webService = webService;
        }

        public async Task<List<GetUsersInDivisionModel>> GetUsersFromAllDivisions()
        {
            return await _webService.GetAsync<List<GetUsersInDivisionModel>>("users/divisions", needsHeaderCheck: true).ConfigureAwait(false);
        }
    }
}
