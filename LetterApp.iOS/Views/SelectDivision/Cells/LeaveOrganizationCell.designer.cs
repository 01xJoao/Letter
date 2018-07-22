// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    [Register ("LeaveOrganizationCell")]
    partial class LeaveOrganizationCell
    {
        [Outlet]
        UIKit.UIButton _leaveButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_leaveButton != null) {
                _leaveButton.Dispose ();
                _leaveButton = null;
            }
        }
    }
}