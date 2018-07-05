using System;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels
{
    public class PendingApprovalViewModel : XViewModel
    {

        //IMPORTANT!!!! Check & Set: AppSettings.UserIsPeddingApproval = true;


        private IOrganizationService _organizationService;
        private IDialogService _dialogService;

        public PendingApprovalViewModel(IDialogService dialogService, IOrganizationService organizationService)
        {
            _organizationService = organizationService;
            _dialogService = dialogService;
        }

        public override async Task InitializeAsync()
        {
        }
    }
}
