using System;
using System.Linq;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using SimpleInjector;
using UIKit;
using Xamarin.Essentials;

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

            AppSettings.UserNoInternetNotified = false;
            StatusBarColor(Connectivity.NetworkAccess == NetworkAccess.Internet);

            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private static void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            StatusBarColor(e.NetworkAccess == NetworkAccess.Internet);
        }

        private static void StatusBarColor(bool hasInternet)
        {
            if (UIApplication.SharedApplication?.ValueForKey((NSString)"statusBarWindow")?.ValueForKey((NSString)"statusBar") is UIView statusBar)
            {
                if (!hasInternet)
                    statusBar.BackgroundColor = Colors.Red;
                else
                {
                    statusBar.BackgroundColor = UIColor.Clear;
                    AppSettings.UserNoInternetNotified = false;
                }
            }
        }

        private static void ConfigureView()
        {
            UINavigationBar.Appearance.BarTintColor = Colors.BlueSetup;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() { TextColor = Colors.White });

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
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
