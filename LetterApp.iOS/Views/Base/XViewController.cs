using System;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Services;
using UIKit;

namespace LetterApp.iOS.Views.Base
{
    public abstract class XViewController<TViewModel> : UIViewController, IXiOSView where TViewModel : class, IXViewModel
    {
        public TViewModel ViewModel { get; private set; }
        public object ParameterData { get; set; }
        public virtual bool ShowAsPresentView => false;

        public XViewController(string nibName, NSBundle bundle) : base(nibName, bundle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel = App.Container.GetInstance<TViewModel>();

            if (ParameterData != null)
                ViewModel.Prepare(ParameterData);
            else
                ViewModel.Prepare();

            ViewModel.InitializeViewModel();

            this.EdgesForExtendedLayout = UIRectEdge.None;
        }

        public override void ViewWillAppear(bool animated)
        {
            ViewModel.Appearing();
            base.ViewWillAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            ViewModel.Disappearing();
            base.ViewWillDisappear(animated);
        }
    }
}
