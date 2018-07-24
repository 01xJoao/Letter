using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels
{
    public class LoadingViewModel : XViewModel
    {
        private IAuthenticationService _authService;
        private IDialogService _dialogService;
        private IStatusCodeService _statusCodeService;

        public LoadingViewModel(IAuthenticationService authService, IDialogService dialogService, IStatusCodeService statusCodeService)
        {
            _authService = authService;
            _dialogService = dialogService;
            _statusCodeService = statusCodeService;
        }

        public override async Task InitializeAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(0.3));

            try
            {
                if (AppSettings.IsUserLogged)
                    await CheckUser();
                else
                    await NavigationService.NavigateAsync<OnBoardingViewModel, object>(null);
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private async Task CheckUser()
        {
            try
            {
                var userCheck = await _authService.CheckUser();

                if (userCheck.StatusCode == 200)
                {
                    if(AppSettings.UserId != userCheck.UserID)
                    {
                        await Logout();
                        return;
                    }

                    var user = RealmUtils.UpdateUser(Realm, userCheck);

                    bool userIsActiveInDivision = false;
                    bool anyDivisionActive = false;
                    bool userIsUnderReview = false;

                    if (userCheck?.Divisions?.Count > 0)
                    {
                        anyDivisionActive = userCheck.Divisions.Any(x => x.IsDivisonActive == true);
                        userIsActiveInDivision = userCheck.Divisions.Any(x => x.IsUserInDivisionActive == true && x.IsDivisonActive == true);
                        userIsUnderReview = userCheck.Divisions.Any(x => x.IsUserInDivisionActive == false && x.IsUnderReview == true && x.IsDivisonActive == true);
                    }

                    if (userCheck.OrganizationID == null)
                    {
                        await NavigationService.NavigateAsync<SelectOrganizationViewModel, string>(userCheck.Email);
                    }
                    else if (string.IsNullOrEmpty(userCheck.Position))
                    {
                        await NavigationService.NavigateAsync<SelectPositionViewModel, int>((int)userCheck.OrganizationID);
                    }
                    else if ((userCheck.Divisions == null || userCheck?.Divisions?.Count == 0) || !anyDivisionActive || (!userIsActiveInDivision && !userIsUnderReview))
                    {
                        await NavigationService.NavigateAsync<SelectDivisionViewModel, Tuple<int,bool>>(new Tuple<int, bool> ((int)userCheck.OrganizationID, true));
                    }
                    else if (!userIsActiveInDivision && userIsUnderReview)
                    {
                        await NavigationService.NavigateAsync<PendingApprovalViewModel, object>(null);
                    }
                    else
                    {
                        AppSettings.MainMenuAllowed = true;
                        await NavigationService.NavigateAsync<MainViewModel, object>(null);
                        return;
                    }

                    AppSettings.MainMenuAllowed = false;
                }
                else if(userCheck.StatusCode == 0)
                {
                    await Logout();
                }
            }
            catch (Exception ex)
            {
                if (ex is ServerErrorException)
                    await Logout();
                else
                {
                    if(AppSettings.MainMenuAllowed)
                        await NavigationService.NavigateAsync<MainViewModel, object>(null);
                    else
                        await Logout();
                }

                Ui.Handle(ex as dynamic);
            }
        }

        private async Task Logout()
        {
            AppSettings.Logout();
            await NavigationService.NavigateAsync<LoginViewModel, object>(null);
        }
    }
}
