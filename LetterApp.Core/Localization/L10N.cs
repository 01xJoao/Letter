using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LetterApp.Core.Services.Interfaces;
using Newtonsoft.Json;

namespace LetterApp.Core.Localization
{
    public static class L10N
    {
        private static string _currentLocale;
        private static string CurrentLocale
        {
            get => _currentLocale ?? (_currentLocale = App.Container.GetInstance<IPlatformSpecificService>().PlatformLanguage());
        }

        private static List<string> SupportedLanguages => new List<string> { "en-US", "pt-PT" };
        private static string DefaultLanguage => "en-US";

        private static List<Literal> _resourceManager;
        private static List<Literal> ResourceManager(string locale)
        {
            if (!SupportedLanguages.Contains(locale))
                locale = DefaultLanguage;

            if (_resourceManager != null)
                return _resourceManager;

            var manifestResourceStream = Assembly.Load(new AssemblyName("LetterApp.Core")).GetManifestResourceStream(string.Format($"LetterApp.Core.Localization.Resources-{locale}.json"));
            var streamReader = new StreamReader(manifestResourceStream);
            var jsonString = streamReader.ReadToEnd();
            var tracksCollection = JsonConvert.DeserializeObject<List<Literal>>(jsonString);
            _resourceManager = tracksCollection;

            return _resourceManager;
        }

        public static string Localize(string key)
        {
            string locale;
            switch (CurrentLocale)
            {
                case "pt": locale = "pt-PT"; break;
                case "en": locale = "en-US"; break;

                default:
                    locale = "en-US";
                    break;
            }

            var val = ResourceManager(locale)?.FirstOrDefault(x => x.Key == key);
            return val != null ? val.Translated : string.Empty;
        }

        public static string Locale()
        {
            return CurrentLocale;
        }

        private class Literal
        {
            public string Key { get; set; }
            public string Translated { get; set; }
        }
    }
}
