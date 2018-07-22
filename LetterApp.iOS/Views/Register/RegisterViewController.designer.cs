// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.Register
{
    [Register ("RegisterViewController")]
    partial class RegisterViewController
    {
        [Outlet]
        UIKit.UIView _backgroundView { get; set; }


        [Outlet]
        UIKit.UIView _buttonView { get; set; }


        [Outlet]
        UIKit.UIButton _submitButton { get; set; }


        [Outlet]
        UIKit.UITableView _tableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_backgroundView != null) {
                _backgroundView.Dispose ();
                _backgroundView = null;
            }

            if (_buttonView != null) {
                _buttonView.Dispose ();
                _buttonView = null;
            }

            if (_submitButton != null) {
                _submitButton.Dispose ();
                _submitButton = null;
            }

            if (_tableView != null) {
                _tableView.Dispose ();
                _tableView = null;
            }
        }
    }
}