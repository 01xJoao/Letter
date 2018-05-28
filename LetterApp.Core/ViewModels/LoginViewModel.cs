using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.Models.DTO.RequestModels;
using SharpRaven.Data;

namespace LetterApp.Core.ViewModels
{
    public class LoginViewModel : XViewModel
    {
        private IAuthenticationService _authService;
        private IDialogService _dialogService;
        private IStatusCodeService _codeResultService;

        private bool _isValidEmail = true;
        public bool IsValidEmail
        {
            get => _isValidEmail;
            set => SetProperty(ref _isValidEmail, value);
        }

        private bool _isSigningIn;
        public bool IsSigningIn
        {
            get => _isSigningIn;
            set => SetProperty(ref _isSigningIn, value);
        }

        private XPCommand<Tuple<string,string>> _signInCommand;
        public XPCommand<Tuple<string, string>> SignInCommand => _signInCommand ?? (_signInCommand = new XPCommand<Tuple<string, string>>(async (value) => await SignIn(value), CanLogin));

        private XPCommand<string> _forgotPassCommand;
        public XPCommand<string> ForgotPassCommand => _forgotPassCommand ?? (_forgotPassCommand = new XPCommand<string>(async (email) => await ForgotPassword(email), CanExecute));

        public LoginViewModel(IAuthenticationService authService, IDialogService dialogService, IStatusCodeService codeResultService)
        {
            _authService = authService;
            _dialogService = dialogService;
            _codeResultService = codeResultService;
        }

        private async Task SignIn(Tuple<string,string> value)
        {
            if (!EmailUtils.IsValidEmail(value.Item1))
            {
                IsValidEmail = false;
                return;
            }

            _isValidEmail  = true;
            IsSigningIn = true;
            IsBusy = true;

            try
            {
                var userRequest = new UserRequestModel(value.Item1, value.Item2);
                var currentUser = await _authService.LoginAsync(userRequest);

                if(currentUser.StatusCode == 200)
                {
                    Realm.Write(() => Realm.Add(currentUser, true));
                    await NavigationService.NavigateAsync<MainViewModel, object>(null);
                }
                else
                    _dialogService.ShowAlert(_codeResultService.GetStatusCodeDescription(currentUser.StatusCode), AlertType.Error);
            }
            catch (Exception ex)
            {
                RavenService.Raven.Capture(new SentryEvent(ex));
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                IsBusy = false;
                IsSigningIn = false;
            }
        }

        private async Task ForgotPassword(string emailInput)
        {
            try
            {
                var email = await _dialogService.ShowTextInput(EnterEmail, emailInput, ConfirmButton, EmailHint, InputType.Email);

                if(!string.IsNullOrEmpty(email) && EmailUtils.IsValidEmail(email))
                {
                    _dialogService.StartLoading();
                    var result = await _authService.SendActivationCode(email, "false");

                    if (result.StatusCode == 200)
                        await NavigationService.NavigateAsync<RecoverPasswordViewModel, string>(email);
                    else
                        _dialogService.ShowAlert(_codeResultService.GetStatusCodeDescription((result.StatusCode)), AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                _dialogService.StopLoading();
            }
        }

        private bool CanLogin(Tuple<string, string> value) => !IsBusy && !string.IsNullOrEmpty(value.Item1) && !string.IsNullOrEmpty(value.Item2);
        private bool CanExecute(object args) => !IsBusy;

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
