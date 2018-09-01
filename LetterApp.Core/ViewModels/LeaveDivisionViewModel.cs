using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels
{
    public class LeaveDivisionViewModel : XViewModel
    {
        private IDialogService _dialogService;
        private IStatusCodeService _statusCodeService;
        private IUserService _userService;

        public List<DivisionModel> Divisions;
        private UserModel _user;

        private bool _updateView;
        public bool UpdateView
        {
            get => _updateView;
            set => SetProperty(ref _updateView, value);
        }

        private XPCommand<DivisionModel> _leaveDivisionCommand;
        public XPCommand<DivisionModel>  LeaveDivisionCommand => _leaveDivisionCommand ?? (_leaveDivisionCommand = new XPCommand<DivisionModel>(async (division) => await LeaveDivision(division), CanExecute));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public LeaveDivisionViewModel(IDialogService dialogService, IStatusCodeService statusCodeService, IUserService userService)
        {
            _dialogService = dialogService;
            _statusCodeService = statusCodeService;
            _userService = userService;
        }

        public override async Task InitializeAsync()
        {
            _user = Realm.Find<UserModel>(AppSettings.UserId);

            Divisions = new List<DivisionModel>(_user.Divisions);
        }

        public async Task LeaveDivision(DivisionModel division)
        {
            IsBusy = true;

            Divisions.Remove(division);

            try
            {
                var res = await _userService.LeaveDivision(division.DivisionID);

                if (res.StatusCode == 206)
                {
                    Realm.Write(() => _user.Divisions.Remove(division));

                    var userDivisions = _user.Divisions.Any(x => x.IsDivisonActive == true && (x.IsUserInDivisionActive == true || x.IsUnderReview == true));

                    if (!userDivisions)
                        await NavigationService.NavigateAsync<SelectDivisionViewModel, Tuple<int, bool>>(new Tuple<int, bool>((int)_user.OrganizationID, true));
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(1f));
                    Divisions.Add(division);
                    RaisePropertyChanged(nameof(UpdateView));
                    _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(res.StatusCode), AlertType.Error);
                }

            }
            catch (Exception ex)
            {
                await Task.Delay(TimeSpan.FromSeconds(1f));
                Divisions.Add(division);
                RaisePropertyChanged(nameof(UpdateView));
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
        private bool CanExecute(object value) => !IsBusy;

        #region Resources

        public string Title = L10N.Localize("LeaveDivision_Title");
        public string LeaveButton = L10N.Localize("LeaveDivison_Leave");
        public string LeaveDivisionQuestion = L10N.Localize("LeaveDivison_LeaveQuestion");

        #endregion
    }
}
