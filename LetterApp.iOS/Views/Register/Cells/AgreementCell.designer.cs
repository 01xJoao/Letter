// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.Register.Cells
{
	[Register ("AgreementCell")]
	partial class AgreementCell
	{
		[Outlet]
		UIKit.UIButton _button { get; set; }

		[Outlet]
		UIKit.UILabel _label { get; set; }

		[Outlet]
		UIKit.UIButton _termsButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_button != null) {
				_button.Dispose ();
				_button = null;
			}

			if (_label != null) {
				_label.Dispose ();
				_label = null;
			}

			if (_termsButton != null) {
				_termsButton.Dispose ();
				_termsButton = null;
			}
		}
	}
}
