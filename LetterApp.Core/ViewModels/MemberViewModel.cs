using System;
using System.Threading.Tasks;
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

        public MemberDetailsModel MemberDetails { get; private set; }

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

        public override async Task InitializeAsync()
        {

        }

        public override async Task Appearing()
        {

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
