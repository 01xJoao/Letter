using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class SelectOrganizationViewModel : XViewModel<string>
    {
        private IOrganizationSerivce _organizationSerivce;
        private IDialogService _dialogService;

        private XPCommand<string> _accessOrgCommand;
        public XPCommand<string> AccessOrgCommand => _accessOrgCommand ?? (_accessOrgCommand = new XPCommand<string>(async (orgName) => await AccessOrganization(orgName), CanExecute));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public string EmailDomain { get; private set; }

        public SelectOrganizationViewModel(IOrganizationSerivce organizationSerivce, IDialogService dialogService)
        {
            _organizationSerivce = organizationSerivce;
            _dialogService = dialogService;
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
                //service
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
            AppSettings.Logout();
            await NavigationService.Close(this);
        }

        private bool CanExecute() => !IsBusy;
        private bool CanExecute(string name) => !string.IsNullOrEmpty(name) && !IsBusy;

        #region Resources

        public string TitleLabel => L10N.Localize("SelectOrganization_TitleLabel");
        public string AccessButton => L10N.Localize("SelectOrganization_AccessButton");
        public string OrganizationHint => L10N.Localize("SelectOrganization_TextHint");

        #endregion
    }
}
