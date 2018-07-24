using System;
using System.Collections.Generic;
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

        private XPCommand<int> _openChatCommand;
        public XPCommand<int> OpenChatCommand => _openChatCommand ?? (_openChatCommand = new XPCommand<int>(async (userId) => await OpenChat(userId), CanExecute));

        private XPCommand<string> _callCommand;
        public XPCommand<string> CallCommand => _callCommand ?? (_callCommand = new XPCommand<string>(async (number) => await CallNumber(number), CanExecute));

        private XPCommand<int> _openProfileCommand;
        public XPCommand<int> OpenProfileCommand => _openProfileCommand ?? (_openProfileCommand = new XPCommand<int>(async (userId) => await OpenProfile(userId), CanExecute));

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
                Contacts = ReflectionHelper.SeparateInLists(_usersInDivision, nameof(GetUsersInDivisionModel.DivisionId))
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

                if(ContactLists?.Contacts?.Count == 0 || ContactLists.Contacts == null)
                {
                    ContactLists = new ContactListsModel
                    {
                        Contacts = ReflectionHelper.SeparateInLists(result, nameof(GetUsersInDivisionModel.DivisionId))
                    };

                    shouldUpdateView = true;
                }

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

        private async Task CallNumber(string number)
        {
        }

        private async Task OpenChat(int userId)
        {
        }

        private async Task OpenProfile(int userId)
        {
            await NavigationService.NavigateAsync<MembersProfileViewModel, int>(userId);
        }

        private bool CanExecute(int value) => !IsBusy;
        private bool CanExecute(string value) => !IsBusy;
    }
}
