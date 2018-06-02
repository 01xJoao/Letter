using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models.Cells;
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

        private string _email;
        public List<FormModel> FormModelList { get; set; }

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

        private XPCommand _submitFormCommand;
        public XPCommand SubmitFormCommand => _submitFormCommand ?? (_submitFormCommand = new XPCommand(async () => await SubmitForm(), CanExecute));

        public RecoverPasswordViewModel(IDialogService dialogService, IAuthenticationService authService, IStatusCodeService statusCodeService) 
        {
            _dialogService = dialogService;
            _authService = authService;
            _statusCodeService = statusCodeService;
        }

        protected override void Prepare(string email) => _email = email;

        public override async Task InitializeAsync()
        {
            FormModelList = new List<FormModel>();

            var emailForm = new FormModel(0,_email, EmailAddressLabel, FieldType.Email, ReturnKeyType.Next);
            var passwordForm = new FormModel(1,"", NewPassTitle, FieldType.Password, ReturnKeyType.Next, new string[] {ShowButton, HideButton});
            var confirmPasswordForm = new FormModel(2,"", NewPassTitle, FieldType.Password, ReturnKeyType.Next, new string[] { ShowButton, HideButton });
            var CodeForm = new FormModel(3,"", CodeLabel, FieldType.Code, ReturnKeyType.Default, keyboardButtonText: SubmitButton, submitKeyboardButton: async () => await SubmitForm());

            var form = new[] { emailForm, passwordForm, confirmPasswordForm, CodeForm };
            FormModelList.AddRange(form);
        }

        private async Task SubmitForm()
        {
            if(FormModelList[1].TextFieldValue.Length < 8)
            {
                _dialogService.ShowAlert(PasswordWeak, AlertType.Error, 6f);
                return;
            }

            if(FormModelList[1].TextFieldValue != FormModelList[2].TextFieldValue)
            {
                _dialogService.ShowAlert(PasswordMatch, AlertType.Error, 3.5f);
                return;
            }

            IsBusy = true;
            IsSubmiting = true;

            try
            {
                var forgotPassModel = new PasswordChangeRequestModel(FormModelList[0].TextFieldValue, FormModelList[1].TextFieldValue, FormModelList[3].TextFieldValue);
                var result = await _authService.ChangePassword(forgotPassModel);

                if (result.StatusCode == 201)
                {
                    await CloseView();
                    await Task.Delay(TimeSpan.FromSeconds(0.3));
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
                var result = await _authService.SendActivationCode(FormModelList[0].TextFieldValue, "false");

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
