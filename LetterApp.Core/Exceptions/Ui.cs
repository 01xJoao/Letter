
using System;
using System.Threading.Tasks;
using LetterApp.Core.Localization;
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
            DialogService.ShowAlert(e.ToString(), AlertType.Error);
        }

        public static void Handle(WrongCredentialsException e)
        {
            DialogService.ShowAlert(e.ToString(), AlertType.Error);
        }

        public static void Handle(SessionTimeoutException e)
        {
            DialogService.ShowAlert(e.ToString(), AlertType.Error);
        }

        public static void Handle(NoInternetException e)
        {
            DialogService.ShowAlert(L10N.Localize("Dialogs_InternetException"), AlertType.Error);
        }

        public static void Handle(ServerErrorException e)
        {
            RavenService.Raven.Capture(new SentryEvent(e));
            DialogService.ShowAlert(CodeNull, AlertType.Error);
        }

        public static void Handle(Exception e)
        {
            RavenService.Raven.Capture(new SentryEvent(e));
            //if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            //{
            //    Dialogs.ShowAlert(UiMessages.NoInternetErrorMessage, UiMessages.NoInternetErrorTitle);
            //    return;
            //}

            #if DEBUG
            DialogService.ShowAlert(e.ToString(), AlertType.Error);
            #endif
        }

        #region Resources
        static string CodeNull => L10N.Localize("Code_E105");
        #endregion
    }
}
