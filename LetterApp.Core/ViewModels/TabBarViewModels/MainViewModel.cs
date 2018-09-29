using System;
using System.Threading.Tasks;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class MainViewModel : XViewModel
    {
        private readonly IMessengerService _messengerService;

        private XPCommand<string> _callCommand;
        public XPCommand<string> CallCommand => _callCommand ?? (_callCommand = new XPCommand<string>(async (userId) => await Call(userId)));

        private XPCommand<bool> _messengerServiceCommand;
        public XPCommand<bool> MessengerServiceCommand => _messengerServiceCommand ?? (_messengerServiceCommand = new XPCommand<bool>(MessengerService));

        public MainViewModel(IMessengerService messengerService)
        {
            _messengerService = messengerService;
        }

        private void MessengerService(bool connect)
        {
            if (connect)
                _messengerService.ConnectMessenger();
            else
                _messengerService.DisconnectMessenger();
        }

        private async Task Call(string userId)
        {
            await NavigationService.NavigateAsync<CallViewModel, Tuple<int, bool>>(new Tuple<int, bool>(Int32.Parse(userId), false));
        }

        #region resources

        public string ChatTab => L10N.Localize("MainViewModel_ChatTab");
        public string CallTab => L10N.Localize("MainViewModel_CallTab");
        public string ContactTab => L10N.Localize("MainViewModel_ContactTab");
        public string ProfileTab => L10N.Localize("MainViewModel_ProfileTab");

        #endregion
    }
}
