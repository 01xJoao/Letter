// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
	[Register ("InputTextViewController")]
	partial class InputTextViewController
	{
		[Outlet]
		UIKit.UIView _backgroundView { get; set; }

		[Outlet]
		UIKit.UIView _buttonView { get; set; }

		[Outlet]
		UIKit.UIButton _closeButton { get; set; }

		[Outlet]
		UIKit.UIButton _confirmButton { get; set; }

		[Outlet]
		UIKit.UILabel _indicatorLabel { get; set; }

		[Outlet]
		UIKit.UIView _indicatorView { get; set; }

		[Outlet]
		UIKit.UITextField _textField { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _textFieldHeightConstraint { get; set; }

		[Outlet]
		UIKit.UILabel _titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_backgroundView != null) {
				_backgroundView.Dispose ();
				_backgroundView = null;
			}

			if (_closeButton != null) {
				_closeButton.Dispose ();
				_closeButton = null;
			}

			if (_confirmButton != null) {
				_confirmButton.Dispose ();
				_confirmButton = null;
			}

			if (_indicatorLabel != null) {
				_indicatorLabel.Dispose ();
				_indicatorLabel = null;
			}

			if (_indicatorView != null) {
				_indicatorView.Dispose ();
				_indicatorView = null;
			}

			if (_textField != null) {
				_textField.Dispose ();
				_textField = null;
			}

			if (_textFieldHeightConstraint != null) {
				_textFieldHeightConstraint.Dispose ();
				_textFieldHeightConstraint = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}

			if (_buttonView != null) {
				_buttonView.Dispose ();
				_buttonView = null;
			}
		}
	}
}
