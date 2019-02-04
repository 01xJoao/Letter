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
    public class ContactListViewModel : XViewModel<ContactsType>
    {
        private IAuthenticationService _authenticationService;
        private IContactsService _contactsService;
        private IDialogService _dialogService;

        public ContactsType IsPresentingCustomView { get; private set; }
        public bool IsFilteredByName { get; private set; }

        private bool _isFilterActive;
        private DateTime _lastContactsUpdate;

        private bool _updateTabBar;
        public bool UpdateTabBar
        {
            get => _updateTabBar;
            set => SetProperty(ref _updateTabBar, value);
        }

        private bool _updateView;
        public bool UpdateView
        {
            get => _updateView;
            set => SetProperty(ref _updateView, value);
        }

        private bool _isSearching;
        public bool IsSearching
        {
            get => _isSearching;
            set => SetProperty(ref _isSearching, value);
        }

        private bool _configureView;
        public bool ConfigureView
        {
            get => _configureView;
            set => SetProperty(ref _configureView, value);
        }

        private UserModel _user;
        private List<int> _allDivisionsUser;

        public ContactListsModel ContactLists { get; set; }
        public List<ContactTabModel> ContactTab { get; set; }

        private List<List<GetUsersInDivisionModel>> _userDivisions;

        private List<GetUsersInDivisionModel> _usersInDivision;
        private List<GetUsersInDivisionModel> _unfilteredUsers;

        private XPCommand _filterCommand;
        public XPCommand FilterCommand => _filterCommand ?? (_filterCommand = new XPCommand(async () => await Filter()));

        private XPCommand<string> _searchCommand;
        public XPCommand<string> SearchCommand => _searchCommand ?? (_searchCommand = new XPCommand<string>((search) => Search(search)));

        private XPCommand<int> _switchDivisionCommand;
        public XPCommand<int> SwitchDivisionCommand => _switchDivisionCommand ?? (_switchDivisionCommand = new XPCommand<int>((viewIndex) => SettingSwitchDivision(viewIndex)));

        private XPCommand<Tuple<ContactEventType, int>> _contactCommand;
        public XPCommand<Tuple<ContactEventType, int>> ContactCommand => _contactCommand ?? (_contactCommand = new XPCommand<Tuple<ContactEventType, int>>(async (user) => await ContactEvent(user), CanExecute));

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public ContactListViewModel(IContactsService contactsService, IAuthenticationService authenticationService, IDialogService dialogService)
        {
            _authenticationService = authenticationService;
            _contactsService = contactsService;
            _dialogService = dialogService;
            SetL10NResources();
        }

        protected override void Prepare(ContactsType contactsType = ContactsType.All)
        {
            IsPresentingCustomView = contactsType;
        }

        public override async Task InitializeAsync()
        {
            IsFilteredByName = AppSettings.FilterByName;

            _lastContactsUpdate = default(DateTime);
            _isFilterActive = AppSettings.FilterByMainDivision;
            _user = Realm.Find<UserModel>(AppSettings.UserId);

            SetDivisionTabs(_user);

            //Creates a List with all memebers
            _usersInDivision = new List<GetUsersInDivisionModel>();

            foreach (var user in Realm.All<GetUsersInDivisionModel>())
            {
                foreach (int divisionId in _allDivisionsUser)
                {
                    if(divisionId == user.DivisionId)
                        _usersInDivision.Add(user);
                }
            }

            SetContactList(_usersInDivision);
        }

        public override async Task Appearing()
        {
            if (DateTime.Now < _lastContactsUpdate && _user.Divisions.Where(x => x.IsUserInDivisionActive).Count() == _allDivisionsUser.Count)
                return;
            
            try
            {
                var shouldUpdateView = false;

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

                if (ContactLists.Contacts == null || ContactLists?.Contacts?.Count == 0)
                    shouldUpdateView = true;

                if (_unfilteredUsers.Count != result.Count)
                {
                    Realm.Write(() => 
                    {
                        foreach (var user in Realm.All<GetUsersInDivisionModel>())
                        {
                            var removeUser = true;

                            foreach (var res in result)
                            {
                                if (user.UserId == res.UserId)
                                {
                                    removeUser = false;
                                }
                            }

                            if (removeUser)
                                Realm.Remove(user);
                        }
                    });

                    shouldUpdateView = true;
                }

                if (ContactTab == null || ContactTab?.Count == 0 || shouldUpdateView)
                {
                    shouldUpdateView = true;
                    await UpdateUser();
                }

                SetContactList(result);

                if (shouldUpdateView)
                    RaisePropertyChanged(nameof(ConfigureView));

                _lastContactsUpdate = DateTime.Now.AddMinutes(10);
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private async Task UpdateUser()
        {
            try
            {
                var result = await _authenticationService.CheckUser();

                if (result.StatusCode == 200)
                {
                    var user = RealmUtils.UpdateUser(Realm, result);

                    SetDivisionTabs(user);
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private void SetDivisionTabs(UserModel user)
        {
            ContactTab = new List<ContactTabModel>();

            int dividionIndex = 0;
            foreach (var division in user.Divisions)
            {
                if (division.IsUserInDivisionActive && division.IsDivisonActive)
                {
                    var tab = new ContactTabModel(dividionIndex == 0, division.Name, division.DivisionID, dividionIndex, SwitchDivision);
                    ContactTab.Add(tab);
                    dividionIndex++;
                }
            }

            _userDivisions = new List<List<GetUsersInDivisionModel>>();
            _allDivisionsUser = new List<int>();
            foreach (var division in user.Divisions)
            {
                if (division.IsDivisonActive && division.IsUserInDivisionActive)
                {
                    _allDivisionsUser.Add(division.DivisionID);
                    _userDivisions.Add(new List<GetUsersInDivisionModel>());
                }
            }
        }

        private void SettingSwitchDivision(int viewIndex)
        {
            object boolObject = false;
            SwitchDivision(boolObject, viewIndex);
        }

        private void SwitchDivision(object shouldUpdateView, int division)
        {
            var tab = ContactTab.Find(x => x.IsSelected == true);
            tab.IsSelected = false;

            var tabSelected = ContactTab.Find(x => x.DivisionIndex == division);
            tabSelected.IsSelected = true;

            if ((bool)shouldUpdateView)
                RaisePropertyChanged(nameof(UpdateView));
            else
                RaisePropertyChanged(nameof(UpdateTabBar));
        }

        private async Task ContactEvent(Tuple<ContactEventType, int> user)
        {
            switch (user.Item1)
            {
                case ContactEventType.OpenProfile:
                    if (IsPresentingCustomView == ContactsType.All)
                        await NavigationService.NavigateAsync<MemberViewModel, int>(user.Item2);
                    else if (IsPresentingCustomView == ContactsType.Call)
                        ShowContactDialog(user.Item2);
                    else
                        OpenChatView(user.Item2);
                    break;
                case ContactEventType.Call:
                    ShowContactDialog(user.Item2);
                    break;
                case ContactEventType.Chat:
                    OpenChatView(user.Item2);
                    break;
                default:
                    break;
            }
        }

        private async Task ShowContactDialog(int userId)
        {
            var user = _usersInDivision.FirstOrDefault(x => x.UserId == userId);

            if (user == null)
                return;

            try
            {
                var callType = await _dialogService.ShowContactOptions(LocationResources, user.ShowNumber);

                switch (callType)
                {
                    case CallingType.Letter:
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        {
                            if (IsPresentingCustomView == ContactsType.Call)
                                await CloseView();

                            await NavigationService.NavigateAsync<CallViewModel, Tuple<int, bool>>(new Tuple<int, bool>(user.UserId, true));
                        }
                        break;
                    case CallingType.Cellphone:
                        CallUtils.Call(user.ContactNumber);

                        if (IsPresentingCustomView == ContactsType.Call)
                            await CloseView();
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

        private async Task OpenChatView(int userId)
        {
            await NavigationService.NavigateAsync<ChatViewModel, int>(userId);

            if (IsPresentingCustomView == ContactsType.Chat)
                await CloseView();
        }

        private async Task Filter()
        {
            List<ContactDialogFilter> filterList = new List<ContactDialogFilter>()
            {
                new ContactDialogFilter(DialogFilterName, IsFilteredByName, DialogFilterNameDescription),
                new ContactDialogFilter(DialogSwitchLabel, _isFilterActive, DialogDescription)
            };

            try
            {
                var result = await _dialogService.ShowFilter(DialogTitle, filterList, DialogButton);

                if (result.Item1 != IsFilteredByName)
                {
                    AppSettings.FilterByName = result.Item1;
                    IsFilteredByName = result.Item1;
                    RaisePropertyChanged(nameof(ConfigureView));
                }

                if (result.Item2 != _isFilterActive)
                {
                    AppSettings.FilterByMainDivision = result.Item2;
                    _isFilterActive = result.Item2;
                    SetContactList(_unfilteredUsers);
                    RaisePropertyChanged(nameof(IsSearching));
                }

            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private void Search(string search)
        {
            if (search == SearchLabel)
                return;
            
            string[] searchSplit = search.ToLower().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            searchSplit = StringUtils.NormalizeString(searchSplit);

             var users = new List<GetUsersInDivisionModel>();

            if (string.IsNullOrEmpty(search))
                users = _usersInDivision;
            else
                users = _usersInDivision.FindAll(x => searchSplit.All(s => x.SearchContainer.Contains(s)));

            SetContactList(users, true);

            RaisePropertyChanged(nameof(IsSearching));
        }

        private void SetContactList(List<GetUsersInDivisionModel> users, bool isSearching = false)
        {
            users = users.OrderBy(x => x?.FirstName).ToList();

            if(!isSearching)
                _unfilteredUsers = new List<GetUsersInDivisionModel>();

            int divisionIndex = 0;
            foreach (var divion in _userDivisions)
            {
                divion.Clear();

                foreach (var user in users)
                {
                    if (user.DivisionId == _allDivisionsUser[divisionIndex])
                    {
                        if (_isFilterActive)
                        {
                            if (user.DivisionId == user.MainDivisionId)
                                divion.Add(user);
                        }
                        else 
                            divion.Add(user);

                        if (!isSearching)
                            _unfilteredUsers.Add(user);
                    }
                }

                divisionIndex++;
            }

            ContactLists = new ContactListsModel
            {
                Contacts = _userDivisions
            };

            if (!isSearching)
                _usersInDivision = users;
        }

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }

        private bool CanExecute(object value) => !IsBusy;

        private bool CanExecute() => !IsBusy;

        public enum ContactEventType
        {
            Call,
            Chat,
            OpenProfile
        }

        #region Resources

        public string Title => L10N.Localize("Contacts_Title");
        public string SearchLabel => L10N.Localize("Contacts_Search");
        public string NewCallTitle => L10N.Localize("Contacts_NewCall");
        public string NewChatTitle => L10N.Localize("Contacts_NewChat");
        public string Cancel => L10N.Localize("Cancel");

        private string DialogTitle => L10N.Localize("Contacts_DialogTitle");

        private string DialogSwitchLabel => L10N.Localize("Contacts_DialogSwitchLabel");
        private string DialogDescription => L10N.Localize("Contacts_DialogDescription");

        private string DialogFilterName => L10N.Localize("Contacts_DialogFilterByName");
        private string DialogFilterNameDescription => L10N.Localize("Contacts_DialogFilterByNameDescription");

        private string DialogButton => L10N.Localize("Contacts_DialogButton");

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

        public enum ContactsType
        {
            All,
            Chat,
            Call
        }

        #endregion
    }
}
