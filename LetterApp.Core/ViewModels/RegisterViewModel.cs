using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Models.Cells;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.RequestModels;
using Xamarin.Essentials;

namespace LetterApp.Core.ViewModels
{
    public class RegisterViewModel : XViewModel
    {
        private IAuthenticationService _authService;
        private IDialogService _dialogService;
        private IStatusCodeService _statusService;
        private ISettingsService _settingsService;

        private bool _userAgreed;
        public List<FormModel> FormModelList { get; set; }

        private XPCommand _createAccountCommand;
        public XPCommand CreateAccountCommand => _createAccountCommand ?? (_createAccountCommand = new XPCommand(async () => await CreateAccount(), CanExecute));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        private XPCommand<bool> _openRegisterViewCommand;
        public XPCommand<bool> AgreementToogleCommand => _openRegisterViewCommand ?? (_openRegisterViewCommand = new XPCommand<bool>((agreed) => AgreementToogle(agreed)));

        public RegisterViewModel(IAuthenticationService authService, IDialogService dialogService, IStatusCodeService statusService, ISettingsService settingsService)
        {
            _authService = authService;
            _dialogService = dialogService;
            _statusService = statusService;
            _settingsService = settingsService;
        }

        private void AgreementToogle(bool agreed) => _userAgreed = agreed;

        public override async Task InitializeAsync()
        {
            FormModelList = new List<FormModel>();

            var firstname = new FormModel(0, "", _firstname, FieldType.Null, ReturnKeyType.Next);
            var lastname = new FormModel(1, "", _lastname, FieldType.Null, ReturnKeyType.Next);
            var email = new FormModel(2, "", _email, FieldType.Email, ReturnKeyType.Next);
            var password = new FormModel(3, "", _password, FieldType.Password, ReturnKeyType.Next, new string[] { _showButton, _hideButton });
            var confirmpassword = new FormModel(4, "", _confirmpassword, FieldType.Password, ReturnKeyType.Next, new string[] { _showButton, _hideButton });
            var number = new FormModel(5, "", _number, FieldType.Phone, ReturnKeyType.Default);

            var form = new[] { firstname, lastname, email, password, confirmpassword, number };
            FormModelList.AddRange(form);
        }

        private async Task CreateAccount()
        {
            IsBusy = true;

            var user = new UserRegistrationRequestModel(
                StringUtils.RemoveWhiteSpaces(FormModelList[2].TextFieldValue),
                FormModelList[5].TextFieldValue,
                StringUtils.RemoveWhiteSpaces(FormModelList[0].TextFieldValue),
                StringUtils.RemoveWhiteSpaces(FormModelList[1].TextFieldValue),
                FormModelList[3].TextFieldValue
            );

            if(!CheckForm(user))
            {
                IsBusy = false;
                return;
            }

            user.FirstName = StringUtils.FirstCharToUpper(user.FirstName);
            user.LastName = StringUtils.FirstCharToUpper(user.LastName);

            try
            {
                var result = await _authService.CreateAccount(user);

                if (result.StatusCode == 207)
                {
                    AppSettings.Logout();
                    _settingsService.Logout();
                    AppSettings.UserEmail = user.Email;
                    await SecureStorage.SetAsync("password", user.Password); 

                    await NavigationService.NavigateAsync<LoginViewModel, object>(null);
                    await CloseView();
                    await NavigationService.NavigateAsync<ActivateAccountViewModel, string>(user.Email);
                    await Task.Delay(TimeSpan.FromSeconds(0.8f));
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

        private bool CheckForm(UserRegistrationRequestModel user)
        {
            if (ReflectionHelper.HasEmptyOrNullValues(user))
            {
                _dialogService.ShowAlert(EmptyForm, AlertType.Error);
                return false;
            }

            if (!StringUtils.CheckForEmojis(user.Password) || !StringUtils.CheckForEmojis(user.FirstName) || !StringUtils.CheckForEmojis(user.LastName))
            {
                _dialogService.ShowAlert(DamnEmojis, AlertType.Error, 6f);
                return false;
            }

            if (!StringUtils.IsLettersOnly(user.FirstName) || !StringUtils.IsLettersOnly(user.LastName))
            {
                _dialogService.ShowAlert(InvalidString, AlertType.Error, 6.5f);
                return false;
            }

            if (!EmailUtils.IsValidEmail(StringUtils.RemoveWhiteSpaces(user.Email)))
            {
                _dialogService.ShowAlert(InvalidEmail, AlertType.Error);
                return false;
            }

            if (user.Password.Length < 8)
            {
                _dialogService.ShowAlert(PasswordWeak, AlertType.Error, 6f);
                return false;
            }

            if (user.Password != FormModelList[4].TextFieldValue)
            {
                _dialogService.ShowAlert(PasswordMatch, AlertType.Error, 3.5f);
                return false;
            }

            if(user.Phone.Length > 16 || user.Phone.Length < 8)
            {
                _dialogService.ShowAlert(AlertPhoneNumber, AlertType.Error, 3.5f);
                return false;
            }

            if (!_userAgreed)
            {
                _dialogService.ShowAlert(UserAgreement, AlertType.Error);
                return false;
            }

            return true;
        }

        private async Task CloseView()
        {
            await NavigationService.Close(this);
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
        private string AlertPhoneNumber => L10N.Localize("UserSettings_CellNumber");

        public string AgreementLabel => L10N.Localize("RegisterViewModel_Agreement");
        private string _firstname => L10N.Localize("RegisterViewModel_FirstName");
        private string _lastname => L10N.Localize("RegisterViewModel_LastName");
        private string _email => L10N.Localize("LoginViewModel_EmailLabel");
        private string _number => L10N.Localize("RegisterViewModel_PhoneNumber");
        private string _password => L10N.Localize("LoginViewModel_PasswordLabel");
        private string _confirmpassword => L10N.Localize("RecoverPasswordViewModel_ConfirmPass");
        private string _showButton => L10N.Localize("RecoverPasswordViewModel_ShowButton");
        private string _hideButton => L10N.Localize("RecoverPasswordViewModel_HideButton");

        #endregion
    }
}
