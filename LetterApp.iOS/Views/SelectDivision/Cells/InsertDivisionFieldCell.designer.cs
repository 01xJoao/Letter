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
            if (_buttonView != null) {
                _buttonView.Dispose ();
                _buttonView = null;
            }

            if (_submitButton != null) {
                _submitButton.Dispose ();
                _submitButton = null;
            }

            if (_textField != null) {
                _textField.Dispose ();
                _textField = null;
            }
        }
    }
}