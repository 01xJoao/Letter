// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.ChangePassword
{
    [Register ("ChangePasswordViewController")]
    partial class ChangePasswordViewController
    {
        [Outlet]
        UIKit.UIView _backgroundButton { get; set; }


        [Outlet]
        UIKit.UITextField _confirmPassword { get; set; }


        [Outlet]
        UIKit.UITextField _currentPassword { get; set; }


        [Outlet]
        UIKit.UITextField _newPassword { get; set; }


        [Outlet]
        UIKit.UIButton _submitButton { get; set; }


        [Outlet]
        UIKit.UIView _view1 { get; set; }


        [Outlet]
        UIKit.UIView _view2 { get; set; }


        [Outlet]
        UIKit.UIView _view3 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_backgroundButton != null) {
                _backgroundButton.Dispose ();
                _backgroundButton = null;
            }

            if (_confirmPassword != null) {
                _confirmPassword.Dispose ();
                _confirmPassword = null;
            }

            if (_currentPassword != null) {
                _currentPassword.Dispose ();
                _currentPassword = null;
            }

            if (_newPassword != null) {
                _newPassword.Dispose ();
                _newPassword = null;
            }

            if (_submitButton != null) {
                _submitButton.Dispose ();
                _submitButton = null;
            }

            if (_view1 != null) {
                _view1.Dispose ();
                _view1 = null;
            }

            if (_view2 != null) {
                _view2.Dispose ();
                _view2 = null;
            }

            if (_view3 != null) {
                _view3.Dispose ();
                _view3 = null;
            }
        }
    }
}