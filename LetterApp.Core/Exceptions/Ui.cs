using System;
using System.Threading.Tasks;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using SharpRaven.Data;
using Xamarin.Essentials;

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

            if (Connectivity.NetworkAccess == NetworkAccess.Internet){}
                //DialogService.ShowAlert(e.ToString(), AlertType.Error);
            else
                Handle(new NoInternetException());
        }

        public static void Handle(WrongCredentialsException e)
        {
            DialogService.ShowAlert(e.ToString(), AlertType.Error);
        }

        public static void Handle(SessionTimeoutException e)
        {
            DialogService.ShowAlert(e.ToString(), AlertType.Error);
        }

        public static void Handle(FeatureNotSupportedException e)
        {
            DialogService.ShowAlert(CodeNull, AlertType.Error);
        }

        public static void Handle(NoInternetException e)
        {
            if(!AppSettings.UserNoInternetNotified)
            {
                DialogService.ShowAlert(L10N.Localize("Dialogs_InternetException"), AlertType.Error);
                AppSettings.UserNoInternetNotified = true;
            }
        }

        public static void Handle(ServerErrorException e)
        {
            RavenService.Raven.Capture(new SentryEvent(e));
            DialogService.ShowAlert(CodeNull, AlertType.Error);
        }

        public static void Handle(Exception e)
        {
            RavenService.Raven.Capture(new SentryEvent(e));

            #if DEBUG
            DialogService.ShowAlert(e.ToString(), AlertType.Error);
            #endif
        }

        #region Resources
        static string CodeNull => L10N.Localize("Code_E105");
        #endregion
    }
}
