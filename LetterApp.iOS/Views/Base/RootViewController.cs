using System;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.Base
{
    public class RootViewController : UIViewController
    {
        private UIViewController _currentViewController;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationController.SetNavigationBarHidden(true, false);
        }

        public void AddViewToRoot(UIViewController viewController, bool isAnimated = true)
        {
            if (_currentViewController?.GetType()?.Name == viewController.GetType().Name)
                return;
            
            if (_currentViewController != null)
            {
                _currentViewController.RemoveFromParentViewController();
                _currentViewController.View.RemoveFromSuperview();
                MemoryUtility.ReleaseUIViewWithChildren(_currentViewController.View);
                _currentViewController = null;
            }

            viewController.View.Frame = this.View.Frame;
            this.View.AddSubview(viewController.View);

            this.AddChildViewController(viewController);
            this.WillMoveToParentViewController(this);

            if (isAnimated && viewController.View.Subviews.Length > 0)
                Animations.Fade(viewController.View.Subviews[0], true);

            viewController.DidMoveToParentViewController(this);
            _currentViewController = viewController;
        }
    }
}