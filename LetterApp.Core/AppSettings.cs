using System;
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

        public static string UserEmail
        {
            get => Preferences.Get(nameof(UserEmail), string.Empty);
            set => Preferences.Set(nameof(UserEmail), value);
        }

        public static bool UserIsPeddingApproval
        {
            get => Preferences.Get(nameof(UserIsPeddingApproval), false);
            set => Preferences.Set(nameof(UserIsPeddingApproval), value);
        }

        public static void Logout()
        {
            Preferences.Remove(nameof(IsUserLogged));
            Preferences.Remove(nameof(AuthToken));
            Preferences.Remove(nameof(AuthTokenExpirationDate));
            Preferences.Remove(nameof(PubNubToken));
            Preferences.Remove(nameof(PubNubTokenExpirationDate));
            Preferences.Remove(nameof(UserIsPeddingApproval));
        }
    }
}
