using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models.DTO.ReceivedModels;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class CallViewModel : XViewModel<Tuple<int, bool>>
    {
        private IMemberService _memberService;

        public int CallerId { get; set; }
        public bool StartedCall { get; set; }
        public int UserId => AppSettings.UserId;
        public string RoomName { get; private set; }
        public string MemberFullName { get; private set; }

        public bool MutedOn { get; set; }
        public bool SpeakerOn { get; set; }

        private MembersProfileModel _memberProfileModel;
        public MembersProfileModel MemberProfileModel
        {
            get => _memberProfileModel;
            set => SetProperty(ref _memberProfileModel, value);
        }

        private XPCommand _leftButtonCommand;
        public XPCommand LeftButtonCommand => _leftButtonCommand ?? (_leftButtonCommand = new XPCommand(() => LeftButtonAction()));

        private XPCommand _rightButtonCommand;
        public XPCommand RightButtonCommand => _rightButtonCommand ?? (_rightButtonCommand = new XPCommand(() => RightButtonAction()));

        private XPCommand _endCallCommand;
        public XPCommand EndCallCommand => _endCallCommand ?? (_endCallCommand = new XPCommand(async () => await EndCall(), CanExecute));

        private bool _inCall;
        public bool InCall
        {
            get => _inCall;
            set => SetProperty(ref _inCall, value);
        }

        public CallViewModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        protected override void Prepare(Tuple<int, bool> call)
        {
            CallerId = call.Item1;
            StartedCall = call.Item2;
        }

        public override async Task InitializeAsync()
        {
            _memberProfileModel = null;

            if (StartedCall)
                RoomName = $"RoomId-{AppSettings.UserId}{CallerId}";
            else
                RoomName = $"RoomId-{CallerId}{AppSettings.UserId}";

            _inCall = false;

            MemberProfileModel = Realm.Find<MembersProfileModel>(CallerId);
            MemberFullName = $"{MemberProfileModel?.FirstName} {MemberProfileModel?.LastName}";
        }

        public override async Task Appearing()
        {
            if (MemberProfileModel != null)
                return;

            try
            {
                var result = await _memberService.GetMemberProfile(CallerId);

                if(result.StatusCode == 200)
                {
                    MemberFullName = $"{result.FirstName} {result.LastName}";
                    MemberProfileModel = result;
                }
                else
                {
                    await EndCall();
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private void LeftButtonAction()
        {
            if (StartedCall || InCall)
            {
                SpeakerOn = !SpeakerOn;
            }
        }

        private void RightButtonAction()
        {
            if (StartedCall || InCall)
            {
                MutedOn = !MutedOn;
            }
            else
            {
                InCall = true;
            }
        }

        private async Task EndCall()
        {
            IsBusy = true;
            await NavigationService.Close(this);
        }

        private bool CanExecute() => !IsBusy;

        #region Resources

        public string LetterLabel       => L10N.Localize("Call_Letter");
        public string CallingLabel      => L10N.Localize("Call_Calling");
        public string IsCallingLabel    => L10N.Localize("Call_IsCalling");
        public string SpeakerLabel      => L10N.Localize("Call_Speaker");
        public string MuteLabel         => L10N.Localize("Call_Mute");
        public string AcceptLabel       => L10N.Localize("Call_Accept");
        public string DeclineLabel      => L10N.Localize("Call_Decline");
        public string ConnectingLabel   => L10N.Localize("Call_Connecting");

        #endregion
    }
}
