using System;
using System.Collections.Generic;
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
    public class SelectPositionViewModel : XViewModel<Tuple<int,object>>
    {
        private IOrganizationService _organizationService;
        private IDialogService _dialogService;
        private IStatusCodeService _statusCodeService;
        private IAuthenticationService _authenticationService;

        private int _organizationId;
        private int? _divisionId;
        private int? _positionId;

        private List<PositionModel> _positions;
        public List<PositionModel> Positions
        {
            get => _positions;
            set => SetProperty(ref _positions, value);
        }

        private XPCommand _setUserCommand;
        public XPCommand SetUserCommand => _setUserCommand ?? (_setUserCommand = new XPCommand(async () => await SetUser(), CanExecute));

        private XPCommand<int> _setPositionCommand;
        public XPCommand<int> SetPositionCommand => _setPositionCommand ?? (_setPositionCommand = new XPCommand<int>((position) => SetPosition(position)));

        public SelectPositionViewModel(IOrganizationService organizationService, IDialogService dialogService, IStatusCodeService statusCodeService, IAuthenticationService authenticationService) 
        {
            _organizationService = organizationService;
            _dialogService = dialogService;
            _statusCodeService = statusCodeService;
            _authenticationService = authenticationService;
        }

        protected override void Prepare(Tuple<int, object> data)
        {
            _organizationId = data.Item1;

            if(data.Item2 != null)
                _divisionId = (int)data.Item2;
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

        private async Task SetUser()
        {
            if(_positionId != null)
            {
                if (_divisionId != null)
                    await SetUserDivison();
                else
                    await UpdatePosition();
            }
        }

        private async Task SetUserDivison()
        {
            IsBusy = true;

            try
            {
                var request = new DivisionRequestModel((int)_divisionId, (int)_positionId);
                var result = await _organizationService.SetUserDivision(request);

                if (result.StatusCode == 200)
                {
                    await NavigationService.NavigateAsync<PendingApprovalViewModel, object>(null);
                    await NavigationService.PopToRoot(false);
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

        private async Task UpdatePosition()
        {
            IsBusy = true;
                
            try
            {
                var result = await _organizationService.UpdatePosition((int)_positionId);

                if (result.StatusCode == 200)
                {
                    await NavigationService.NavigateAsync<MainViewModel, object>(null);
                    await NavigationService.Close(this);
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
