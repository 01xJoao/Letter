// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.CustomViews.ProfileDivision
{
    partial class DivisionView
    {
        [Outlet]
        UIKit.UIButton _button { get; set; }


        [Outlet]
        UIKit.UIImageView _imageView { get; set; }


        [Outlet]
        UIKit.UILabel _label { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_button != null) {
                _button.Dispose ();
                _button = null;
            }

            if (_imageView != null) {
                _imageView.Dispose ();
                _imageView = null;
            }

            if (_label != null) {
                _label.Dispose ();
                _label = null;
            }
        }
    }
}