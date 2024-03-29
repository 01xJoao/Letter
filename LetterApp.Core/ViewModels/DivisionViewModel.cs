﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
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
        private readonly IDialogService _dialogService;
        private readonly IDivisionService _divisionService;

        private int _divisionId;

        private DivisionModelProfile _division;
        public DivisionModelProfile Division
        {
            get => _division;
            set => SetProperty(ref _division, value);
        }

        public OrganizationInfoModel OrganizationInfo { get; set; }
        public ProfileDetailsModel ProfileDetails { get; private set; }

        private XPCommand _sendEmailCommand;
        public XPCommand SendEmailCommand => _sendEmailCommand ?? (_sendEmailCommand = new XPCommand(async () => await SendEmail()));

        private XPCommand _callCommand;
        public XPCommand CallCommand => _callCommand ?? (_callCommand = new XPCommand(async () => await Call()));

        private XPCommand _openOrganizationCommand;
        public XPCommand OpenOrganizationCommand => _openOrganizationCommand ?? (_openOrganizationCommand = new XPCommand(async () => await OpenOrganization(), CanExecute));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public DivisionViewModel(IDivisionService divisionService, IDialogService dialogService) 
        {
            _dialogService = dialogService;
            _divisionService = divisionService;
        }

        protected override void Prepare(int divisionId)
        {
            _divisionId = divisionId;
        }

        public override async Task Appearing()
        {
            _division = Realm.Find<DivisionModelProfile>(_divisionId);
            SetupModels(_division);

            if(_division == null)
                _dialogService.StartLoading();

            try
            {
                var result = await _divisionService.GetDivisionProfile(_divisionId);

                if(result.StatusCode == 200)
                {
                    var shouldUpdateView = false;

                    if (result.LastUpdateTicks > _division?.LastUpdateTicks || _division == null)
                        shouldUpdateView = true;

                    _division = result;
                     Realm.Write(() => 
                    {
                        Realm.Add(_division, true);                     });

                    if(shouldUpdateView)
                        SetupModels(_division);
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                _dialogService.StopLoading();
            }
        }

        private void SetupModels(DivisionModelProfile division)
        {
            if (division == null)
                return;

            var profileDetails = new List<ProfileDetail>();

            if(!string.IsNullOrEmpty(division.Address))
            {
                var profileDetail1 = new ProfileDetail();
                profileDetail1.Description = AddressLabel;
                profileDetail1.Value = division.Address;

                profileDetails.Add(profileDetail1);
            }

            if (!string.IsNullOrEmpty(division.Email))
            {
                var profileDetail2 = new ProfileDetail();
                profileDetail2.Description = EmailLabel;
                profileDetail2.Value = division.Email;
                profileDetails.Add(profileDetail2);
            }

            if (!string.IsNullOrEmpty(division.ContactNumber))
            {
                var profileDetail3 = new ProfileDetail();
                profileDetail3.Description = MobileLabel;
                profileDetail3.Value = division.ContactNumber;
                profileDetails.Add(profileDetail3);
            }

            ProfileDetails = new ProfileDetailsModel(profileDetails);
            OrganizationInfo = new OrganizationInfoModel(division.OrgName, division.OrgPic, OrganizationLabel, OpenOrganizationCommand);

            RaisePropertyChanged(nameof(Division));
        }

        private void CloseEvent(object sender, EventArgs e) => CloseView();

        private async Task OpenOrganization()
        {
            await NavigationService.NavigateAsync<OrganizationViewModel, int>(Division.OrgID);
        }

        private async Task SendEmail()
        {
            await EmailUtils.SendEmail(_division.Email);
        }

        private async Task Call()
        {
            CallUtils.Call(_division.ContactNumber);
        }

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }

        public override async Task Disappearing()
        {
            _dialogService.StopLoading();
        }

        private bool CanExecute() => !IsBusy;

        #region Resources

        public string SendEmailLabel => L10N.Localize("Division_SendEmail");
        public string CallLabel => L10N.Localize("Division_Call");
        public string MembersLabel => L10N.Localize("Division_Members");
        public string DivisionNoDescription => L10N.Localize("Division_NoDescription");

        private string OrganizationLabel => L10N.Localize("Division_Organization");
        private string AddressLabel => L10N.Localize("Division_Address");
        private string EmailLabel => L10N.Localize("Division_Email");
        private string MobileLabel => L10N.Localize("Division_Mobile");

        #endregion
    }
}
