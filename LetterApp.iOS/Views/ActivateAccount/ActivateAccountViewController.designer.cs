// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.ActivateAccount
{
	[Register ("ActivateAccountViewController")]
	partial class ActivateAccountViewController
	{
		[Outlet]
		UIKit.UILabel _activateLabel { get; set; }

		[Outlet]
		UIKit.UIView _backgroundView { get; set; }

		[Outlet]
		UIKit.UIButton _button { get; set; }

		[Outlet]
		UIKit.UIView _buttonView { get; set; }

		[Outlet]
		UIKit.UIButton _closeButon { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _navigationTopConstraint { get; set; }

		[Outlet]
		UIKit.UIButton _requestCodeButton { get; set; }

		[Outlet]
		UIKit.UITextField _textField { get; set; }

		[Outlet]
		UIKit.UILabel _titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_activateLabel != null) {
				_activateLabel.Dispose ();
				_activateLabel = null;
			}

			if (_backgroundView != null) {
				_backgroundView.Dispose ();
				_backgroundView = null;
			}

			if (_button != null) {
				_button.Dispose ();
				_button = null;
			}

			if (_buttonView != null) {
				_buttonView.Dispose ();
				_buttonView = null;
			}

			if (_closeButon != null) {
				_closeButon.Dispose ();
				_closeButon = null;
			}

			if (_requestCodeButton != null) {
				_requestCodeButton.Dispose ();
				_requestCodeButton = null;
			}

			if (_textField != null) {
				_textField.Dispose ();
				_textField = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}

			if (_navigationTopConstraint != null) {
				_navigationTopConstraint.Dispose ();
				_navigationTopConstraint = null;
			}
		}
	}
}
