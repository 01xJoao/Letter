using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Helpers.Commands;
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

        private bool _updateView;
        public bool UpdateView
        {
            get => _updateView;
            set => SetProperty(ref _updateView, value);
        }

        public ContactListsModel ContactLists { get; set; }
        public List<ContactTabModel> ContactTab { get; set; }

        private List<GetUsersInDivisionModel> _usersInDivision;

        private XPCommand<Tuple<ContactEventType, int>> _contactCommand;
        public XPCommand<Tuple<ContactEventType, int>> ContactCommand => _contactCommand ?? (_contactCommand = new XPCommand<Tuple<ContactEventType, int>>(async (user) => await ContactEvent(user), CanExecute));

        public ContactListViewModel(IContactsService contactsService, IAuthenticationService authenticationService) 
        {
            _authenticationService = authenticationService;
            _contactsService = contactsService;
        }

        public override async Task InitializeAsync()
        {
            SetDivisionTabs(Realm.Find<UserModel>(AppSettings.UserId));

            //Creates a List with all memebers
            _usersInDivision = new List<GetUsersInDivisionModel>();
            foreach (var usr in Realm.All<GetUsersInDivisionModel>()){
                _usersInDivision.Add(usr);    
            }

            //Separate the members in diferent lits(divisions)
            ContactLists = new ContactListsModel{
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
                    RaisePropertyChanged(nameof(UpdateView));
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
                    var tab = new ContactTabModel(dividionIndex == 0, division.Name, division.DivisionID, dividionIndex, SwitchDivisionView);
                    ContactTab.Add(tab);
                    dividionIndex++;
                }
            }
        }

        private void SwitchDivisionView(object sender, int division)
        {
            var tab = ContactTab.Find(x => x.IsSelected == true);
            tab.IsSelected = false;

            var tabSelected = ContactTab.Find(x => x.DivisionIndex == division);

            tabSelected.IsSelected = true;
            RaisePropertyChanged(nameof(UpdateView));
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
    }
}
