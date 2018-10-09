using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.RequestModels;

namespace LetterApp.Core.ViewModels
{
    public class SelectOrganizationViewModel : XViewModel<string>
    {
        private IOrganizationService _organizationService;
        private IDialogService _dialogService;
        private IStatusCodeService _statusCodeService;
        private ISettingsService _settingsService;

        private XPCommand<string> _accessOrgCommand;
        public XPCommand<string> AccessOrgCommand => _accessOrgCommand ?? (_accessOrgCommand = new XPCommand<string>(async (orgName) => await AccessOrganization(orgName), CanExecute));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        private XPCommand _createOrganizationCommand;
        public XPCommand CreateOrganizationCommand => _createOrganizationCommand ?? (_createOrganizationCommand = new XPCommand(async () => await OpenCreateOrganization()));

        public string EmailDomain { get; private set; }

        public bool RegisterUser { get; private set; }

        public SelectOrganizationViewModel(IOrganizationService organizationService, IDialogService dialogService, IStatusCodeService statusCodeService, ISettingsService settingsService)
        {
            _organizationService = organizationService;
            _dialogService = dialogService;
            _statusCodeService = statusCodeService;
            _settingsService = settingsService;
        }

        protected override void Prepare(string email)
        {
            EmailDomain = EmailUtils.GetEmailDomain(email);
        }

        private async Task AccessOrganization(string orgName)
        {
            IsBusy = true;

            try
            {
                var organization = await _organizationService.VerifyOrganization(orgName);

                if(organization?.StatusCode == 200)
                {
                    if (!organization.RequiresAccessCode)
                    {
                        AppSettings.OrganizationId = organization.OrganizationID;
                        AppSettings.UserAndOrganizationIds = $"{AppSettings.UserId}-{AppSettings.OrganizationId}";
                        RaisePropertyChanged(nameof(RegisterUser));
                        await NavigationService.NavigateAsync<SelectPositionViewModel, int>(organization.OrganizationID);
                    }
                    else
                    {
                        IsBusy = false;
                        var result = await _dialogService.ShowTextInput(OrganizationLabel, organization.Name, string.Empty, EnterButton, AccessHint, InputType.Text);

                        if(!string.IsNullOrEmpty(result))
                        {
                            var orgCode = new OrganizationRequestModel(orgName, result);
                            var res = await _organizationService.AccessCodeOrganization(orgCode);

                            if(res.StatusCode == 200)
                                await NavigationService.NavigateAsync<SelectPositionViewModel, int>(organization.OrganizationID);
                            else
                                _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(res.StatusCode), AlertType.Error);
                        }
                    }
                }
                else
                {
                    _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(organization?.StatusCode), AlertType.Error);
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

        private async Task OpenCreateOrganization()
        {
            await BrowserUtils.OpenWebsite("http://www.lettermessenger.com");
        }

        private async Task CloseView()
        {
            var result = await _dialogService.ShowQuestion(QuestionLabel, QuestionButton, QuestionType.Bad);

            if(result)
            {
                AppSettings.Logout();
                _settingsService.Logout();
                await NavigationService.NavigateAsync<LoginViewModel, object>(null);
            }
        }

        private bool CanExecute() => !IsBusy;
        private bool CanExecute(string name) => !string.IsNullOrEmpty(name) && !IsBusy;

        #region Resources

        public string TitleLabel            => L10N.Localize("SelectOrganization_TitleLabel");
        public string AccessButton          => L10N.Localize("SelectOrganization_AccessButton");
        public string OrganizationHint      => L10N.Localize("SelectOrganization_TextHint");
        public string CreateOrganization    => L10N.Localize("SelectOrganization_CreateOrg");
        private string OrganizationLabel    => L10N.Localize("SelectOrganization_Organization");
        private string AccessHint           => L10N.Localize("SelectOrganization_AccessHint");
        private string EnterButton          => L10N.Localize("SelectOrganization_EnterButton");
        private string QuestionLabel        => L10N.Localize("DialogLogout_Question");
        private string QuestionButton       => L10N.Localize("DialogLogout_Button");

        #endregion
    }
}
