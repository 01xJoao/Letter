using System;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Core.ViewModels.TabBarViewModels;

namespace LetterApp.Core.ViewModels
{
    public class LoadingViewModel : XViewModel
    {
        public LoadingViewModel() {}

        public override async Task InitializeAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(0.3));
            if(AppSettings.IsUserLogged)
                await NavigationService.NavigateAsync<MainViewModel, object>(null);
            else
                await NavigationService.NavigateAsync<OnBoardingViewModel, object>(null);
        }
    }
}
