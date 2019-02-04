using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Models.DTO.ReceivedModels;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using Xamarin.Essentials;

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

        private XPCommand _callCommand;
        public XPCommand CallCommand => _callCommand ?? (_callCommand = new XPCommand(async () => await Call()));

        private XPCommand _chatCommand;
        public XPCommand ChatCommand => _chatCommand ?? (_chatCommand = new XPCommand(async () => await Chat()));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public MemberViewModel(IDialogService dialogService, IMemberService memberService)
        {
            _dialogService = dialogService;
            _memberService = memberService;

            SetL10NResources();
        }

        protected override void Prepare(int userId)
        {
            _userId = userId;
        }

        public override async Task Appearing()
        {
            _memberProfileModel = Realm.Find<MembersProfileModel>(_userId);
            SetupModels(_memberProfileModel);

            if (_memberProfileModel == null)
                _dialogService.StartLoading();

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
            finally
            {
                _dialogService.StopLoading();
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

        private async Task Call()
        {
            try
            {
                var callType = await _dialogService.ShowContactOptions(LocationResources, _memberProfileModel.ShowContactNumber);

                switch (callType)
                {
                    case CallingType.Letter:
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                            await NavigationService.NavigateAsync<CallViewModel, Tuple<int, bool>>(new Tuple<int, bool>(_memberProfileModel.UserID, true));
                        break;
                    case CallingType.Cellphone:
                        CallUtils.Call(_memberProfileModel.ContactNumber);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private async Task Chat()
        {
            await NavigationService.NavigateAsync<ChatViewModel, int>(_memberProfileModel.UserID);
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

        public string MemberNoDescriptionLabel => L10N.Localize("Member_NoDescription");
        public string CallLabel => L10N.Localize("Member_Call");
        public string ChatLabel => L10N.Localize("Member_Chat");

        private string Role => L10N.Localize("UserProfile_Role");
        private string Email => L10N.Localize("UserProfile_Email");
        private string Mobile => L10N.Localize("UserProfile_Mobile");
        private string DivisionsLabel => L10N.Localize("UserProfile_Divisions");

        private Dictionary<string, string> LocationResources = new Dictionary<string, string>();
        private string TitleDialog => L10N.Localize("ContactDialog_Title");
        private string LetterDialog => L10N.Localize("ContactDialog_TitleLetter");
        private string LetterDescriptionDialog => L10N.Localize("ContactDialog_DescriptionLetter");
        private string PhoneDialog => L10N.Localize("ContactDialog_TitlePhone");
        private string PhoneDescriptionDialog => L10N.Localize("ContactDialog_DescriptionPhone");

        private void SetL10NResources()
        {
            LocationResources.Add("Title", TitleDialog);
            LocationResources.Add("TitleLetter", LetterDialog);
            LocationResources.Add("DescriptionLetter", LetterDescriptionDialog);
            LocationResources.Add("TitlePhone", PhoneDialog);
            LocationResources.Add("DescriptionPhone", PhoneDescriptionDialog);
        }
        #endregion
    }
}
