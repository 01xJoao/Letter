using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.ViewModels
{
    public class RecoverPasswordViewModel : XViewModel<string>
    {
        private IDialogService _dialogService;
        private IAuthenticationService _authService;
        private IStatusCodeService _statusCodeService;

        public string Email { get; set; }

        private bool _isValidPassword = true;
        public bool IsValidPassword
        {
            get => _isValidPassword;
            set => SetProperty(ref _isValidPassword, value);
        }

        private bool _isSubmiting;
        public bool IsSubmiting
        {
            get => _isSubmiting;
            set => SetProperty(ref _isSubmiting, value);
        }

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        private XPCommand _resendCodeCommand;
        public XPCommand ResendCodeCommand => _resendCodeCommand ?? (_resendCodeCommand = new XPCommand(async () => await ResendCode(), CanExecute));

        private XPCommand<Tuple<string, string, string>> _submitFormCommand;
        public XPCommand<Tuple<string, string, string>> SubmitFormCommand => _submitFormCommand ?? 
            (_submitFormCommand = new XPCommand<Tuple<string, string, string>>(async (formValues) => await SubmitForm(formValues), CanExecute));

        public RecoverPasswordViewModel(IDialogService dialogService, IAuthenticationService authService, IStatusCodeService statusCodeService) 
        {
            _dialogService = dialogService;
            _authService = authService;
            _statusCodeService = statusCodeService;
        }

        protected override void Prepare(string email) => Email = email;

        private async Task SubmitForm(Tuple<string,string,string> formValues)
        {
            _isValidPassword = false;

            if(formValues.Item1.Length < 8)
            {
                _dialogService.ShowAlert(PasswordWeak, AlertType.Error, 6f);
                IsValidPassword = true;
                return;
            }

            if(formValues.Item1 != formValues.Item2)
            {
                _dialogService.ShowAlert(PasswordMatch, AlertType.Error, 3.5f);
                IsValidPassword = true;
                return;
            }

            IsBusy = true;
            IsSubmiting = true;

            try
            {
                var forgotPassModel = new PasswordChangeRequestModel(Email, formValues.Item1, formValues.Item3);
                var result = await _authService.ChangePassword(forgotPassModel);

                if (result.StatusCode == 201)
                {
                    await CloseView();
                    await Task.Delay(TimeSpan.FromSeconds(0.5));
                    _dialogService.ShowAlert(PasswordChanged, AlertType.Success, 5f);
                }
                else
                    _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(result.StatusCode), AlertType.Error);
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                IsBusy = false;
                IsSubmiting = false;
            }
        }

        private async Task ResendCode()
        {
            IsBusy = true;

            try
            {
                var result = await _authService.SendActivationCode(Email, "false");

                if (result.StatusCode == 200)
                    _dialogService.ShowAlert(EmailConfirmation, AlertType.Success, 6f);
                else
                    _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription((result.StatusCode)), AlertType.Error);
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

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }

        private bool CanExecute() => !IsBusy;
        private bool CanExecute(Tuple<string, string, string> formValues)
        {
            if (string.IsNullOrEmpty(formValues.Item1) || string.IsNullOrEmpty(formValues.Item2) || string.IsNullOrEmpty(formValues.Item3))
                return false;
            
            return !IsBusy;
        }

        #region Resources

        public string NewPassTitle          => L10N.Localize("RecoverPasswordViewModel_NewPassTitle");
        public string EmailAddressLabel     => L10N.Localize("RecoverPasswordViewModel_EmailAddress");
        public string ConfirmPassLabel      => L10N.Localize("RecoverPasswordViewModel_ConfirmPass");
        public string CodeLabel             => L10N.Localize("RecoverPasswordViewModel_Code");
        public string RequestAgainButton    => L10N.Localize("RecoverPasswordViewModel_RequestAgain");
        public string SubmitButton          => L10N.Localize("RecoverPasswordViewModel_Submit");
        public string ShowButton            => L10N.Localize("RecoverPasswordViewModel_ShowButton");
        public string HideButton            => L10N.Localize("RecoverPasswordViewModel_HideButton");
        private string EmailConfirmation     => L10N.Localize("RecoverPasswordViewModel_EmailConfirmation");
        private string PasswordMatch         => L10N.Localize("RecoverPasswordViewModel_PasswordMatch");
        private string PasswordWeak          => L10N.Localize("RecoverPasswordViewModel_PasswordWeak");
        private string PasswordChanged       => L10N.Localize("RecoverPasswordViewModel_PasswordChanged");

        #endregion
    }
}
