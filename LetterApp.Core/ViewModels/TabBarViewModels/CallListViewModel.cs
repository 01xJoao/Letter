using System;
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
using Xamarin.Essentials;
using static LetterApp.Core.ViewModels.TabBarViewModels.ContactListViewModel;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class CallListViewModel : XViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IContactsService _contactsService;

        public bool NoCalls { get; private set; }

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

        private XPCommand<int> _callCommand;
        public XPCommand<int> CallCommand => _callCommand ?? (_callCommand = new XPCommand<int>(async (callerId) => await Call(callerId)));

        private XPCommand<int> _openCallerProfileCommand;
        public XPCommand<int> OpenCallerProfileCommand => _openCallerProfileCommand ?? (_openCallerProfileCommand = new XPCommand<int>(OpenCallerProfile));

        private XPCommand _openContactList;
        public XPCommand OpenCallListCommand => _openContactList ?? (_openContactList = new XPCommand(OpenContactList));

        private XPCommand<int> _deleteCallCommand;
        public XPCommand<int> DeleteCallCommand => _deleteCallCommand ?? (_deleteCallCommand = new XPCommand<int>(DeleteCall));

        private XPCommand<int> _callStackCommand;
        public XPCommand<int> CallStackCommand => _callStackCommand ?? (_callStackCommand = new XPCommand<int>(ShowCallStack));

        public CallListViewModel(IContactsService contactsService, IDialogService dialogService) 
        {
            _contactsService = contactsService;
            _dialogService = dialogService;
            SetL10NResources();
        }
        public override async Task Appearing()
        {
            //if (_callHistory != null && _calls != null && _callHistory.Count == _calls.Count)
            //return;
            if (_users == null || _users.Count == 0)
                await GetUsers();

            _callHistory = new List<CallHistoryModel>();

            _calls = Realm.All<CallModel>().ToList();

            var lastCall = new CallHistoryModel();

            foreach (CallModel call in _calls.OrderBy(x => x.CallDate))
            {
                var user = _users.FirstOrDefault(x => x.UserId == call.CallerId);

                if (user == null)
                    continue;

                var date = new DateTime(call.CallDate);

                if (lastCall.CallerId == call.CallerId && DateUtils.CompareDates(call.CallDate, lastCall.CallDate))
                {
                    //if (lastCall.HasSuccess == call.Success && (int)lastCall.CallType == call.CallType)
                    //{
                    lastCall.CallDateText = DateUtils.TimePassed(date);
                    lastCall.CallStack.Add(call.CallId);
                    lastCall.CallCountAndType = call.CallType == 0 ? $"{Call_Outgoing} ({lastCall.CallStack.Count()})" :
                        call.Success ? $"{Call_Incoming} ({lastCall.CallStack.Count()})" : $"{Call_Missed} ({lastCall.CallStack.Count()})";

                    //new (remove this to back to original)
                    lastCall.HasSuccess = call.Success;
                    lastCall.CallDate = date;
                    lastCall.CallType = call.CallType == 0 ? CallType.Outgoing : CallType.Incoming;
                    lastCall.ShouldAlert = call.IsNew && !call.Success && call.CallType == 1;

                    continue;
                    // }

                    //if(lastCall.CallType == CallType.Incoming)
                    //lastCall.ShouldAlert = false;
                }

                var callHistory = new CallHistoryModel
                {
                    CallerId = call.CallerId,
                    CallDate = date,
                    CallDateText = DateUtils.TimePassed(date),
                    CallType = call.CallType == 0 ? CallType.Outgoing : CallType.Incoming,
                    CallCountAndType = call.CallType == 0 ? Call_Outgoing : call.Success ? Call_Incoming : Call_Missed,
                    HasSuccess = call.Success,
                    ShouldAlert = call.IsNew && !call.Success && call.CallType == 1,
                    CallerInfo = $"{user.FirstName} {user.LastName} - {user.Position}",
                    CallerPicture = user.Picture
                };

                callHistory.CallStack.Add(call.CallId);

                _callHistory.Add(callHistory);

                lastCall = callHistory;
            }

            _callHistory = _callHistory.OrderByDescending(x => x.CallDate).ToList();

            if (_callHistory.Count > 0)
                RaisePropertyChanged(nameof(CallHistory));

            Realm.Write(() =>
            {
                foreach (var call in _calls)
                {
                    if (call.IsNew)
                    {
                        call.IsNew = false;
                        Realm.Add(call, true);
                    }
                }
            });
        }

        private async Task GetUsers()
        {
            try
            {
                _users = Realm.All<GetUsersInDivisionModel>().ToList();

                if (_users.Count == 0)
                {
                    var result = await _contactsService.GetUsersFromAllDivisions();

                    if (result == null && result.Count == 0)
                        return;

                    Realm.Write(() => {
                        foreach (var res in result)
                        {
                            res.UniqueKey = $"{res.UserId}+{res.DivisionId}";
                            var contacNumber = res.ShowNumber ? res?.ContactNumber : string.Empty;
                            string[] stringSearch = { res?.FirstName?.ToLower(), res?.LastName?.ToLower(), res?.Position?.ToLower() };
                            stringSearch = StringUtils.NormalizeString(stringSearch);
                            res.SearchContainer = $"{stringSearch[0]}, {stringSearch[1]}, {stringSearch[2]}, {contacNumber} {res?.Email?.ToLower()}";

                            Realm.Add(res, true);
                        }
                    });

                    _users = Realm.All<GetUsersInDivisionModel>().ToList();
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private void DeleteCall(int index)
        {
            var listCalls = new List<CallModel>();

            foreach (string callId in _callHistory[index]?.CallStack)
            {
                var call = _calls.Find(x => x.CallId == callId);

                if (call != null)
                    listCalls.Add(call);
            }

            Realm.Write(() => {
                foreach (CallModel call in listCalls)
                    Realm.Remove(call);
            });

            _calls = Realm.All<CallModel>().ToList();

            if (_callHistory.ElementAtOrDefault(index) != null)
                _callHistory.RemoveAt(index);
            else
                _callHistory.Remove(_callHistory.LastOrDefault());

            if(_callHistory.Count == 0)
                RaisePropertyChanged(nameof(NoCalls));
        }

        private void ShowCallStack(int callId)
        {
            var listCallStack = new List<CallStackModel>();
            List<string> callStackIds = new List<string>(); 

            foreach (string callID in _callHistory[callId]?.CallStack)
            {
                var call = _calls.Find(x => x.CallId == callID);

                if (call != null)
                {
                    var callStack = new CallStackModel
                    {
                        CallType = call.CallType == 0 ? Call_Outgoing : call.Success ? Call_Incoming : Call_Missed,
                        Date = new DateTime(call.CallDate).ToShortTimeString(),
                        Successful = call.Success
                    };

                    listCallStack.Add(callStack);
                }
            }

            listCallStack.Reverse();

            _dialogService.ShowCallStack(_callHistory[callId]?.CallDate.ToLongDateString(), listCallStack);
        }

        private void OpenContactList()
        {
            NavigationService.NavigateAsync<ContactListViewModel, ContactsType>(ContactsType.Call);
        }

        private void OpenCallerProfile(int user)
        {
            NavigationService.NavigateAsync<MemberViewModel, int>(user);
        }

        private async Task Call(int userId)
        {
            var user = _users.FirstOrDefault(x => x.UserId == userId);

            if (user == null)
                return;

            try
            {
                var callType = await _dialogService.ShowContactOptions(LocationResources, user.ShowNumber);

                switch (callType)
                {
                    case CallingType.Letter:
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                            await NavigationService.NavigateAsync<CallViewModel, Tuple<int, bool>>(new Tuple<int, bool>(user.UserId, true));
                        break;
                    case CallingType.Cellphone:
                        CallUtils.Call(user.ContactNumber);
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

        #region Resources

        public string Title => L10N.Localize("MainViewModel_CallTab");
        public string Delete => L10N.Localize("Delete");
        public string NoRecentCalls => L10N.Localize("Calls_NoRecentCalls");

        public string CallActionInfo => L10N.Localize("Calls_CallActionInfo");
        //public string CallStackTitle => L10N.Localize("Calls_CallStackTitle");

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


//var testCall1 = new CallModel();
//testCall1.CallerId = 92;
//testCall1.CallDate = DateTime.Now.AddDays(-1).Ticks;
//testCall1.CallId = 11;
//testCall1.IsNew = true;
//testCall1.CallType = 1;
//testCall1.Success = false;

//var testCall0 = new CallModel();
//testCall0.CallerId = 92;
//testCall0.CallDate = DateTime.Now.AddDays(-1).AddHours(-1).Ticks;
//testCall0.CallId = 21;
//testCall0.IsNew = true;
//testCall0.CallType = 1;
//testCall0.Success = false;

//var testCall2 = new CallModel();
//testCall2.CallerId = 59;
//testCall2.CallDate = DateTime.Now.AddDays(-2).Ticks;
//testCall2.CallId = 1;
//testCall2.IsNew = true;
//testCall2.CallType = 0;
//testCall2.Success = true;

//var testCall3 = new CallModel();
//testCall3.CallerId = 86;
//testCall3.CallDate = DateTime.Now.AddDays(-7).Ticks;
//testCall3.CallId = 2;
//testCall3.IsNew = false;
//testCall3.CallType = 1;
//testCall3.Success = false;

//var testCalls = new List<CallModel>() { testCall0, testCall1, testCall2, testCall3 };