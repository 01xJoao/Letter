// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.OnBoarding
{
	[Register ("OnBoardingViewController")]
	partial class OnBoardingViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint _buttonHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIView _pageContainer { get; set; }

		[Outlet]
		UIKit.UIPageControl _pageControl { get; set; }

		[Outlet]
		UIKit.UIView _pageParent { get; set; }

		[Outlet]
		UIKit.UIButton _signInButton { get; set; }

		[Outlet]
		UIKit.UIButton _signUpButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_pageContainer != null) {
				_pageContainer.Dispose ();
				_pageContainer = null;
			}

			if (_pageControl != null) {
				_pageControl.Dispose ();
				_pageControl = null;
			}

			if (_pageParent != null) {
				_pageParent.Dispose ();
				_pageParent = null;
			}

			if (_signInButton != null) {
				_signInButton.Dispose ();
				_signInButton = null;
			}

			if (_signUpButton != null) {
				_signUpButton.Dispose ();
				_signUpButton = null;
			}

			if (_buttonHeightConstraint != null) {
				_buttonHeightConstraint.Dispose ();
				_buttonHeightConstraint = null;
			}
		}
	}
}
