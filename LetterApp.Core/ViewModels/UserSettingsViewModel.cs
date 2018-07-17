using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels
{
    public class UserSettingsViewModel : XViewModel
    {
        private IDialogService _dialogService;
        private IStatusCodeService _statusCodeService;
        private IUserService _userService;

        private UserModel _user;

        private bool _updateView;
        public bool UpdateView
        {
            get => _updateView;
            set => SetProperty(ref _updateView, value);
        }

        public SettingsPhoneModel PhoneModel { get; set; }

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public UserSettingsViewModel(IDialogService dialogService, IStatusCodeService statusCodeService, IUserService userService)
        {
            _dialogService = dialogService;
            _statusCodeService = statusCodeService;
            _userService = userService;
        }

        public override async Task InitializeAsync()
        {
            _user = Realm.Find<UserModel>(AppSettings.UserId);

            PhoneModel = new SettingsPhoneModel(PhoneLabel, _user.ContactNumber, ChangeNumber);

        }

        private void ChangeNumber(object sender, int number) => ChangePhoneNumber(number);

        private async Task ChangePhoneNumber(int number)
        {
            IsBusy = true;

            try
            {
                var result = await _userService.ChangePhoneNumber(number);

                if (result.StatusCode == 204)
                {
                    _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(result.StatusCode), AlertType.Success);
                }
                else
                {
                    RaisePropertyChanged(nameof(UpdateView));
                    _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(result.StatusCode), AlertType.Error);
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

        #region Resources

        public string SettingsTitle => L10N.Localize("UserSettings_SettingsTitle");
        private string PhoneLabel => L10N.Localize("UserSettings_PhoneNumberLabel");

        #endregion
    }
}
