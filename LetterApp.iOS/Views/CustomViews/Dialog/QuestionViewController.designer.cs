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
    [Register ("QuestionViewController")]
    partial class QuestionViewController
    {
        [Outlet]
        UIKit.UIView _backgroundView { get; set; }


        [Outlet]
        UIKit.UIButton _closeButton { get; set; }


        [Outlet]
        UIKit.UILabel _label { get; set; }


        [Outlet]
        UIKit.UIButton _submitButton { get; set; }


        [Outlet]
        UIKit.UIView _viewButton { get; set; }


        [Outlet]
        UIKit.UIView _viewButtonSmall { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_backgroundView != null) {
                _backgroundView.Dispose ();
                _backgroundView = null;
            }

            if (_closeButton != null) {
                _closeButton.Dispose ();
                _closeButton = null;
            }

            if (_label != null) {
                _label.Dispose ();
                _label = null;
            }

            if (_submitButton != null) {
                _submitButton.Dispose ();
                _submitButton = null;
            }

            if (_viewButton != null) {
                _viewButton.Dispose ();
                _viewButton = null;
            }

            if (_viewButtonSmall != null) {
                _viewButtonSmall.Dispose ();
                _viewButtonSmall = null;
            }
        }
    }
}