using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.ViewModels
{
    public class RegisterViewModel : XViewModel
    {
        IAuthenticationService _authService;
        IDialogService _dialogService;
        IStatusCodeService _statusService;

        public RegisterFormModel RegisterForm;

        private XPCommand _createAccountCommand;
        public XPCommand CreateAccountCommand => _createAccountCommand ?? (_createAccountCommand = new XPCommand(async () => await CreateAccount(), CanExecute));

        public RegisterViewModel(IAuthenticationService authService, IDialogService dialogService, IStatusCodeService statusService)
        {
            _authService = authService;
            _dialogService = dialogService;
            _statusService = statusService;
            SetL10NResources();
            RegisterForm = new RegisterFormModel();
        }

        private async Task CreateAccount()
        {
            if (ReflectionHelper.WasEmptyValues(RegisterForm))
            {
                _dialogService.ShowAlert(EmptyForm, AlertType.Error);
                return;
            }

            if (!StringUtils.CheckForEmojis(RegisterForm.Password) || !StringUtils.CheckForEmojis(RegisterForm.FirstName) || !StringUtils.CheckForEmojis(RegisterForm.LastName))
            {
                _dialogService.ShowAlert(DamnEmojis, AlertType.Error, 6f);
                return;
            }

            if (!StringUtils.IsLettersOnly(RegisterForm.FirstName) || !StringUtils.IsLettersOnly(RegisterForm.LastName))
            {
                _dialogService.ShowAlert(InvalidString, AlertType.Error, 6.5f);
                return;
            }

            if(!EmailUtils.IsValidEmail(StringUtils.RemoveWhiteSpaces(RegisterForm.EmailAddress)))
            {
                _dialogService.ShowAlert(InvalidEmail, AlertType.Error);
                return;
            }

            if (RegisterForm.Password.Length < 8)
            {
                _dialogService.ShowAlert(PasswordWeak, AlertType.Error, 6f);
                return;
            }

            if (RegisterForm.Password != RegisterForm.VerifyPassword)
            {
                _dialogService.ShowAlert(PasswordMatch, AlertType.Error, 3.5f);
                return;
            }

            if(!RegisterForm.UserAgreed)
            {
                _dialogService.ShowAlert(UserAgreement, AlertType.Error);
                return;
            }

            IsBusy = true;

            var user = new UserRegistrationRequestModel(StringUtils.RemoveWhiteSpaces(RegisterForm.EmailAddress), RegisterForm.PhoneNumber, StringUtils.RemoveWhiteSpaces(RegisterForm.FirstName), 
                                                        StringUtils.RemoveWhiteSpaces(RegisterForm.LastName), RegisterForm.Password);

            try
            {
                var result = await _authService.CreateAccount(user);

                if (result.StatusCode == 207)
                {
                    await NavigationService.NavigateAsync<LoginViewModel, object>(null);
                    await NavigationService.Close(this);
                    await NavigationService.NavigateAsync<ActivateAccountViewModel, string>(RegisterForm.EmailAddress);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    _dialogService.ShowAlert(_statusService.GetStatusCodeDescription(result.StatusCode), AlertType.Success, 8f);
                }
                else
                {
                    _dialogService.ShowAlert(_statusService.GetStatusCodeDescription(result.StatusCode), AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanExecute() => !IsBusy;

        #region resources

        public string Title             => L10N.Localize("RegisterViewModel_Title");
        public string SubmitButton      => L10N.Localize("RegisterViewModel_CreateAccountButton");
        private string EmptyForm        => L10N.Localize("RegisterViewModel_EmptyForm");
        private string InvalidString    => L10N.Localize("RegisterViewModel_InvalidString");
        private string PasswordMatch    => L10N.Localize("RecoverPasswordViewModel_PasswordMatch");
        private string PasswordWeak     => L10N.Localize("RecoverPasswordViewModel_PasswordWeak");
        private string InvalidEmail     => L10N.Localize("LoginViewModel_InvalidEmail");
        private string UserAgreement    => L10N.Localize("RegisterViewModel_UserAgreement");
        private string DamnEmojis       => L10N.Localize("RegisterViewModel_DamnEmojis");

        public Dictionary<string, string> LocationResources = new Dictionary<string, string>();

        private string _firstname => L10N.Localize("RegisterViewModel_FirstName");
        private string _lastname => L10N.Localize("RegisterViewModel_LastName");
        private string _email => L10N.Localize("LoginViewModel_EmailLabel");
        private string _number => L10N.Localize("RegisterViewModel_PhoneNumber");
        private string _password => L10N.Localize("LoginViewModel_PasswordLabel");
        private string _confirmpassword => L10N.Localize("RecoverPasswordViewModel_ConfirmPass");
        private string _agreement => L10N.Localize("RegisterViewModel_Agreement");

        private void SetL10NResources()
        {
            LocationResources.Add("firstname", _firstname);
            LocationResources.Add("lastname", _lastname);
            LocationResources.Add("email", _email);
            LocationResources.Add("passsword", _password);
            LocationResources.Add("confirmpassword", _confirmpassword);
            LocationResources.Add("number", _number);
            LocationResources.Add("agreement", _agreement);
        }

        #endregion
    }
}
