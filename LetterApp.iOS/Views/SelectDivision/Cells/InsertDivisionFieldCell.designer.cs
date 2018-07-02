// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
	[Register ("InsertDivisionFieldCell")]
	partial class InsertDivisionFieldCell
	{
		[Outlet]
		UIKit.UIView _buttonView { get; set; }

		[Outlet]
		UIKit.UIButton _submitButton { get; set; }

		[Outlet]
		UIKit.UITextField _textField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_textField != null) {
				_textField.Dispose ();
				_textField = null;
			}

			if (_buttonView != null) {
				_buttonView.Dispose ();
				_buttonView = null;
			}

			if (_submitButton != null) {
				_submitButton.Dispose ();
				_submitButton = null;
			}
		}
	}
}
