using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Models.Generic;
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
        public SettingsAllowCallsModel AllowCallsModel { get; set; }
        public List<DescriptionTypeEventModel> TypeModel { get; set; }
        public List<DescriptionAndBoolEventModel> SwitchModel { get; set; }

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        private XPCommand<bool> _allowCallsCommand;
        public XPCommand<bool> AllowCallsCommand => _allowCallsCommand ?? (_allowCallsCommand = new XPCommand<bool>(async (value) => await SetAllowCalls(value), CanExecute));

        private XPCommand<int> _changeNumberCommand;
        public XPCommand<int> ChangeNumberCommand => _changeNumberCommand ?? (_changeNumberCommand = new XPCommand<int>(async (value) => await ChangePhoneNumber(value), CanExecute));

        public UserSettingsViewModel(IDialogService dialogService, IStatusCodeService statusCodeService, IUserService userService)
        {
            _dialogService = dialogService;
            _statusCodeService = statusCodeService;
            _userService = userService;
        }

        public override async Task InitializeAsync()
        {
            _user = Realm.Find<UserModel>(AppSettings.UserId);

            PhoneModel = new SettingsPhoneModel(PhoneLabel, _user.ContactNumber, ChangeNumberCommand);
            AllowCallsModel = new SettingsAllowCallsModel(AllowCallsTitle, AllowCallsDescription, AllowCallsCommand);

            var passwordType = new DescriptionTypeEventModel(PasswordLabel, true, GenericMethodType, CellType.Password);
            var contactUsType = new DescriptionTypeEventModel(ContactUsLabel, false, GenericMethodType, CellType.ContactUs);
            var termsOfServiceType = new DescriptionTypeEventModel(TermsLabel, false, GenericMethodType, CellType.TermsOfService);
            var createOrganizationType = new DescriptionTypeEventModel(CreateOrgLabel, false, GenericMethodType, CellType.CreateOrganization);
            var signOutType = new DescriptionTypeEventModel(SignOutLabel, false, GenericMethodType, CellType.SignOut);
            var leaveDivisionType = new DescriptionTypeEventModel(LeaveDivisionLabel, true, GenericMethodType, CellType.LeaveDivision);
            var leaveOrganizationType = new DescriptionTypeEventModel(LeaveOrganizationLabel, false, GenericMethodType, CellType.LeaveOrganization);
            var deleteAccountType = new DescriptionTypeEventModel(DeleteAccountLabel, false, GenericMethodType, CellType.DeleteAccount);

            var cellTypes = new[] {passwordType, contactUsType, termsOfServiceType, createOrganizationType, signOutType, leaveDivisionType, leaveOrganizationType, deleteAccountType};
            TypeModel = new List<DescriptionTypeEventModel>();
            TypeModel.AddRange(cellTypes);

            var messageNotifications = new DescriptionAndBoolEventModel(MessageNotificationLabel, AppSettings.MessageNotifications, MessageNotificationEvent);
            var callNotifications = new DescriptionAndBoolEventModel(CallNotificationLabel, AppSettings.CallNotifications, CallNotificationEvent);
            var groupNotifications = new DescriptionAndBoolEventModel(GroupNotificationLabel, AppSettings.GroupNotifications, GroupNotificationEvent);

            var notificationTypes = new[] { messageNotifications, callNotifications, groupNotifications };
            SwitchModel = new List<DescriptionAndBoolEventModel>();
            SwitchModel.AddRange(notificationTypes);
        }

        private void GroupNotificationEvent(object sender, bool value) => AppSettings.GroupNotifications = value;
        private void CallNotificationEvent(object sender, bool value) => AppSettings.CallNotifications = value;
        private void MessageNotificationEvent(object sender, bool value) => AppSettings.MessageNotifications = value;

        private void GenericMethodType(object sender, CellType type)
        {
            if (IsBusy)
                return;

            try
            {
                switch (type)
                {
                    case CellType.Password:
                        NavigationService.NavigateAsync<ChangePasswordViewModel, object>(null);
                        break;

                    case CellType.ContactUs:
                        BrowserUtils.OpenWebsite("http://www.lettermessenger.com/support/contactus");
                        break;

                    case CellType.TermsOfService:
                        BrowserUtils.OpenWebsite("http://www.lettermessenger.com/support/termsofservice");
                        break;

                    case CellType.CreateOrganization:
                        BrowserUtils.OpenWebsite("http://www.lettermessenger.com/organizations");
                        break;

                    case CellType.SignOut:
                        SignOut();
                        break;
                    case CellType.LeaveDivision:
                        NavigationService.NavigateAsync<LeaveDivisionViewModel, object>(null);
                        break;

                    case CellType.LeaveOrganization:
                        LeaveOrganization();
                        break;

                    case CellType.DeleteAccount:
                        DeleteAccount();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private async Task DeleteAccount()
        {
            IsBusy = true;

            try
            {
                var result = await _dialogService.ShowQuestion(DeleteAccountQuestion, DeleteAccountLabel, QuestionType.Bad);

                if(result)
                {
                    var res = await _userService.DeleteAccount();

                    if(res.StatusCode == 205)
                    {
                        await NavigationService.NavigateAsync<OnBoardingViewModel, object>(null);
                        _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(res.StatusCode), AlertType.Success);
                    }
                    else
                    {
                        _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(res.StatusCode), AlertType.Error);
                    }
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

        private async Task LeaveOrganization()
        {
            IsBusy = true;

            try
            {
                var result = await _dialogService.ShowQuestion(LeaveOrganizationQuestion, LeaveOrganizationLabel, QuestionType.Bad);

                if (result)
                {
                    var res = await _userService.LeaveOrganization((int)_user.OrganizationID);

                    if(res.StatusCode == 208)
                    {
                        await NavigationService.NavigateAsync<SelectOrganizationViewModel, object>(null);
                        await NavigationService.Close(this);
                        _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(res.StatusCode), AlertType.Success);
                    }
                    else
                    {
                        _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(res.StatusCode), AlertType.Error);
                    }
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

        private async Task SignOut()
        {
            IsBusy = true;

            try
            {
                var result = await _dialogService.ShowQuestion(SignOutQuestion, SignOutLabel, QuestionType.Bad);

                if(result)
                {
                    await NavigationService.NavigateAsync<OnBoardingViewModel, object>(null);
                    AppSettings.Logout();
                    await NavigationService.Close(this);
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

        private async Task SetAllowCalls(bool allow)
        {
            try
            {
                await _userService.AllowPhoneCalls(allow);   
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

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
        private bool CanExecute(bool obj) => !IsBusy;
        private bool CanExecute(int arg) => !IsBusy;

        #region Resources

        public string SettingsTitle => L10N.Localize("UserSettings_SettingsTitle");
        private string PhoneLabel => L10N.Localize("UserSettings_PhoneNumberLabel");
        private string AllowCallsTitle => L10N.Localize("UserSettings_AllowPhoneCallsLabel");
        private string AllowCallsDescription => L10N.Localize("UserSettings_AllowPhoneCallsDescription");
        private string PasswordLabel => L10N.Localize("UserSettings_PasswordLabel");
        private string ContactUsLabel => L10N.Localize("UserSettings_ContactUsLabel");
        private string TermsLabel => L10N.Localize("UserSettings_TermsOfService");
        private string CreateOrgLabel => L10N.Localize("UserSettings_CreateOrgLabel");
        private string SignOutLabel => L10N.Localize("UserSettings_SignOutLabel");
        private string LeaveDivisionLabel => L10N.Localize("UserSettings_LeaveDivisionLabel");
        private string LeaveOrganizationLabel => L10N.Localize("UserSettings_LeaveOrgLabel");
        private string DeleteAccountLabel => L10N.Localize("UserSettings_DeleteAccountLabel");
        private string SignOutQuestion => L10N.Localize("DialogLogout_Question");
        private string LeaveOrganizationQuestion => L10N.Localize("DialogOrganization_Question");
        private string DeleteAccountQuestion => L10N.Localize("DialogDelete_Question");
        private string MessageNotificationLabel => L10N.Localize("UserSettings_MessageLabel");
        private string CallNotificationLabel => L10N.Localize("UserSettings_CallLabel");
        private string GroupNotificationLabel => L10N.Localize("UserSettings_GroupLabel");
        #endregion
    }
}
