using System;
using LetterApp.Core;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.iOS.Interfaces;
using UIKit;

namespace LetterApp.iOS.Views.Base
{
    public abstract class XTabBarViewController<TViewModel> : UITabBarController, IXiOSView where TViewModel : class, IXViewModel
    {
        protected TViewModel ViewModel { get; private set; }
        public object ParameterData { get; set; }
        public bool ShowAsPresentView => false;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
                
            ViewModel = App.Container.GetInstance<TViewModel>();
            ViewModel.InitializeViewModel();
        }
    }
}
