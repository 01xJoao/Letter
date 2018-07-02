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
	[Register ("SelectButtonCell")]
	partial class SelectButtonCell
	{
		[Outlet]
		UIKit.UIView _backgroundView { get; set; }

		[Outlet]
		UIKit.UIButton _verifyButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_backgroundView != null) {
				_backgroundView.Dispose ();
				_backgroundView = null;
			}

			if (_verifyButton != null) {
				_verifyButton.Dispose ();
				_verifyButton = null;
			}
		}
	}
}
