using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using Xamarin.Essentials;

namespace LetterApp.Core.ViewModels
{
    public class ChangePasswordViewModel : XViewModel
    {
        private IDialogService _dialogService;
        private IStatusCodeService _statusCodeService;
        private IUserService _userService;

        private bool _cleanPassowrd;
        public bool CleanPassword
        {
            get => _cleanPassowrd;
            set => SetProperty(ref _cleanPassowrd, value);
        }

        private XPCommand<Tuple<string, string,string>> _changePassCommand;
        public XPCommand<Tuple<string, string, string>> ChangePassCommand => _changePassCommand ?? (_changePassCommand = new XPCommand<Tuple<string, string, string>>(async (value) => await ChangePass(value), CanLogin));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public ChangePasswordViewModel(IDialogService dialogService, IStatusCodeService statusCodeService, IUserService userService) 
        {
            _dialogService = dialogService;
            _statusCodeService = statusCodeService; 
            _userService = userService;
        }

        private async Task ChangePass(Tuple<string, string, string> value)
        {

            if(string.IsNullOrEmpty(value.Item1) || string.IsNullOrEmpty(value.Item2) || string.IsNullOrEmpty(value.Item3))
            {
                _dialogService.ShowAlert(EmptyForm, AlertType.Error, 4f);
                return;
            }

            if (value.Item2.Length < 8)
            {
                _dialogService.ShowAlert(PasswordWeak, AlertType.Error, 6f);
                return;
            }

            if (value.Item2 != value.Item3)
            {
                _dialogService.ShowAlert(PasswordMatch, AlertType.Error, 3.5f);
                return;
            }

            IsBusy = true;

            try
            {
                var result = await _userService.ChangePassword(value.Item1, value.Item2);

                if(result.StatusCode == 200)
                {
                    await SecureStorage.SetAsync("password", value.Item2);
                    await CloseView();
                    await Task.Delay(TimeSpan.FromSeconds(0.3f));
                    _dialogService.ShowAlert(PasswordChanged, AlertType.Success, 5f);
                }
                else
                {
                    _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(result.StatusCode), AlertType.Error, 4f);
                    RaisePropertyChanged(nameof(CleanPassword));
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

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }

        private bool CanExecute() => !IsBusy;
        private bool CanLogin(object obj) => !IsBusy;

        #region Resources

        public string Title                 => L10N.Localize("ChangePassword_Title");
        public string CurrentPass           => L10N.Localize("ChangePassword_CurrentPass");
        public string NewPassTitle          => L10N.Localize("RecoverPasswordViewModel_NewPassTitle");
        public string NewPassAgainTitle     => L10N.Localize("RecoverPasswordViewModel_NewPassAgainTitle");
        public string ChangePassword        => L10N.Localize("ChangePassword_Change");

        private string EmptyForm        => L10N.Localize("RegisterViewModel_EmptyForm");
        private string PasswordMatch    => L10N.Localize("RecoverPasswordViewModel_PasswordMatch");
        private string PasswordWeak     => L10N.Localize("RecoverPasswordViewModel_PasswordWeak");
        private string PasswordChanged  => L10N.Localize("ChangePassword_Changed");
        #endregion
    }
}
