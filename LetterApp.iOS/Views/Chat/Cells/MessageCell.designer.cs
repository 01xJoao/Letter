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
    [Register ("MessageCell")]
    partial class MessageCell
    {
        [Outlet]
        UIKit.UIImageView _imageView { get; set; }


        [Outlet]
        UIKit.UILabel _messageLabel { get; set; }


        [Outlet]
        UIKit.UILabel _nameLabel { get; set; }


        [Outlet]
        UIKit.UIView _presenceView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_imageView != null) {
                _imageView.Dispose ();
                _imageView = null;
            }

            if (_messageLabel != null) {
                _messageLabel.Dispose ();
                _messageLabel = null;
            }

            if (_nameLabel != null) {
                _nameLabel.Dispose ();
                _nameLabel = null;
            }

            if (_presenceView != null) {
                _presenceView.Dispose ();
                _presenceView = null;
            }
        }
    }
}