using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models.DTO.RequestModels;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels
{
    public class SelectPositionViewModel : XViewModel<int>
    {
        private IOrganizationService _organizationService;
        private IDialogService _dialogService;
        private IStatusCodeService _statusCodeService;
        private IAuthenticationService _authenticationService;

        private int _organizationId;
        private int? _positionId;

        private List<PositionModel> _positions;
        public List<PositionModel> Positions
        {
            get => _positions;
            set => SetProperty(ref _positions, value);
        }

        private XPCommand _setUserCommand;
        public XPCommand SetUserCommand => _setUserCommand ?? (_setUserCommand = new XPCommand(async () => await UpdatePosition(), CanExecute));

        private XPCommand<int> _setPositionCommand;
        public XPCommand<int> SetPositionCommand => _setPositionCommand ?? (_setPositionCommand = new XPCommand<int>((position) => SetPosition(position)));

        public SelectPositionViewModel(IOrganizationService organizationService, IDialogService dialogService, IStatusCodeService statusCodeService, IAuthenticationService authenticationService) 
        {
            _organizationService = organizationService;
            _dialogService = dialogService;
            _statusCodeService = statusCodeService;
            _authenticationService = authenticationService;
        }

        protected override void Prepare(int orgId)
        {
            _organizationId = orgId;
        }

        public override async Task InitializeAsync()
        {
            IsBusy = true;

            try
            {
                Positions = await _organizationService.GetPositions(_organizationId);
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

        private void SetPosition(int position)
        {
            _positionId = position;
        }

        private async Task UpdatePosition()
        {
            if (_positionId == null) return;

            IsBusy = true;
                
            try
            {
                var result = await _organizationService.UpdatePosition((int)_positionId);

                if (result.StatusCode == 200)
                {
                    var res = await _authenticationService.CheckUser();

                    var userIsActiveInDivision = res.Divisions.Any(x => x.IsUserInDivisionActive == true && x.IsDivisonActive == true);

                    if (userIsActiveInDivision)
                    {
                        AppSettings.MainMenuAllowed = true;
                        await NavigationService.NavigateAsync<MainViewModel, object>(null);
                        await NavigationService.Close(this);
                    }
                    else
                    {
                        await NavigationService.NavigateAsync<SelectDivisionViewModel, Tuple<int, bool>>(new Tuple<int, bool>(_organizationId, true));
                    }
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
            }
        }

        private bool CanExecute() => !IsBusy;

        #region Resources

        public string TitleLabel => L10N.Localize("SelectPosition_TitleLabel");
        public string SelectButton => L10N.Localize("SelectPosition_Select");
        public string SelectPositionLabel => L10N.Localize("SelectPosition_SelectPosition");

        #endregion
    }
}
