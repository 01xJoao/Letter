using System;
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
    public class CallViewModel : XViewModel<Tuple<int, bool>>
    {
        private IDialogService _dialogService;
        private IMemberService _memberService;

        public int OpenSettings { get; private set; }
        public int CallerId { get; set; }
        public bool StartedCall { get; set; }
        public int UserId => AppSettings.UserId;
        public string RoomName { get; private set; }
        public string MemberFullName { get; private set; }

        private bool _mutedOn;
        public bool MutedOn 
        {             
            get => _mutedOn;
            set => SetProperty(ref _mutedOn, value);
        }
        public bool SpeakerOn { get; set; }

        private MembersProfileModel _memberProfileModel;
        public MembersProfileModel MemberProfileModel
        {
            get => _memberProfileModel;
            set => SetProperty(ref _memberProfileModel, value);
        }

        private XPCommand _leftButtonCommand;
        public XPCommand LeftButtonCommand => _leftButtonCommand ?? (_leftButtonCommand = new XPCommand(LeftButtonAction));

        private XPCommand<bool> _rightButtonCommand;
        public XPCommand<bool> RightButtonCommand => _rightButtonCommand ?? (_rightButtonCommand = new XPCommand<bool>((value) => RightButtonAction(value)));

        private XPCommand _endCallCommand;
        public XPCommand EndCallCommand => _endCallCommand ?? (_endCallCommand = new XPCommand(async () => await EndCall(), CanExecute));

        private XPCommand _microphoneAlertCommand;
        public XPCommand MicrophoneAlertCommand => _microphoneAlertCommand ?? (_microphoneAlertCommand = new XPCommand(async () => await MicrophoneAlert()));

        private bool _inCall;
        public bool InCall
        {
            get => _inCall;
            set => SetProperty(ref _inCall, value);
        }

        public CallViewModel(IMemberService memberService, IDialogService dialogService)
        {
            _dialogService = dialogService;
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

            _memberProfileModel = Realm.Find<MembersProfileModel>(CallerId);
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

                    Realm.Write(() => {
                        Realm.Add(result, true);
                    });
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
            SpeakerOn = !SpeakerOn;
        }

        private void RightButtonAction(bool value)
        {
            _mutedOn = value;
        }

        private async Task MicrophoneAlert()
        {
            try
            {
                var result = await _dialogService.ShowQuestion(MicrophoneLabel, OpenSettingsLabel, QuestionType.Bad);
                RaisePropertyChanged(nameof(OpenSettings));
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private async Task EndCall()
        {
            IsBusy = true;
            await NavigationService.Close(this);
        }

        private bool CanExecute() => !IsBusy;

        #region Resources

        private string MicrophoneLabel => L10N.Localize("Call_MicAlert");
        private string OpenSettingsLabel => L10N.Localize("Call_OpenSettingsAlert");

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
