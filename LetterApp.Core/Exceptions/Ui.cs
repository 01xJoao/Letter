
using System;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using SharpRaven.Data;

namespace LetterApp.Core.Exceptions
{
    public static class Ui
    {
        private static IDialogService _dialogService;
        private static IDialogService DialogService => _dialogService ?? (_dialogService = App.Container.GetInstance<IDialogService>());

        private static IRavenService _ravenService;
        private static IRavenService RavenService => _ravenService ?? (_ravenService = App.Container.GetInstance<IRavenService>());

        public static void Handle(TaskCanceledException e)
        {
            RavenService.Raven.Capture(new SentryEvent(e));

            DialogService.ShowAlert(nameof(e), e.ToString());
        }

        public static void Handle(WrongCredentialsException e)
        {
            DialogService.ShowAlert(nameof(e), e.ToString());
        }

        public static void Handle(SessionTimeoutException e)
        {
            DialogService.ShowAlert(nameof(e), e.ToString());
        }

        public static void Handle(NoInternetException e)
        {
            DialogService.ShowAlert(nameof(e), e.ToString());
        }

        public static void Handle(ServerErrorException e)
        {
            DialogService.ShowAlert(nameof(e), e.ToString());
        }

        public static void Handle(Exception e)
        {
            //if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            //{
            //    Dialogs.ShowAlert(UiMessages.NoInternetErrorMessage, UiMessages.NoInternetErrorTitle);
            //    return;
            //}

            #if DEBUG
            DialogService.ShowAlert(nameof(e), e.ToString());
            #endif
        }
    }
}
