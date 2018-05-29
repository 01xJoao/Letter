using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class RecoverPasswordViewModel : XViewModel<string>
    {
        private IDialogService _dialogService;
        private IAuthenticationService _authService;
        private IStatusCodeService _statusCodeService;

        public string Email { get; set; }

        private bool _isSubmiting;
        public bool IsSubmiting
        {
            get => _isSubmiting;
            set => SetProperty(ref _isSubmiting, value);
        }

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView()));

        private XPCommand _resendCodeCommand;
        public XPCommand ResendCodeCommand => _resendCodeCommand ?? (_resendCodeCommand = new XPCommand(async () => await ResendCode(), CanExecute));

        public RecoverPasswordViewModel(IDialogService dialogService, IAuthenticationService authService, IStatusCodeService statusCodeService) 
        {
            _dialogService = dialogService;
            _authService = authService;
            _statusCodeService = statusCodeService;
        }

        protected override void Prepare(string email)
        {
            Email = email;
        }

        public override async Task Appearing()
        {
            _dialogService.ShowAlert(EmailConfirmation, AlertType.Success, 6f);
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

        #region Resources

        public string NewPassTitle          => L10N.Localize("RecoverPasswordViewModel_NewPassTitle");
        public string EmailAddressLabel     => L10N.Localize("RecoverPasswordViewModel_EmailAddress");
        public string ConfirmPassLabel      => L10N.Localize("RecoverPasswordViewModel_ConfirmPass");
        public string CodeLabel             => L10N.Localize("RecoverPasswordViewModel_Code");
        public string RequestAgainButton    => L10N.Localize("RecoverPasswordViewModel_RequestAgain");
        public string SubmitButton          => L10N.Localize("RecoverPasswordViewModel_Submit");
        public string ShowButton            => L10N.Localize("RecoverPasswordViewModel_ShowButton");
        public string HideButton            => L10N.Localize("RecoverPasswordViewModel_HideButton");
        public string EmailConfirmation     => L10N.Localize("RecoverPasswordViewModel_EmailConfirmation");

        #endregion
    }
}
