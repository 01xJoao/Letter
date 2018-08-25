using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Helpers;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class CallListViewModel : XViewModel
    {
        private readonly IDialogService _dialogService;

        //Realms fields
        private List<CallModel> _calls;
        private List<GetUsersInDivisionModel> _users;

        //Model
        private List<CallHistoryModel> _callHistory;
        public List<CallHistoryModel> CallHistory
        {
            get => _callHistory;
            set => SetProperty(ref _callHistory, value);
        }

        public CallListViewModel(IDialogService dialogService) 
        {
            _dialogService = dialogService;
            SetL10NResources();
        }

        public override async Task InitializeAsync()
        {
            _users = Realm.All<GetUsersInDivisionModel>().ToList();
        }

        public override async Task Appearing()
        {
            if (_callHistory != null && _calls != null && _callHistory.Count == _calls.Count)
                return;

            _callHistory = new List<CallHistoryModel>();

            _calls = Realm.All<CallModel>().ToList();

            var lastCall = new CallHistoryModel();

            var testCall1 = new CallModel();
            testCall1.CallerId = 92;
            testCall1.CallDate = DateTime.Now.AddDays(-1).Ticks;
            testCall1.CallId = 11;
            testCall1.IsNew = true;
            testCall1.CallType = 1;
            testCall1.Success = false;

            var testCall0 = new CallModel();
            testCall0.CallerId = 92;
            testCall0.CallDate = DateTime.Now.AddDays(-1).AddHours(-4).Ticks;
            testCall0.CallId = 21;
            testCall0.IsNew = true;
            testCall0.CallType = 1;
            testCall0.Success = false;

            var testCall2 = new CallModel();
            testCall2.CallerId = 59;
            testCall2.CallDate = DateTime.Now.AddDays(-2).Ticks;
            testCall2.CallId = 1;
            testCall2.IsNew = true;
            testCall2.CallType = 0;
            testCall2.Success = true;

            var testCall3 = new CallModel();
            testCall3.CallerId = 86;
            testCall3.CallDate = DateTime.Now.AddDays(-7).Ticks;
            testCall3.CallId = 2;
            testCall3.IsNew = false;
            testCall3.CallType = 1;
            testCall3.Success = false;

            var testCalls = new List<CallModel>() { testCall0, testCall1, testCall2, testCall3 };

            foreach (CallModel call in testCalls.OrderBy(x => x.CallDate))
            {
                var user = _users.FirstOrDefault(x => x.UserId == call.CallerId);

                if (user == null)
                    continue;

                var date = new DateTime(call.CallDate);

                if (lastCall.CallerId == call.CallerId && DateUtils.CompareDates(call.CallDate, lastCall.CallDate))
                {
                    if (lastCall.HasSuccess == call.Success && (int)lastCall.CallType == call.CallType)
                    {
                        lastCall.CallDateText = DateUtils.CallsDateString(date);
                        lastCall.NumberOfCalls++;
                        lastCall.CallCountAndType = call.CallType == 0 ? $"{Call_Outgoing} ({lastCall.NumberOfCalls})" :
                            call.Success ? $"{Call_Incoming} ({lastCall.NumberOfCalls})" : $"{Call_Missed} ({lastCall.NumberOfCalls})";
                        continue;
                    }

                    //if(lastCall.CallType == CallType.Incoming)
                        //lastCall.ShouldAlert = false;
                }

                var callHistory = new CallHistoryModel
                {
                    CallerId = call.CallerId,
                    CallDate = date,
                    CallDateText = DateUtils.CallsDateString(date),
                    CallType = call.CallType == 0 ? CallType.Outgoing : CallType.Incoming,
                    CallCountAndType = call.CallType == 0 ? Call_Outgoing : call.Success ? Call_Incoming : Call_Missed,
                    HasSuccess = call.Success,
                    ShouldAlert = call.IsNew && !call.Success && call.CallType == 1,
                    NumberOfCalls = 1,
                    CallerInfo = $"{user.FirstName} {user.LastName} · {user.Position}",
                    CallerPicture = user.Picture
                };

                _callHistory.Add(callHistory);

                lastCall = callHistory;
            }

            _callHistory = _callHistory.OrderByDescending(x => x.CallDate).ToList();

            if(_callHistory.Count > 0)
                RaisePropertyChanged(nameof(CallHistory));
                
            if(_calls.Any(x => x.IsNew == true))
                Realm.Write(() => { _calls.All(x => x.IsNew = false); });
        }

        #region Resources

        public string Title => L10N.Localize("MainViewModel_CallTab");
        public string Delete => L10N.Localize("Delete");

        private string Call_Incoming => L10N.Localize("Call_Incoming");
        private string Call_Outgoing => L10N.Localize("Call_Outgoing");
        private string Call_Missed => L10N.Localize("Call_Missed");

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
