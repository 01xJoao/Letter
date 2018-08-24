using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.Base
{
    public class RootViewController : UIViewController
    {
        public UIViewController CurrentViewController { get; private set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationController.SetNavigationBarHidden(true, false);
        }

        public void AddViewToRoot(UIViewController viewController, bool isAnimated = true)
        {
            if (CurrentViewController?.GetType()?.Name == viewController.GetType().Name)
                return;
            
            if (CurrentViewController != null)
            {
                CurrentViewController.RemoveFromParentViewController();
                CurrentViewController.View.RemoveFromSuperview();
                MemoryUtility.ReleaseUIViewWithChildren(CurrentViewController.View);
                CurrentViewController = null;
            }

            viewController.View.Frame = this.View.Frame;
            this.View.AddSubview(viewController.View);

            this.AddChildViewController(viewController);
            this.WillMoveToParentViewController(this);

            if (isAnimated && viewController.View.Subviews.Length > 0)
                Animations.Fade(viewController.View.Subviews[0], true);

            viewController.DidMoveToParentViewController(this);
            CurrentViewController = viewController;
        }
    }
}