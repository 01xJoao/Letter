// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    [Register ("InputTextViewController")]
    partial class InputTextViewController
    {
        [Outlet]
        UIKit.UIView _backgroundView { get; set; }


        [Outlet]
        UIKit.UIView _buttonView { get; set; }


        [Outlet]
        UIKit.UIView _buttonView1 { get; set; }


        [Outlet]
        UIKit.UIView _buttonView2 { get; set; }


        [Outlet]
        UIKit.UIButton _closeButton { get; set; }


        [Outlet]
        UIKit.UIButton _confirmButton { get; set; }


        [Outlet]
        UIKit.UILabel _indicatorLabel { get; set; }


        [Outlet]
        UIKit.UIView _indicatorView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _subtitleHeightConstraint { get; set; }


        [Outlet]
        UIKit.UILabel _subtitleLabel { get; set; }


        [Outlet]
        UIKit.UITextField _textField { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _textFieldHeightConstraint { get; set; }


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

            if (_buttonView1 != null) {
                _buttonView1.Dispose ();
                _buttonView1 = null;
            }

            if (_buttonView2 != null) {
                _buttonView2.Dispose ();
                _buttonView2 = null;
            }

            if (_closeButton != null) {
                _closeButton.Dispose ();
                _closeButton = null;
            }

            if (_confirmButton != null) {
                _confirmButton.Dispose ();
                _confirmButton = null;
            }

            if (_indicatorLabel != null) {
                _indicatorLabel.Dispose ();
                _indicatorLabel = null;
            }

            if (_indicatorView != null) {
                _indicatorView.Dispose ();
                _indicatorView = null;
            }

            if (_subtitleHeightConstraint != null) {
                _subtitleHeightConstraint.Dispose ();
                _subtitleHeightConstraint = null;
            }

            if (_subtitleLabel != null) {
                _subtitleLabel.Dispose ();
                _subtitleLabel = null;
            }

            if (_textField != null) {
                _textField.Dispose ();
                _textField = null;
            }

            if (_textFieldHeightConstraint != null) {
                _textFieldHeightConstraint.Dispose ();
                _textFieldHeightConstraint = null;
            }

            if (_titleLabel != null) {
                _titleLabel.Dispose ();
                _titleLabel = null;
            }
        }
    }
}