// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.Register
{
	[Register ("RegisterViewController")]
	partial class RegisterViewController
	{
		[Outlet]
		UIKit.UIView _backgroundView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _buttonHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIView _buttonView { get; set; }

		[Outlet]
		UIKit.UIButton _submitButton { get; set; }

		[Outlet]
		UIKit.UITableView _tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_backgroundView != null) {
				_backgroundView.Dispose ();
				_backgroundView = null;
			}

			if (_buttonView != null) {
				_buttonView.Dispose ();
				_buttonView = null;
			}

			if (_submitButton != null) {
				_submitButton.Dispose ();
				_submitButton = null;
			}

			if (_tableView != null) {
				_tableView.Dispose ();
				_tableView = null;
			}

			if (_buttonHeightConstraint != null) {
				_buttonHeightConstraint.Dispose ();
				_buttonHeightConstraint = null;
			}
		}
	}
}
