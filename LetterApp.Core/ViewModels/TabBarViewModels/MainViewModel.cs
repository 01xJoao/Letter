using System;
using System.Threading.Tasks;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using Xamarin.Essentials;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class MainViewModel : XViewModel
    {
        private readonly IMessengerService _messengerService;

        private XPCommand<bool> _messengerServiceCommand;
        public XPCommand<bool> MessengerServiceCommand => _messengerServiceCommand ?? (_messengerServiceCommand = new XPCommand<bool>(async (connect) => await MessengerService(connect), CanExecute));

        public MainViewModel(IMessengerService messengerService)
        {
            _messengerService = messengerService;

           // CheckConnection();
        }

        private async Task MessengerService(bool connect)
        {
            //if (connect)
            //{
            //    await Task.Delay(TimeSpan.FromSeconds(0.5f));
            //    await _messengerService.ConnectMessenger();
            //}
            //else
                //_messengerService.DisconnectMessenger();
        }

        //private void CheckConnection()
        //{
        //    Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        //    Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        //}

        //private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        //{
        //    if (e.NetworkAccess == NetworkAccess.Internet)
        //        MessengerService(true);
        //}

        private bool CanExecute(bool connect) => !IsBusy;

        #region resources

        public string ChatTab => L10N.Localize("MainViewModel_ChatTab");
        public string CallTab => L10N.Localize("MainViewModel_CallTab");
        public string ContactTab => L10N.Localize("MainViewModel_ContactTab");
        public string ProfileTab => L10N.Localize("MainViewModel_ProfileTab");

        #endregion
    }
}
