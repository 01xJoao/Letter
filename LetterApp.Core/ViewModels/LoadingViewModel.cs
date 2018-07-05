using System;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
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
                else if (AppSettings.UserIsPeddingApproval)
                    await NavigationService.NavigateAsync<PendingApprovalViewModel, object>(null);
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
                    var user = Realm.Find<UserModel>(userCheck.UserID);

                    Realm.Write(() =>
                    {
                        user.IsUserActive = userCheck.IsUserActive;
                        user.Position = userCheck.Position;
                        user.OrganizationID = userCheck.OrganizationID;
                        user.Divisions = userCheck.Divisions;
                    });

                    bool userIsActiveInDivision = false;
                    bool AnyDivisionActive = false;

                    if (user.Divisions?.Count > 0)
                    {
                        AnyDivisionActive = user.Divisions.Any(x => x.IsDivisonActive == true);
                        userIsActiveInDivision = user.Divisions.Any(x => x.IsUserInDivisionActive == true && x.IsDivisonActive == true);
                    }

                    if (user.OrganizationID == null)
                    {
                        await NavigationService.NavigateAsync<SelectOrganizationViewModel, string>(user.Email);
                    }
                    else if (user.Divisions == null && !AnyDivisionActive)
                    {
                        await NavigationService.NavigateAsync<SelectDivisionViewModel, int>((int)user.OrganizationID);
                    }
                    else if (string.IsNullOrEmpty(user.Position))
                    {
                        await NavigationService.NavigateAsync<SelectPositionViewModel, Tuple<int, object>>(new Tuple<int, object>((int)user.OrganizationID, null));
                    }
                    else if (!userIsActiveInDivision)
                    {
                        AppSettings.UserIsPeddingApproval = true;
                        await NavigationService.NavigateAsync<PendingApprovalViewModel, object>(null);
                    }
                    else
                    {
                        await NavigationService.NavigateAsync<MainViewModel, object>(null);
                    }
                }
                else
                {
                    await NavigationService.NavigateAsync<MainViewModel, object>(null);
                }
            }
            catch (Exception ex)
            {
                await NavigationService.NavigateAsync<MainViewModel, object>(null);
                Ui.Handle(ex as dynamic);
            }
        }
    }
}
