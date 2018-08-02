using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Models.DTO.ReceivedModels;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class MemberViewModel : XViewModel<int>
    {
        private IDialogService _dialogService;
        private IMemberService _memberService;

        private int _userId;

        private MembersProfileModel _memberProfileModel;
        public MembersProfileModel MemberProfileModel
        {
            get => _memberProfileModel;
            set => SetProperty(ref _memberProfileModel, value);
        }

        public ProfileDetailsModel MemberDetails { get; private set; }

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public MemberViewModel(IDialogService dialogService, IMemberService memberService)
        {
            _dialogService = dialogService;
            _memberService = memberService;
        }

        protected override void Prepare(int userId)
        {
            _userId = userId;
        }

        public override async Task Appearing()
        {
            _memberProfileModel = Realm.Find<MembersProfileModel>(_userId);
            SetupModels(_memberProfileModel);

            try
            {
                var result = await _memberService.GetMemberProfile(_userId);

                if(result.StatusCode == 200)
                {
                    bool shouldUpdateView = false;

                    if (_memberProfileModel == null || _memberProfileModel.LastUpdateTicks < result.LastUpdateTicks)
                    {
                        shouldUpdateView = true;
                    }

                    Realm.Write(() => Realm.Add(result, true));

                    if(shouldUpdateView)
                    {
                        SetupModels(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private void SetupModels(MembersProfileModel memberProfileModel)
        {
            if (memberProfileModel == null)
                return;

            var profileDetails = new List<ProfileDetail>();

            if (!string.IsNullOrEmpty(memberProfileModel.Position))
            {
                var profileDetail1 = new ProfileDetail();
                profileDetail1.Description = Role;
                profileDetail1.Value = memberProfileModel.Position;
                profileDetails.Add(profileDetail1);
            }

            if (!string.IsNullOrEmpty(memberProfileModel.Email))
            {
                var profileDetail2 = new ProfileDetail();
                profileDetail2.Description = Email;
                profileDetail2.Value = memberProfileModel.Email;
                profileDetails.Add(profileDetail2);
            }

            if (!string.IsNullOrEmpty(memberProfileModel.ContactNumber))
            {
                var profileDetail3 = new ProfileDetail();
                profileDetail3.Description = Mobile;
                profileDetail3.Value = memberProfileModel.ContactNumber;
                profileDetails.Add(profileDetail3);
            }

            if (!string.IsNullOrEmpty(memberProfileModel.Divisions))
            {
                var profileDetail4 = new ProfileDetail();
                profileDetail4.Description = DivisionsLabel;
                profileDetail4.Value = memberProfileModel.Divisions;
                profileDetails.Add(profileDetail4);
            }

            MemberDetails = new ProfileDetailsModel(profileDetails);
            _memberProfileModel = null;
            MemberProfileModel = memberProfileModel;
        }

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }

        private bool CanExecute() => !IsBusy;

        #region Resources

        public string MemberNoDescriptionLabel => L10N.Localize("Member_NoDescription");
        public string CallLabel => L10N.Localize("Member_Call");
        public string ChatLabel => L10N.Localize("Member_Chat");

        private string Role => L10N.Localize("UserProfile_Role");
        private string Email => L10N.Localize("UserProfile_Email");
        private string Mobile => L10N.Localize("UserProfile_Mobile");
        private string DivisionsLabel => L10N.Localize("UserProfile_Divisions");
        #endregion
    }
}
