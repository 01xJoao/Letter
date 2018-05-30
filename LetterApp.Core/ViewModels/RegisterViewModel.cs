using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class RegisterViewModel : XViewModel
    {
        public RegisterFormModel RegisterForm = new RegisterFormModel();

        private XPCommand _createAccountCommand;
        public XPCommand CreateAccountCommand => _createAccountCommand ?? (_createAccountCommand = new XPCommand(async () => await CreateAccount(), CanExecute));

        public RegisterViewModel()
        {
            SetL10NResources();
        }

        private async Task CreateAccount()
        {
            //create account service

            //if success:
            await NavigationService.NavigateAsync<LoginViewModel, object>(null);
            await NavigationService.Close(this);
            await NavigationService.NavigateAsync<ActivateAccountViewModel, object>(null);
        }

        private bool CanExecute() => !IsBusy;

        #region resources

        public string Title => L10N.Localize("RegisterViewModel_Title");
        public string SubmitButton => L10N.Localize("RegisterViewModel_CreateAccountButton");

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
