// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.Chat.Cells
{
    [Register ("ImageCell")]
    partial class ImageCell
    {
        [Outlet]
        UIKit.UIButton _button { get; set; }


        [Outlet]
        UIKit.UIImageView _imageView { get; set; }

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
        }
    }
}