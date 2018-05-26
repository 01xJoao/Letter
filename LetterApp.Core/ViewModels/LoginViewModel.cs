using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;
using SharpRaven.Data;

namespace LetterApp.Core.ViewModels
{
    public class LoginViewModel : XViewModel
    {
        private IAuthenticationService _authService;
        private IDialogService _dialogService;
        private ICodeResultService _codeResultService;

        private XPCommand<Tuple<string,string>> _signInCommand;
        public XPCommand<Tuple<string, string>> SignInCommand => _signInCommand ?? (_signInCommand = new XPCommand<Tuple<string, string>>(async (value) => await SignIn(value), CanLogin));

        private XPCommand _forgotPassCommand;
        public XPCommand ForgotPassCommand => _forgotPassCommand ?? (_forgotPassCommand = new XPCommand(async () => await ForgotPass(), CanExecute));

        public LoginViewModel(IAuthenticationService authService, IDialogService dialogService, ICodeResultService codeResultService)
        {
            _authService = authService;
            _dialogService = dialogService;
            _codeResultService = codeResultService;
        }

        private async Task SignIn(Tuple<string,string> value)
        {
            IsBusy = true;
            try
            {
                var userRequest = new UserRequestModel(value.Item1, value.Item2);
                var currentUser = await _authService.LoginAsync(userRequest);

                if(currentUser.Code == null)
                {
                    Realm.Write(() => Realm.Add(currentUser, true));
                    await NavigationService.NavigateAsync<MainViewModel, object>(null);
                }
                else
                    _dialogService.ShowAlert(_codeResultService.GetCodeDescription((int)currentUser.Code), AlertType.Error);
            }
            catch (Exception ex)
            {
                RavenService.Raven.Capture(new SentryEvent(ex));
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ForgotPass()
        {
            try
            {
                var email = await _dialogService.ShowInput(EnterEmail, ConfirmButton, EmailHint, InputType.Email);
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private bool CanLogin(Tuple<string, string> value) => !IsBusy && !string.IsNullOrEmpty(value.Item1) && !string.IsNullOrEmpty(value.Item2);
        private bool CanExecute() => !IsBusy;

        #region Resources

        public string EmailLabel => L10N.Localize("LoginViewModel_EmailLabel");
        public string PasswordLabel => L10N.Localize("LoginViewModel_PasswordLabel");
        public string ForgotPasswordButton => L10N.Localize("LoginViewModel_ForgotPassButton");
        public string SignUpButton => L10N.Localize("LoginViewModel_SignUpButton");
        public string SignInButton => L10N.Localize("LoginViewModel_SignInButton");
        public string EnterEmail => L10N.Localize("LoginViewModel_EnterEmail");
        public string EmailHint => L10N.Localize("LoginViewModel_EmailHint");
        public string ConfirmButton => L10N.Localize("LoginViewModel_SendCode");

        #endregion
    }
}
