// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.OnBoarding
{
    [Register ("BoardPageViewController")]
    partial class BoardPageViewController
    {
        [Outlet]
        UIKit.UIView _backgroundView { get; set; }


        [Outlet]
        UIKit.UILabel _descriptionLabel { get; set; }


        [Outlet]
        UIKit.UIImageView _imageView { get; set; }


        [Outlet]
        UIKit.UILabel _titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_backgroundView != null) {
                _backgroundView.Dispose ();
                _backgroundView = null;
            }

            if (_descriptionLabel != null) {
                _descriptionLabel.Dispose ();
                _descriptionLabel = null;
            }

            if (_imageView != null) {
                _imageView.Dispose ();
                _imageView = null;
            }

            if (_titleLabel != null) {
                _titleLabel.Dispose ();
                _titleLabel = null;
            }
        }
    }
}