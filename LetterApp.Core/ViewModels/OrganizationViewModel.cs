﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    public class OrganizationViewModel : XViewModel<int>
    {
        private readonly IDialogService _dialogService;
        private readonly IOrganizationService _organizationService;

        private int _organizationId;

        public ProfileDetailsModel ProfileDetails { get; private set; }
        public ProfileOrganizationModel ProfileOrganization { get; private set; }

        private OrganizationModel _organization;
        public OrganizationModel Organization
        {
            get => _organization;
            set => SetProperty(ref _organization, value);
        }

        private XPCommand _sendEmailCommand;
        public XPCommand SendEmailCommand => _sendEmailCommand ?? (_sendEmailCommand = new XPCommand(async () => await SendEmail()));

        private XPCommand _callCommand;
        public XPCommand CallCommand => _callCommand ?? (_callCommand = new XPCommand(async () => await Call()));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));


        public OrganizationViewModel(IOrganizationService organizationService, IDialogService dialogService) 
        {
            _dialogService = dialogService;
            _organizationService = organizationService;
        }

        protected override void Prepare(int organizationId)
        {
            _organizationId = organizationId;
        }

        public override async Task Appearing()
        {
            _organization = Realm.Find<OrganizationModel>(_organizationId);
            SetupModels(_organization);

            if(_organization == null)
                _dialogService.StartLoading();

            try
            {
                var result = await _organizationService.GetOrganizationProfile(_organizationId);

                if(result.StatusCode == 200)
                {
                    bool shouldUpdateView = false;

                    if (result.LastUpdateTicks > _organization?.LastUpdateTicks || _organization == null)
                        shouldUpdateView = true;
                    
                    var organization = RealmUtils.UpdateOrganization(Realm, Realm.Find<UserModel>(AppSettings.UserId), result);

                    if(shouldUpdateView)
                    {
                        _organization = organization;
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
                _dialogService.StopLoading();
            }
        }

        private void SetupModels(OrganizationModel organization)
        {
            if (organization == null)
                return;
            
            var organizations = new List<ProfileOrganizationDetails>();

            if (organization.Divisions != null)
            {
                foreach (var division in organization.Divisions)
                {
                    if (division.IsDivisonActive)
                    {
                        var div = new ProfileOrganizationDetails();
                        div.Name = division.Name;
                        div.Picture = division.Picture;
                        div.MembersCount = $"{division.UserCount} {MembersLabel}";
                        organizations.Add(div);
                    }
                }
            }

            ProfileOrganization = new ProfileOrganizationModel(DivisionsLabel, organizations);


            var profileDetails = new List<ProfileDetail>();

            if (!string.IsNullOrEmpty(organization.Address))
            {
                var profileDetail1 = new ProfileDetail();
                profileDetail1.Description = AddressLabel;
                profileDetail1.Value = organization.Address;

                profileDetails.Add(profileDetail1);
            }

            if (!string.IsNullOrEmpty(organization.Email))
            {
                var profileDetail2 = new ProfileDetail();
                profileDetail2.Description = EmailLabel;
                profileDetail2.Value = organization.Email;
                profileDetails.Add(profileDetail2);
            }

            if (!string.IsNullOrEmpty(organization.ContactNumber))
            {
                var profileDetail3 = new ProfileDetail();
                profileDetail3.Description = MobileLabel;
                profileDetail3.Value = organization.ContactNumber;
                profileDetails.Add(profileDetail3);
            }

            ProfileDetails = new ProfileDetailsModel(profileDetails);

            RaisePropertyChanged(nameof(Organization));
        }

        private async Task SendEmail()
        {
            await EmailUtils.SendEmail(_organization.Email);
        }

        private async Task Call()
        {
            CallUtils.Call(_organization.ContactNumber);
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

        public string SendEmailLabel => L10N.Localize("Organization_SendEmail");
        public string CallLabel => L10N.Localize("Organization_Call");
        public string OrganizationNoDescription => L10N.Localize("Organization_NoDescription");
        public string MembersLabel => L10N.Localize("Organization_Members");

        private string DivisionsLabel => L10N.Localize("Organization_Divisions");
        private string AddressLabel => L10N.Localize("Organization_Address");
        private string EmailLabel => L10N.Localize("Organization_Email");
        private string MobileLabel => L10N.Localize("Organization_Mobile");

        #endregion
    }
}
