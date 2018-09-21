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
    [Register ("CallHistoryCell")]
    partial class CallHistoryCell
    {
        [Outlet]
        UIKit.UILabel _callerInfoLabel { get; set; }


        [Outlet]
        UIKit.UILabel _callingTypeLabel { get; set; }


        [Outlet]
        UIKit.UILabel _dateLabel { get; set; }


        [Outlet]
        UIKit.UIImageView _imageView { get; set; }


        [Outlet]
        UIKit.UIButton _openProfileButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_callerInfoLabel != null) {
                _callerInfoLabel.Dispose ();
                _callerInfoLabel = null;
            }

            if (_callingTypeLabel != null) {
                _callingTypeLabel.Dispose ();
                _callingTypeLabel = null;
            }

            if (_dateLabel != null) {
                _dateLabel.Dispose ();
                _dateLabel = null;
            }

            if (_imageView != null) {
                _imageView.Dispose ();
                _imageView = null;
            }

            if (_openProfileButton != null) {
                _openProfileButton.Dispose ();
                _openProfileButton = null;
            }
        }
    }
}