// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    [Register ("HeaderCell")]
    partial class HeaderCell
    {
        [Outlet]
        UIKit.UIImageView _imageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_imageView != null) {
                _imageView.Dispose ();
                _imageView = null;
            }
        }
    }
}