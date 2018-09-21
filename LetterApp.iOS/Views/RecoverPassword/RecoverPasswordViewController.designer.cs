// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.RecoverPassword
{
    [Register ("RecoverPasswordViewController")]
    partial class RecoverPasswordViewController
    {
        [Outlet]
        UIKit.UIView _backgroundView { get; set; }


        [Outlet]
        UIKit.UIView _buttonView { get; set; }


        [Outlet]
        UIKit.UIButton _closeButton { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _navBarTopConstraint { get; set; }


        [Outlet]
        UIKit.UIView _navigationBarView { get; set; }


        [Outlet]
        UIKit.UIButton _submitButton { get; set; }


        [Outlet]
        UIKit.UITableView _tableView { get; set; }


        [Outlet]
        UIKit.UILabel _titleLabel { get; set; }

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

            if (_closeButton != null) {
                _closeButton.Dispose ();
                _closeButton = null;
            }

            if (_navBarTopConstraint != null) {
                _navBarTopConstraint.Dispose ();
                _navBarTopConstraint = null;
            }

            if (_navigationBarView != null) {
                _navigationBarView.Dispose ();
                _navigationBarView = null;
            }

            if (_submitButton != null) {
                _submitButton.Dispose ();
                _submitButton = null;
            }

            if (_tableView != null) {
                _tableView.Dispose ();
                _tableView = null;
            }

            if (_titleLabel != null) {
                _titleLabel.Dispose ();
                _titleLabel = null;
            }
        }
    }
}