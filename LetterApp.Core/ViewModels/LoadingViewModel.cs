using System;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Core.ViewModels.TabBarViewModels;

namespace LetterApp.Core.ViewModels
{
    public class LoadingViewModel : XViewModel
    {
        private ICodeResultService _errorCodeService;

        public LoadingViewModel(ICodeResultService errorCodeService)
        {
            _errorCodeService = errorCodeService;
        }

        public override async Task InitializeAsync()
        {
            await Task.Delay(1);
            _errorCodeService.SetCodes();
            await NavigationService.NavigateAsync<OnBoardingViewModel, object>(null);
        }
    }
}
