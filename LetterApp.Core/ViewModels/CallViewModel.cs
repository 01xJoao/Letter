using System;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models.DTO.ReceivedModels;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels
{
    public class CallViewModel : XViewModel<Tuple<int, bool>>
    {
        private IDialogService _dialogService;
        private IMemberService _memberService;
        private ISettingsService _settingsService;
        private IMessengerService _messengerService;

        public bool EndCallForSettings { get; set; }
        public int CallerId { get; set; }
        public bool StartedCall { get; set; }
        public int UserId => AppSettings.UserId;
        private string _thisUserName;
        private string _callerPushToken;
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

        private XPCommand _sendPushFailedCallCommand;
        public XPCommand SendPushFailedCallCommand => _sendPushFailedCallCommand ?? (_sendPushFailedCallCommand = new XPCommand(SendFailedCallNotification));

        public CallViewModel(IMemberService memberService, IDialogService dialogService, ISettingsService settingsService, IMessengerService messengerService)
        {
            _dialogService = dialogService;
            _memberService = memberService;
            _settingsService = settingsService;
            _messengerService = messengerService;
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
            {
                RoomName = $"RoomId-{AppSettings.UserId}{CallerId}";

                var user = Realm.Find<UserModel>(UserId);
                _thisUserName = $"{user.FirstName} {user.LastName}";
                var users = Realm.All<GetUsersInDivisionModel>();
                var userCall = users.First(x => x.UserId == CallerId);
                _callerPushToken = userCall?.PushNotificationToken;
            }
            else
                RoomName = $"RoomId-{CallerId}{AppSettings.UserId}";

            _memberProfileModel = Realm.Find<MembersProfileModel>(CallerId);
            MemberFullName = $"{_memberProfileModel?.FirstName} {_memberProfileModel?.LastName}";
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
            if (_settingsService.CheckMicrophonePermissions())
                return;

            try
            {
                RaisePropertyChanged(nameof(EndCallForSettings));
                var result = await _dialogService.ShowQuestion(MicrophoneLabel, OpenSettingsLabel, QuestionType.Bad);

                if(result)
                    _settingsService.OpenSettings();
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

        private void SendFailedCallNotification()
        {
            _messengerService.SendPushNotification(UserId.ToString(), _thisUserName, _callerPushToken, string.Empty, NotificationType.Call);
        }

        private bool CanExecute() => !IsBusy;

        #region Resources

        private string MicrophoneLabel => L10N.Localize("Call_MicAlert");
        private string OpenSettingsLabel => L10N.Localize("Call_OpenSettingsAlert");
        //private string FailedCall => L10N.Localize("Call_FailedCall");

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
