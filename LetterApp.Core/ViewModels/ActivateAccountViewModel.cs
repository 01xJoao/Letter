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
    public class ActivateAccountViewModel : XViewModel<string>
    {
        private IDialogService _dialogService;
        private IAuthenticationService _authService;
        private IStatusCodeService _statusCodeService;

        private string _email;

        private bool _isActivating;
        public bool IsActivating
        {
            get => _isActivating;
            set => SetProperty(ref _isActivating, value);
        }


        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        private XPCommand _resendCodeCommand;
        public XPCommand ResendCodeCommand => _resendCodeCommand ?? (_resendCodeCommand = new XPCommand(async () => await ResendCode(), CanExecute));

        private XPCommand<string> _activateAccountCommand;
        public XPCommand<string> ActivateAccountCommand => _activateAccountCommand ?? (_activateAccountCommand = new XPCommand<string>(async (code) => await ActivateAccount(code), CanExecute));

        public ActivateAccountViewModel(IDialogService dialogService, IAuthenticationService authService, IStatusCodeService statusCodeService)
        {
            _dialogService = dialogService;
            _authService = authService;
            _statusCodeService = statusCodeService;
        }

        private async Task ActivateAccount(string code)
        {
            IsBusy = true;
            IsActivating = true;
            try
            {
                var requestActivation = new ActivationCodeRequestModel(_email, code);
                var result = await _authService.ActivateAccount(requestActivation);

                if(result.StatusCode == 200)
                {
                    AppSettings.UserEmail = _email;
                    await NavigationService.Close(this);
                    await Task.Delay(TimeSpan.FromSeconds(0.3));
                    _dialogService.ShowAlert(ActivationCompleted, AlertType.Success, 4f);
                }
                else
                    _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription((result.StatusCode)), AlertType.Error);

            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                IsActivating = false;
                IsBusy = false;
            }
        }

        private async Task ResendCode()
        {
            IsBusy = true;

            try
            {
                var result = await _authService.SendActivationCode(_email, "true");

                if (result.StatusCode == 200)
                    _dialogService.ShowAlert(EmailConfirmation, AlertType.Success, 4f);
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

        protected override void Prepare(string email) => _email = email;

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }
        private bool CanExecute() => !IsBusy;
        private bool CanExecute(string code) => !IsBusy && !string.IsNullOrEmpty(code);

        #region Resources

        public string Title => L10N.Localize("ActivateAccountViewModel_Title");
        public string SubmitButton => L10N.Localize("ActivateAccountViewModel_Submit");
        public string PlaceHolder => L10N.Localize("ActivateAccountViewModel_PlaceHolder");
        public string ResendCodeButton => L10N.Localize("ActivateAccountViewModel_ResendCodeButton");
        public string ActivateLabel => L10N.Localize("ActivateAccountViewModel_ActivateLabel");
        private string ActivationCompleted => L10N.Localize("ActivateAccountViewModel_ActivationCompleted");
        private string EmailConfirmation => L10N.Localize("RecoverPasswordViewModel_EmailConfirmation");

        #endregion
    }
}
