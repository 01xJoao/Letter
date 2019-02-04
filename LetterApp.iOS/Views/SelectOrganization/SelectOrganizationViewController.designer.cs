// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.SelectOrganization
{
	[Register ("SelectOrganizationViewController")]
	partial class SelectOrganizationViewController
	{
		[Outlet]
		UIKit.UIView _backgroundView { get; set; }

		[Outlet]
		UIKit.UIView _buttonBackgroundView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _buttonHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIButton _closeButton { get; set; }

		[Outlet]
		UIKit.UIButton _createOrgButton { get; set; }

		[Outlet]
		UIKit.UILabel _label { get; set; }

		[Outlet]
		UIKit.UIButton _submitButton { get; set; }

		[Outlet]
		UIKit.UITextField _textField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_backgroundView != null) {
				_backgroundView.Dispose ();
				_backgroundView = null;
			}

			if (_buttonBackgroundView != null) {
				_buttonBackgroundView.Dispose ();
				_buttonBackgroundView = null;
			}

			if (_closeButton != null) {
				_closeButton.Dispose ();
				_closeButton = null;
			}

			if (_createOrgButton != null) {
				_createOrgButton.Dispose ();
				_createOrgButton = null;
			}

			if (_label != null) {
				_label.Dispose ();
				_label = null;
			}

			if (_submitButton != null) {
				_submitButton.Dispose ();
				_submitButton = null;
			}

			if (_textField != null) {
				_textField.Dispose ();
				_textField = null;
			}

			if (_buttonHeightConstraint != null) {
				_buttonHeightConstraint.Dispose ();
				_buttonHeightConstraint = null;
			}
		}
	}
}
