using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels
{
    public class DivisionViewModel : XViewModel<int>
    {
        private IDivisionService _divisionService;
        private IDialogService _dialogService;
        private IStatusCodeService _statusCodeService;

        private int _divisionId;

        private DivisionModel _division;
        public DivisionModel Division
        {
            get => _division;
            set => SetProperty(ref _division, value);
        }

        public OrganizationInfoModel OrganizationInfo { get; set; }
        public ProfileDetailsModel ProfileDetails { get; private set; }
        public DivisionHeaderModel DivisionHeader { get; private set; }

        private XPCommand _sendEmailCommand;
        public XPCommand SendEmailCommand => _sendEmailCommand ?? (_sendEmailCommand = new XPCommand(async () => await SendEmail()));

        private XPCommand _callCommand;
        public XPCommand CallCommand => _callCommand ?? (_callCommand = new XPCommand(async () => await Call()));

        private XPCommand _openOrganizationCommand;
        public XPCommand OpenOrganizationCommand => _openOrganizationCommand ?? (_openOrganizationCommand = new XPCommand(async () => await OpenOrganization(), CanExecute));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public DivisionViewModel(IDivisionService divisionService, IDialogService dialogService, IStatusCodeService statusCodeService) 
        {
            _divisionService = divisionService;
            _dialogService = dialogService;
            _statusCodeService = statusCodeService;
        }

        protected override void Prepare(int divisionId)
        {
            _divisionId = divisionId;
        }

        public override async Task InitializeAsync()
        {
            SetupModels(Realm.Find<DivisionModel>(_divisionId));

            try
            {
                var result = await _divisionService.GetDivisionProfile(_divisionId);

                if(result.StatusCode == 200)
                {
                    result.IsUnderReview = Division.IsUnderReview;
                    result.IsUserInDivisionActive = Division.IsUserInDivisionActive;

                    Realm.Write(() => {
                        Realm.Add(result, true);
                    });

                    SetupModels(result);
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                if(Division == null)
                    _dialogService.ShowAlert(_statusCodeService.GetStatusCodeDescription(0), AlertType.Error);
            }
        }

        private void SetupModels(DivisionModel division)
        {
            if (division == null)
                return;

            var profileDetails = new List<ProfileDetail>();

            var profileDetail1 = new ProfileDetail();
            profileDetail1.Description = AddressLabel;
            profileDetail1.Value = division.Address;

            var profileDetail2 = new ProfileDetail();
            profileDetail2.Description = EmailLabel;
            profileDetail2.Value = division.Email;

            var profileDetail3 = new ProfileDetail();
            profileDetail3.Description = MobileLabel;
            profileDetail3.Value = division.ContactNumber;

            var details = new[] { profileDetail1, profileDetail2, profileDetail3 };
            profileDetails.AddRange(details);

            ProfileDetails = new ProfileDetailsModel(profileDetails);
            DivisionHeader = new DivisionHeaderModel(division.Name, division.UserCount, division.Description, division.Picture, CloseEvent);
            OrganizationInfo = new OrganizationInfoModel(division.OrgName, division.OrgPic, OpenOrganizationCommand);

            Division = division;
        }

        private void CloseEvent(object sender, EventArgs e) => CloseView();

        private Task OpenOrganization()
        {
            throw new NotImplementedException();
        }

        private async Task SendEmail()
        {
            throw new NotImplementedException();
        }

        private async Task Call()
        {
            throw new NotImplementedException();
        }

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }

        private bool CanExecute() => !IsBusy;

        #region Resources

        public string Title => L10N.Localize("Division_Title");
        public string SendEmailLabel => L10N.Localize("Division_SendEmail");
        public string CallLabel => L10N.Localize("Division_Call");
        public string MembersLabel => L10N.Localize("Division_Members");
        public string OrganizationLabel => L10N.Localize("Division_Organization");

        private string AddressLabel => L10N.Localize("Division_Address");
        private string EmailLabel => L10N.Localize("Division_Email");
        private string MobileLabel => L10N.Localize("Division_Mobile");

        #endregion
    }
}
