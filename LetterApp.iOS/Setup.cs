using System;
using System.Linq;
using LetterApp.Core;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using SimpleInjector;
using UIKit;

namespace LetterApp.iOS
{
    public static class Setup
    {
        public static void Initialize()
        {
            App.Initialize();
            ConfigureView();
            RegisterPlatformServices();
            InitializePlatformServices();
            App.Start();
        }

        private static void ConfigureView()
        {
            UINavigationBar.Appearance.BarTintColor = Colors.MainBlue;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() { TextColor = Colors.White });
            UINavigationBar.Appearance.LargeTitleTextAttributes = new UIStringAttributes { ForegroundColor = Colors.White, Shadow = CustomUIExtensions.TextShadow() };
        }

        private static void RegisterPlatformServices()
        {
            var repositoryAssembly = typeof(Setup).Assembly;

            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace == "LetterApp.iOS.Services"
                where type.GetInterfaces().Any() && !type.IsAbstract
                select new { Interface = type.GetInterfaces().Single(), Implementation = type };

            foreach (var registration in registrations)
            {
                App.Container.Register(registration.Interface, registration.Implementation, Lifestyle.Singleton);
            }
        }

        private static void InitializePlatformServices()
        {
            App.Container.GetInstance<IXNavigationService>().Initialize();
        }
    }
}
