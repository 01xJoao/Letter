using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Helpers;
using LetterApp.Core.Services;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Services
{
    public class XiOSNavigationService : XNavigationService
    {
        public RootViewController RootViewController;
        private UINavigationController MasterNavigationController;

        public override Task NavigatePlatformAsync<TViewModel, TObject>(TObject data)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var viewController = CreateViewControllerForViewModel<TViewModel, TObject>(data);
                ShowView(viewController);
            });

            Debug.WriteLine($"Task Completed: NavigatePlatformAsync");

            return Task.CompletedTask;
        }

        private UIViewController CreateViewControllerForViewModel<TViewModel, TObject>(TObject data) where TViewModel : class, IXViewModel
        {
            var vmType = typeof(TViewModel);
            var view = GetViewForViewModel(vmType);
            var viewController = Activator.CreateInstance(view) as UIViewController;

            Debug.WriteLine($"Final ViewModel: {vmType} for viewcontroller: {viewController}");

            var xVC = viewController as IXiOSView;

            if (xVC != null && !data.IsNull())
                xVC.ParameterData = data;

            return viewController;
        }

        private void ShowView(UIViewController viewController)
        {
            if (viewController is IRootView)
            {
                Debug.WriteLine($"ViewController is a Root Controller");
                SetRootViewController(viewController);
            }
            else
            {
                var presentViewProperty = viewController?.GetType()?.GetProperty("ShowAsPresentView");
                bool showAsModal = (bool)presentViewProperty?.GetValue(viewController);

                if(MasterNavigationController == null)
                {
                    Debug.WriteLine($"MasterNavigationController is null");
                    SetRootViewController(new MainViewController());
                }
                if (showAsModal)
                    MasterNavigationController.PresentViewController(viewController, true, null);
                else
                    MasterNavigationController.PushViewController(viewController, true);

                Debug.WriteLine($"{viewController} was Pushed/Presented");
            }
        }

        public override Task Close<TViewModel>(TViewModel viewModel)
        {
            var viewController = MasterNavigationController.VisibleViewController;
            var presentViewProperty = viewController?.GetType()?.GetProperty("ShowAsPresentView");

            if ((bool)presentViewProperty?.GetValue(viewController))
                MasterNavigationController.DismissViewController(true, null);
            else
                MasterNavigationController.PopViewController(true);

            return Task.CompletedTask;
        }

        private void SetRootViewController(UIViewController viewController)
        {
            if(RootViewController == null)
            {
                Debug.WriteLine($"RootViewController is null");

                using(var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate)
                {
                    RootViewController = appDelegate.RootController;
                    MasterNavigationController = RootViewController.NavigationController;
                }
            }
            Debug.WriteLine($"{viewController} added to RootViewController");
            RootViewController.AddViewToRoot(viewController);
        }

        protected override IEnumerable<Type> GetPlatformMainViewTypes()
        {
            var repositoryAssembly = typeof(XiOSNavigationService).Assembly;

            var controllers =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace.Contains("LetterApp.iOS.Views")
                where !type.IsAbstract && (type.IsSubclassOfRawGeneric(typeof(XViewController<>)) || type.IsSubclassOfRawGeneric(typeof(XTabBarViewController<>)))
                select type;

            return controllers;
        }

        public override Task PopToRoot(bool animated)
        {
            MasterNavigationController.PopToRootViewController(animated);
            return Task.CompletedTask;
        }
    }
}
