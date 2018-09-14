using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class ChatListViewModel : XViewModel
    {
        private readonly IMessengerService _messagerService;

        public bool NoChats { get; set; }
        public string[] Actions;

        public List<ChatListUserCellModel> _chatList;
        public List<ChatListUserCellModel> ChatList
        {
            get => _chatList;
            set => SetProperty(ref _chatList, value);
        }

        private XPCommand _openContactsCommand;
        public XPCommand OpenContactsCommand => _openContactsCommand ?? (_openContactsCommand = new XPCommand(async () => await OpenContacts()));

        private XPCommand<string> _searchChatCommand;
        public XPCommand<string> SearchChatCommand => _searchChatCommand ?? (_searchChatCommand = new XPCommand<string>(SearchChat));

        private XPCommand _closeSearchCommand;
        public XPCommand CloseSearchCommand => _closeSearchCommand ?? (_closeSearchCommand = new XPCommand(CloseSearch));

        private XPCommand<Tuple<ChatEventType, int>> _rowActionCommand;
        public XPCommand<Tuple<ChatEventType, int>> RowActionCommand => _rowActionCommand ?? (_rowActionCommand = new XPCommand<Tuple<ChatEventType, int>>(RowAction));

        public ChatListViewModel(IMessengerService messagerService)
        {
            _messagerService = messagerService;

            Actions = new string[] { DeleteAction, MuteAction, UnMuteAction };

            //Tests
            //CreateChannel();
        }

        private async Task CreateChannel()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            try
            {
                var channel = await _messagerService.CreateChannel(new List<string> { "85-33" });

                if (channel != null)
                    await _messagerService.SendMessage(channel, "Hello World!");

            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }

        private async Task OpenContacts()
        {
        }

        private void RowAction(Tuple<ChatEventType, int> action)
        {
        }

        private void SearchChat(string search)
        {
        }

        private void CloseSearch()
        {
        }


        #region Resources

        public string Title => L10N.Localize("ChatList_Title");
        public string NoRecentChat => L10N.Localize("ChatList_NoChats");

        public string DeleteAction => L10N.Localize("ChatList_Delete");
        public string MuteAction => L10N.Localize("ChatList_Mute");
        public string UnMuteAction => L10N.Localize("ChatList_UnMute");

        public enum ChatEventType
        {
            Mute,
            Delete
        }

        #endregion
    }
}
