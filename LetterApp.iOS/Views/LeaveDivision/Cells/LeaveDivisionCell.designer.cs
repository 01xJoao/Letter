// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.LeaveDivision.Cells
{
	[Register ("LeaveDivisionCell")]
	partial class LeaveDivisionCell
	{
		[Outlet]
		UIKit.UIButton _button { get; set; }

		[Outlet]
		UIKit.UILabel _label { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_label != null) {
				_label.Dispose ();
				_label = null;
			}

			if (_button != null) {
				_button.Dispose ();
				_button = null;
			}
		}
	}
}
