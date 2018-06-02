// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
	[Register ("TextFieldCell")]
	partial class TextFieldCell
	{
		[Outlet]
		UIKit.UIButton _button { get; set; }

		[Outlet]
		UIKit.UILabel _indicatorLabel { get; set; }

		[Outlet]
		UIKit.UIView _indicatorView { get; set; }

		[Outlet]
		UIKit.UITextField _textField { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _textFieldHeightConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _textfieldWidthConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_indicatorLabel != null) {
				_indicatorLabel.Dispose ();
				_indicatorLabel = null;
			}

			if (_textField != null) {
				_textField.Dispose ();
				_textField = null;
			}

			if (_indicatorView != null) {
				_indicatorView.Dispose ();
				_indicatorView = null;
			}

			if (_button != null) {
				_button.Dispose ();
				_button = null;
			}

			if (_textfieldWidthConstraint != null) {
				_textfieldWidthConstraint.Dispose ();
				_textfieldWidthConstraint = null;
			}

			if (_textFieldHeightConstraint != null) {
				_textFieldHeightConstraint.Dispose ();
				_textFieldHeightConstraint = null;
			}
		}
	}
}
