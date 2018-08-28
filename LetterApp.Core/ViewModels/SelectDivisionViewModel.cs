using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels
{
    public class SelectDivisionViewModel : XViewModel<Tuple<int,bool>>
    {
        private IDialogService _dialogService;
        private IStatusCodeService _statusCodeService;
        private IOrganizationService _organizationService;
        private ILogoutService _logoutService;

        private int _organizationId;
        public bool NewUser { get; set; }

        private List<DivisionModel> _divisions;
        public List<DivisionModel> Divisions
        {
            get => _divisions;
            set => SetProperty(ref _divisions, value);
        }

        private bool _isLoading = true;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private XPCommand<DivisionModel> _showDivisionInformationCommand;
        public XPCommand<DivisionModel> ShowDivisionInformationCommand => _showDivisionInformationCommand ?? (_showDivisionInformationCommand = new XPCommand<DivisionModel>(async (division) => await ShowDivisionInfo(division), CanExecute));

        private XPCommand<string> _verifyDivisionCodeCommand;
        public XPCommand<string> VerifyDivisionCodeCommand => _verifyDivisionCodeCommand ?? (_verifyDivisionCodeCommand = new XPCommand<string>(async (code) => await VerifyDivisionCode(code), CanExecute));

        private XPCommand _leaveOrganizationCommand;
        public XPCommand LeaveOrganizationCommand => _leaveOrganizationCommand ?? (_leaveOrganizationCommand = new XPCommand(async () => await LeaveOrganization(), CanExecute));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public SelectDivisionViewModel(IOrganizationService organizationService, IDialogService dialogService, IStatusCodeService statusCodeService, ILogoutService logoutService)
        {
            _organizationService = organizationService;
            _dialogService = dialogService;
            _statusCodeService = statusCodeService;
            _logoutService = logoutService;

            SetL10NResources();
        }

        protected override void Prepare(Tuple<int, bool> data)
        {
            _organizationId = data.Item1;
            NewUser = data.Item2;
        }

        public override async Task InitializeAsync()
        {
            IsBusy = true;

            try
            {
                Divisions = await _organizationService.GetDivisions(_organizationId);
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                IsBusy = false;
                IsLoading = false;
            }
        }

        private async Task VerifyDivisionCode(string code)
        {
            IsBusy = true;

            try
            {
                var result = await _organizationService.VerifyDivisionCode(code);

                if (result.StatusCode == 200)
                    await ShowDivisionInfo(result);
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

        private async Task ShowDivisionInfo(DivisionModel division)
        {
            IsBusy = true;

            try
            {
                var result = await _dialogService.ShowInformation(division.Name, division.Email, $"{division.UserCount} {MembersLabel}", division.Description, SubmitButton);

                if (result)
                {
                    var res = await _organizationService.JoinDivision(division.DivisionID);

                    if (res.StatusCode == 200)
                    {
                        await NavigationService.NavigateAsync<PendingApprovalViewModel, int>(NewUser ? 2 : 3);
                        await NavigationService.PopToRoot(false);
                    }
                    else
                        _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(res.StatusCode), AlertType.Info);
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
                var result = await _organizationService.LeaveOrganization(_organizationId);

                if (result.StatusCode == 208)
                {
                    await NavigationService.NavigateAsync<SelectOrganizationViewModel, object>(null);
                    await NavigationService.PopToRoot(true);
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

        private async Task CloseView()
        {
            if(NewUser)
            {
                var result = await _dialogService.ShowQuestion(QuestionLabel, QuestionButton, QuestionType.Bad);

                if (result)
                {
                    AppSettings.Logout();
                    _logoutService.Logout();
                    await NavigationService.NavigateAsync<LoginViewModel, object>(null);
                    await NavigationService.PopToRoot(true);
                }
            }
            else
            {
                await NavigationService.Close(this);
            }
        }

        private bool CanExecute() => !IsBusy;
        private bool CanExecute(object obj) => !IsBusy;

        #region Resources

        public string TitleLabel => L10N.Localize("SelectDivision_TitleLabel");
        public string TitleMainLabel => L10N.Localize("SelectDivision_TitleMainLabel");
        private string QuestionLabel => L10N.Localize("DialogLogout_Question");
        private string QuestionButton => L10N.Localize("DialogLogout_Button");
        private string MailLabel => L10N.Localize("SelectDivision_EmailLabel");
        private string MembersLabel => L10N.Localize("SelectDivision_MembersLabel");
        private string DescriptionLabel => L10N.Localize("SelectDivision_DescriptionLabel");

        public Dictionary<string, string> LocationResources = new Dictionary<string, string>();
        private string PrivateDivisionLabel     => L10N.Localize("SelectDivision_PrivateLabel");
        private string PublicDivisionLabel      => L10N.Localize("SelectDivision_PublicLabel");
        private string InsertDivisionText       => L10N.Localize("SelectDivision_InsertText");
        private string SubmitButton             => L10N.Localize("SelectDivision_SubmitButton");
        private string LeaveOrganizationButton  => L10N.Localize("SelectDivision_LeaveOrganization");

        private void SetL10NResources()
        {
            LocationResources.Add("Private", PrivateDivisionLabel);
            LocationResources.Add("Public", PublicDivisionLabel);
            LocationResources.Add("Insert", InsertDivisionText);
            LocationResources.Add("Submit", SubmitButton);
            LocationResources.Add("Leave", LeaveOrganizationButton);
        }

        #endregion
    }
}
