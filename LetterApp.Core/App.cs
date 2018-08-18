using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels;
using Realms;
using SimpleInjector;

namespace LetterApp.Core
{
    public static class App
    {
        public static Container Container { get; private set; } = new Container();
        private static IXNavigationService NavigationService => Container.GetInstance<IXNavigationService>();

        public static void Start()
        {
            NavigationService.NavigateAsync<LoadingViewModel, object>(null);
        }

        public static void StartCall(int callerId)
        {
            NavigationService.NavigateAsync<CallViewModel, Tuple<int, bool>>(new Tuple<int, bool>(callerId, false));
        }

        public static void Initialize()
        {
            //InitializeDatabase();
            InitializeIoC();
        }

        public static void InitializeIoC()
        {
            RegisterViewModelsAuto();
            RegisterServicesAuto();
        }

        private static void RegisterViewModelsAuto()
        {
            var repositoryAssembly = typeof(App).GetTypeInfo().Assembly;

            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace == "LetterApp.Core.ViewModels"
                where !type.GetTypeInfo().IsAbstract
                select new { Implementation = type };

            registrations = registrations.ToList();

            foreach (var registration in registrations)
            {
                Container.Register(registration.Implementation, registration.Implementation, Lifestyle.Transient);
                Debug.WriteLine($"{registration.Implementation.FullName} Registered");
            }
        }

        private static void RegisterServicesAuto()
        {
            var repositoryAssembly = typeof(App).GetTypeInfo().Assembly;

            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace == "LetterApp.Core.Services"
                where type.GetTypeInfo().GetInterfaces().Any() && !type.GetTypeInfo().IsAbstract
                select new { Interface = type.GetTypeInfo().GetInterfaces().Single(), Implementation = type };

            foreach (var registration in registrations)
            {
                Container.Register(registration.Interface, registration.Implementation, Lifestyle.Singleton);
                Debug.WriteLine($"{registration.Interface.FullName} Registered");
            }
        }

        private static void InitializeDatabase()
        {
            //Update SchemaVersion if necessary
            var config = new RealmConfiguration() { SchemaVersion = 1 };
            Realm.GetInstance(config);
        }
    }
}
