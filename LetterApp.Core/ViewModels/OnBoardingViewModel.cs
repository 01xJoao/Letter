using System;
using System.Threading.Tasks;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Core.ViewModels.TabBarViewModels;

namespace LetterApp.Core.ViewModels
{
    public class OnBoardingViewModel : XViewModel
    {
        private XPCommand _openLoginViewCommand;
        public XPCommand OpenLoginViewCommand => _openLoginViewCommand ?? (_openLoginViewCommand = new XPCommand(async () => await OpenLoginView()));

        private XPCommand _openRegisterViewCommand;
        public XPCommand OpenRegisterViewCommand => _openRegisterViewCommand ?? (_openRegisterViewCommand = new XPCommand(async () => await OpenRegisterView()));

        private async Task OpenRegisterView()
        {
            await NavigationService.NavigateAsync<RegisterViewModel, object>(null);
        }

        private async Task OpenLoginView()
        {
            await NavigationService.NavigateAsync<LoginViewModel, object>(null);
        }

        #region Resources

        public string WelcomeTitle => L10N.Localize("OnBoardingViewModel_LetterHelloWorld");
        public string RegisterTitle => L10N.Localize("OnBoardingViewModel_Register");
        public string CallTitle => L10N.Localize("OnBoardingViewModel_Call");

        public string WelcomeSubtitle => L10N.Localize("OnBoardingViewModel_LetterSlogan");
        public string RegisterSubtitle => L10N.Localize("OnBoardingViewModel_RegisterSubtitle");
        public string CallSubtitle => L10N.Localize("OnBoardingViewModel_CallSubtitle");

        public string SignIn => L10N.Localize("OnBoardingViewModel_SignIn");
        public string SignUp => L10N.Localize("OnBoardingViewModel_SignUp");

        #endregion
    }
}
