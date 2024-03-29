﻿using System;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels
{
    public class PendingApprovalViewModel : XViewModel<int>
    {
        private int _organizationId;
        private int _tabIndex;
        public DivisionModel Division { get; set; }

        private bool _canContinue;
        public bool CanContinue
        {
            get => _canContinue;
            set => SetProperty(ref _canContinue, value);
        }

        private IOrganizationService _organizationService;
        private IDialogService _dialogService;
        private IAuthenticationService _authenticationService;
        private IStatusCodeService _statusCodeService;
        private ISettingsService _settingsService;

        private XPCommand _navigateToMainCommand;
        public XPCommand NavigateToMainCommand => _navigateToMainCommand ?? (_navigateToMainCommand = new XPCommand(async () => await NavigateToMain(), CanExecute));

        private XPCommand _updateCommand;
        public XPCommand UpdateCommand => _updateCommand ?? (_updateCommand = new XPCommand(async () => await CheckUser(true), CanExecute));

        private XPCommand _openEmailCommand;
        public XPCommand OpenEmailCommand => _openEmailCommand ?? (_openEmailCommand = new XPCommand(async () => await OpenEmail(), CanExecute));

        private XPCommand _logoutCommand;
        public XPCommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new XPCommand(async () => await Logout(), CanExecute));

        private XPCommand _leaveDivisionCommand;
        public XPCommand LeaveDivisionCommand => _leaveDivisionCommand ?? (_leaveDivisionCommand = new XPCommand(async () => await LeaveDivision(), CanExecute));

        public PendingApprovalViewModel(IStatusCodeService statusCodeService, IDialogService dialogService, IOrganizationService organizationService, 
                                        IAuthenticationService authenticationService, ISettingsService settingsService)
        {
            _organizationService = organizationService;
            _dialogService = dialogService;
            _authenticationService = authenticationService;
            _statusCodeService = statusCodeService;
            _settingsService = settingsService;
        }

        protected override void Prepare(int tabIndex)
        {
            _tabIndex = tabIndex;
        }

        public override async Task Appearing() => await CheckUser(false);

        private async Task CheckUser(bool isUpdating)
        {
            if (!isUpdating)
                _dialogService.StartLoading();

            IsBusy = true;

            try
            {
                var user = await _authenticationService.CheckUser();
                _organizationId = (int)user.OrganizationID;

                bool userIsActiveInDivision = false;
                bool anyDivisionActive = false;
                bool userIsUnderReview = false;

                if (user?.Divisions?.Count > 0)
                {
                    anyDivisionActive = user.Divisions.Any(x => x.IsDivisonActive == true);
                    userIsActiveInDivision = user.Divisions.Any(x => x.IsUserInDivisionActive == true && x.IsDivisonActive == true);
                    userIsUnderReview = user.Divisions.Any(x => x.IsUserInDivisionActive == false && x.IsUnderReview == true && x.IsDivisonActive == true);
                }
                else
                {
                    await Logout();
                    return;
                }

                if (!userIsUnderReview && userIsActiveInDivision)
                {
                    await Task.Delay(TimeSpan.FromSeconds(4f));
                    await NavigateToMain();
                    return;
                }
                else if (userIsUnderReview && userIsActiveInDivision)
                {
                    CanContinue = true;
                }
                else if(userIsUnderReview && !userIsActiveInDivision)
                {
                    Division = user.Divisions.First(x => x.IsUserInDivisionActive == false && x.IsDivisonActive == true);

                    if (isUpdating)
                        _dialogService.ShowAlert(UpdateAlert, AlertType.Info);
                    else
                        RaisePropertyChanged(nameof(CanContinue));
                }
                else
                {
                    await Logout();
                    return;
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                _dialogService.StopLoading();
                IsBusy = false;
            }
        }

        private async Task OpenEmail()
        {
            await EmailUtils.SendEmail(Division.Email);
        }

        private async Task NavigateToMain()
        {
            AppSettings.MainMenuAllowed = true;
            await NavigationService.NavigateAsync<MainViewModel, int>(_tabIndex);
        }

        private async Task Logout()
        {
            AppSettings.Logout();
            _settingsService.Logout();
            await NavigationService.NavigateAsync<LoginViewModel, object>(null);
        }

        private async Task LeaveDivision()
        {
            IsBusy = true;

            try
            {
                var result = await _organizationService.LeaveDivision(Division.DivisionID);

                if(result.StatusCode == 206)
                {
                    await NavigationService.NavigateAsync<SelectDivisionViewModel, Tuple<int, bool>>(new Tuple<int, bool>(_organizationId, true));
                    _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(result.StatusCode), AlertType.Success);
                }
                else
                {
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

        private bool CanExecute() => !IsBusy;

        #region Resources

        public string Title         => L10N.Localize("Pending_Title");
        public string Subtitle      => L10N.Localize("Pending_Subtitle");
        public string Description   => L10N.Localize("Pending_Description");
        public string HelpLabel     => L10N.Localize("Pending_Help");
        public string LeaveButton   => L10N.Localize("Pending_LeaveButton");
        public string SubmitButton  => L10N.Localize("Pending_ContinueButton");
        public string LogoutButton  => L10N.Localize("Pending_LogoutButton");
        private string UpdateAlert  => L10N.Localize("Pending_Updated");

        #endregion
    }
}
