using System;
using Realms;
using Xamarin.Essentials;
namespace LetterApp.Core
{
    public static class AppSettings
    {
        public static bool IsUserLogged
        {
            get => Preferences.Get(nameof(IsUserLogged), false);
            set => Preferences.Set(nameof(IsUserLogged), value);
        }

        public static string AuthToken 
        {
            get => Preferences.Get(nameof(AuthToken), string.Empty);
            set => Preferences.Set(nameof(AuthToken), value);
        }

        public static long AuthTokenExpirationDate
        {
            get => Preferences.Get(nameof(AuthTokenExpirationDate), default(DateTime).Ticks);
            set => Preferences.Set(nameof(AuthTokenExpirationDate), value);
        }

        public static string PubNubToken
        {
            get => Preferences.Get(nameof(PubNubToken), string.Empty);
            set => Preferences.Set(nameof(PubNubToken), value);
        }

        public static long PubNubTokenExpirationDate
        {
            get => Preferences.Get(nameof(PubNubTokenExpirationDate), default(DateTime).Ticks);
            set => Preferences.Set(nameof(PubNubTokenExpirationDate), value);
        }

        public static int UserId
        {
            get => Preferences.Get(nameof(UserId), 0);
            set => Preferences.Set(nameof(UserId), value);
        }

        public static string UserEmail
        {
            get => Preferences.Get(nameof(UserEmail), string.Empty);
            set => Preferences.Set(nameof(UserEmail), value);
        }

        public static bool MainMenuAllowed
        {
            get => Preferences.Get(nameof(MainMenuAllowed), false);
            set => Preferences.Set(nameof(MainMenuAllowed), value);
        }

        public static bool MessageNotifications
        {
            get => Preferences.Get(nameof(MessageNotifications), false);
            set => Preferences.Set(nameof(MessageNotifications), value);
        }

        public static bool CallNotifications
        {
            get => Preferences.Get(nameof(CallNotifications), false);
            set => Preferences.Set(nameof(CallNotifications), value);
        }

        public static bool GroupNotifications
        {
            get => Preferences.Get(nameof(GroupNotifications), false);
            set => Preferences.Set(nameof(GroupNotifications), value);
        }

        public static bool UserNoInternetNotified
        {
            get => Preferences.Get(nameof(UserNoInternetNotified), false);
            set => Preferences.Set(nameof(UserNoInternetNotified), value);
        }

        public static bool FilteredMembers
        {
            get => Preferences.Get(nameof(FilteredMembers), true);
            set => Preferences.Set(nameof(FilteredMembers), value);
        }

        public static void Logout()
        {
            Preferences.Remove(nameof(IsUserLogged));
            Preferences.Remove(nameof(AuthToken));
            Preferences.Remove(nameof(AuthTokenExpirationDate));
            Preferences.Remove(nameof(PubNubToken));
            Preferences.Remove(nameof(PubNubTokenExpirationDate));
            Preferences.Remove(nameof(MainMenuAllowed));
            Preferences.Remove(nameof(UserId));
            Preferences.Remove(nameof(MessageNotifications));
            Preferences.Remove(nameof(CallNotifications));
            Preferences.Remove(nameof(GroupNotifications));
            Preferences.Remove(nameof(UserNoInternetNotified));
            Preferences.Remove(nameof(FilteredMembers));
            //TODO Delete DB
        }
    }
}
