using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.OnBoarding
{
    public partial class OnBoardingViewController : XViewController<OnBoardingViewModel>, IRootView
    {
        private UIPageViewController _pageViewController;
        private OnBoardPageDataSource _onBoardPageSource;

        public OnBoardingViewController() : base("OnBoardingViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView();

            _signInButton.TouchUpInside -= OnSignInButton_TouchUpInside;
            _signInButton.TouchUpInside += OnSignInButton_TouchUpInside;

            _pageViewController = new UIPageViewController(UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation.Horizontal);

            var viewControllers = new List<XBoardPageViewController>
            {
                new BoardPageViewController(0, ViewModel.WelcomeTitle, ViewModel.WelcomeSubtitle, "letter_logo"),
                new BoardPageViewController(1, ViewModel.RegisterTitle, ViewModel.RegisterSubtitle, "board_mail"),
                new BoardPageViewController(2, ViewModel.CallTitle, ViewModel.CallSubtitle, "board_chat")
            };

            _onBoardPageSource = new OnBoardPageDataSource(viewControllers);
            _pageViewController.DataSource = _onBoardPageSource;
            _pageViewController.SetViewControllers(new UIViewController[] { viewControllers.FirstOrDefault() }, UIPageViewControllerNavigationDirection.Forward, false, null);

            this.AddChildViewController(_pageViewController);
            _pageContainer.AddSubview(_pageViewController.View);

            _pageViewController.DidFinishAnimating -= DidTransition;
            _pageViewController.DidFinishAnimating += DidTransition;

            _pageControl.CurrentPage = 0;
        }

        private void DidTransition(object sender, UIPageViewFinishedAnimationEventArgs e)
        {
            if(e.Finished)
            {
                var viewController = _pageViewController.ViewControllers[0] as XBoardPageViewController;
                _pageControl.CurrentPage = viewController.Index;
            }
        }

        public override void ViewDidLayoutSubviews()
		{
            base.ViewDidLayoutSubviews();
            _pageViewController.View.Frame = _pageParent.Bounds;
		}

        private void SetupView()
        {
            var underlineAttr = new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.Single, ForegroundColor = UIColor.White, Font = UIFont.SystemFontOfSize(16) };
            _signInButton.SetAttributedTitle(new NSAttributedString(ViewModel.SignIn, underlineAttr), UIControlState.Normal);

            UIButtonExtensions.SetupButtonAppearance(_signUpButton, Colors.MainBlue, 16f, ViewModel.SignUp);

            _pageParent.BackgroundColor = Colors.MainBlue;
            _pageContainer.BackgroundColor = Colors.MainBlue;
        }

        private void OnSignInButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.OpenLoginViewCommand.Execute();
        }

		public override void ViewWillAppear(bool animated)
		{
            base.ViewWillAppear(animated);
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
		}
	}
}

