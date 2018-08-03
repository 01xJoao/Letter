// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.TabBar.CallListViewController.Cells
{
    [Register ("CallCell")]
    partial class CallCell
    {
        [Outlet]
        UIKit.UILabel _callLabel { get; set; }


        [Outlet]
        UIKit.UILabel _dateLabel { get; set; }


        [Outlet]
        UIKit.UIImageView _imageView { get; set; }


        [Outlet]
        UIKit.UILabel _nameLabel { get; set; }


        [Outlet]
        UIKit.UILabel _roleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_callLabel != null) {
                _callLabel.Dispose ();
                _callLabel = null;
            }

            if (_dateLabel != null) {
                _dateLabel.Dispose ();
                _dateLabel = null;
            }

            if (_imageView != null) {
                _imageView.Dispose ();
                _imageView = null;
            }

            if (_nameLabel != null) {
                _nameLabel.Dispose ();
                _nameLabel = null;
            }

            if (_roleLabel != null) {
                _roleLabel.Dispose ();
                _roleLabel = null;
            }
        }
    }
}