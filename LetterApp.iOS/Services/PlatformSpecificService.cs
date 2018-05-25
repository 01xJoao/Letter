using System;
using System.Threading;
using Foundation;
using LetterApp.Core.Services.Interfaces;

namespace LetterApp.iOS.Services
{
    public class PlatformSpecificService : IPlatformSpecificService
    {
        public string PlatformLanguage()
        {
            var language = NSLocale.PreferredLanguages[0];
            return language.Split('-')[0];
        }

        public void ExitApp()
        {
            Thread.CurrentThread.Abort();
        }
    }
}
