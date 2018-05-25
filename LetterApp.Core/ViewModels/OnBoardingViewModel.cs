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
        private XPCommand<object> _openLoginViewCommand;
        public XPCommand<object> OpenLoginViewCommand => _openLoginViewCommand ?? (_openLoginViewCommand = new XPCommand<object>(async (navigation) => await OpenLoginView(navigation)));

        private async Task OpenLoginView(object navigation)
        {
            await NavigationService.NavigateAsync<LoginViewModel, object>(navigation);
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
