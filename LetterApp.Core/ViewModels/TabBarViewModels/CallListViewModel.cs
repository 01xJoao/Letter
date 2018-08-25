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

        private List<CallModel> _calls;

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

        public override async Task Appearing()
        {
            CallHistory = new List<CallHistoryModel>();

            _calls = Realm.All<CallModel>().ToList();

            var users = Realm.All<GetUsersInDivisionModel>().ToList();

            var lastCall = new CallHistoryModel();

            var testCall = new CallModel();
            testCall.CallerId = 59;
            testCall.CallDate = DateTime.Now.Ticks;
            testCall.CallId = 1;
            testCall.IsNew = true;
            testCall.CallType = 0;
            testCall.Success = true;

            var testCall1 = new CallModel();
            testCall1.CallerId = 84;
            testCall1.CallDate = DateTime.Now.AddDays(-1).Ticks;
            testCall1.CallId = 2;
            testCall1.IsNew = false;
            testCall1.CallType = 1;
            testCall1.Success = false;

            var testCalls = new List<CallModel>() { testCall, testCall1 };

            foreach (CallModel call in testCalls)
            {
                var user = users.FirstOrDefault(x => x.UserId == call.CallerId);

                if (user == null)
                    continue;

                var date = new DateTime(call.CallDate);

                if (lastCall.CallerId == call.CallerId && DateUtils.CompareDates(call.CallDate, lastCall.CallDate))
                {
                    lastCall.CallDateText = DateUtils.CallsDateString(date);
                    lastCall.NumberOfCalls++;
                    continue;
                }

                var callHistory = new CallHistoryModel
                {
                    CallerId = call.CallerId,
                    CallDate = date,
                    CallDateText = DateUtils.CallsDateString(date),
                    CallType = call.CallType == 0 ? Call_Outgoing : Call_Incoming,
                    HasSuccess = call.Success,
                    IsNew = call.IsNew,
                    NumberOfCalls = 1,
                    CallerInfo = $"{user.FirstName} {user.LastName} · {user.Position}",
                    CallerPicture = user.Picture
                };

                _callHistory.Add(callHistory);

                lastCall = callHistory;
            }

            CallHistory = _callHistory;

            //TODO This might need to be moved to Appeared or Disappering
            Realm.Write(() =>
            {
                _calls.All(x => x.IsNew = false);
            });
        }

        #region Resources

        public string Title => L10N.Localize("MainViewModel_CallTab");

        private string Call_Incoming => L10N.Localize("Call_Incoming");
        private string Call_Outgoing => L10N.Localize("Call_Outgoing");

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
