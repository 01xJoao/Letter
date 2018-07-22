// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.UserSettings.Cells
{
	[Register ("AllowPhoneCallsCell")]
	partial class AllowPhoneCallsCell
	{
		[Outlet]
		UIKit.UILabel _description { get; set; }

		[Outlet]
		UIKit.UISwitch _switch { get; set; }

		[Outlet]
		UIKit.UILabel _title { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_title != null) {
				_title.Dispose ();
				_title = null;
			}

			if (_description != null) {
				_description.Dispose ();
				_description = null;
			}

			if (_switch != null) {
				_switch.Dispose ();
				_switch = null;
			}
		}
	}
}
