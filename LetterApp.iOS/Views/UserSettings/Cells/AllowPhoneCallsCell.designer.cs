// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
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
            if (_description != null) {
                _description.Dispose ();
                _description = null;
            }

            if (_switch != null) {
                _switch.Dispose ();
                _switch = null;
            }

            if (_title != null) {
                _title.Dispose ();
                _title = null;
            }
        }
    }
}