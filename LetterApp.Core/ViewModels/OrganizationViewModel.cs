using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels
{
    public class OrganizationViewModel : XViewModel<int>
    {
        private IOrganizationService _organizationService;
        private IDialogService _dialogService;
        private IStatusCodeService _statusCodeService;

        private int _organizationId;

        private OrganizationModel _organization;
        public OrganizationModel Organization
        {
            get => _organization;
            set => SetProperty(ref _organization, value);
        }

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));


        public OrganizationViewModel(IOrganizationService organizationService, IDialogService dialogService, IStatusCodeService statusCodeService) 
        {
            _organizationService = organizationService;
            _dialogService = dialogService;
            _statusCodeService = statusCodeService;
        }

        protected override void Prepare(int organizationId)
        {
            _organizationId = organizationId;
        }

        public override async Task Appearing()
        {
            _organization = Realm.Find<OrganizationModel>(_organizationId);
            SetupModels(_organization);

            try
            {
                var result = await _organizationService.GetOrganizationProfile(_organizationId);

                if(result.StatusCode == 200)
                {
                    var shouldUpdateView = false;

                    if (result.LastUpdateTicks > _organization?.LastUpdateTicks || _organization == null)
                        shouldUpdateView = true;

                    var org = new OrganizationModel();

                    org.OrganizationID = result.OrganizationID;
                    org.Name = result.Name;
                    org.Description = result.Description;
                    org.Picture = result.Picture;
                    org.Description = result.Description;
                    org.ContactNumber = result.ContactNumber;
                    org.Address = result.Address;
                    org.Email = result.Email;
                    org.LastUpdateTicks = result.LastUpdateTicks;
                    foreach (var divion in result.Divisions)
                           org.Divisions.Add(divion);

                    Realm.Write(() => {
                        Realm.Add(org, true);
                    });

                    if(shouldUpdateView)
                    {
                        _organization = org;
                        SetupModels(_organization);
                    }
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                if (Organization == null)
                    _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(0), AlertType.Error);
            }
        }

        private void SetupModels(OrganizationModel organization)
        {
            
        }

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }

        private bool CanExecute() => !IsBusy;

    }
}
