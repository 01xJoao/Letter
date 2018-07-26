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

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class ContactListViewModel : XViewModel
    {
        private IAuthenticationService _authenticationService;
        private IContactsService _contactsService;

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

        private bool _configureView;
        public bool ConfigureView
        {
            get => _configureView;
            set => SetProperty(ref _configureView, value);
        }

        private UserModel _user;

        public ContactListsModel ContactLists { get; set; }
        public List<ContactTabModel> ContactTab { get; set; }

        private List<GetUsersInDivisionModel> _usersInDivision;

        private XPCommand<int> _switchDivisionCommand;
        public XPCommand<int> SwitchDivisionCommand => _switchDivisionCommand ?? (_switchDivisionCommand = new XPCommand<int>((viewIndex) => SettingSwitchDivision(viewIndex)));

        private XPCommand<Tuple<ContactEventType, int>> _contactCommand;
        public XPCommand<Tuple<ContactEventType, int>> ContactCommand => _contactCommand ?? (_contactCommand = new XPCommand<Tuple<ContactEventType, int>>(async (user) => await ContactEvent(user), CanExecute));

        public ContactListViewModel(IContactsService contactsService, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _contactsService = contactsService;
        }

        public override async Task InitializeAsync()
        {
            _user = Realm.Find<UserModel>(AppSettings.UserId);
            SetDivisionTabs(_user);

            var allDivisionsUser = new List<int>();

            foreach (var division in _user.Divisions)
            {
                if (division.IsDivisonActive && division.IsUserInDivisionActive)
                {
                    allDivisionsUser.Add(division.DivisionID);
                }
            }

            //Creates a List with all memebers
            _usersInDivision = new List<GetUsersInDivisionModel>();
            foreach (var user in Realm.All<GetUsersInDivisionModel>())
            {
                foreach (int divisionId in allDivisionsUser)
                {
                    if (user?.DivisionId == divisionId)
                        _usersInDivision.Add(user);
                }
            }

            //Separate the members in diferent lits(divisions)
            ContactLists = new ContactListsModel
            {
                Contacts = SeparateInLists(_usersInDivision)
            };
        }

        public override async Task Appearing()
        {
            try
            {
                var result = await _contactsService.GetUsersFromAllDivisions();

                foreach (var res in result)
                {
                    res.UniqueKey = $"{res.UserId}+{res.DivisionId}";
                    Realm.Write(() => Realm.Add(res, true));
                }

                var shouldUpdateView = false;

                if (ContactLists.Contacts == null || ContactLists?.Contacts?.Count == 0)
                    shouldUpdateView = true;

                ContactLists = new ContactListsModel
                {
                    Contacts = SeparateInLists(result)
                };

                if (ContactTab == null || ContactTab?.Count != ContactLists?.Contacts?.Count || shouldUpdateView)
                {
                    await UpdateUser();
                    shouldUpdateView = true;
                }

                if (shouldUpdateView)
                    RaisePropertyChanged(nameof(ConfigureView));
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
                    await NavigationService.NavigateAsync<MembersProfileViewModel, int>(user.Item2);
                    break;
                default:
                    break;
            }
        }

        public List<List<GetUsersInDivisionModel>> SeparateInLists(List<GetUsersInDivisionModel> source)
        {
            return source
                .OrderBy(o => o.FirstName)
                .GroupBy(s => s.DivisionId)
                .OrderBy(g => g.Key)
                .Select(g => g.ToList())
                .ToList();
        }

        private bool CanExecute(object value) => !IsBusy;

        public enum ContactEventType
        {
            Call,
            Chat,
            OpenProfile
        }

        #region Resources

        public string Title => L10N.Localize("Contacts_Title");

        #endregion
    }
}
