// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.TabBar.ChatListViewController.Cells
{
    [Register ("ChatCell")]
    partial class ChatCell
    {
        [Outlet]
        UIKit.UIButton _chatButton { get; set; }


        [Outlet]
        UIKit.UILabel _dateLabel { get; set; }


        [Outlet]
        UIKit.UILabel _memberNameLabel { get; set; }


        [Outlet]
        UIKit.UILabel _messageLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _messageTrailConstraint { get; set; }


        [Outlet]
        UIKit.UIView _presenceView { get; set; }


        [Outlet]
        UIKit.UIImageView _profileImage { get; set; }


        [Outlet]
        UIKit.UIButton _profileImageButton { get; set; }


        [Outlet]
        UIKit.UIView _separatorLineView { get; set; }


        [Outlet]
        UIKit.UIImageView _silentImage { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _silentImageWidthConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _unreadCountHeightConstraint { get; set; }


        [Outlet]
        UIKit.UILabel _unreadCountLabel { get; set; }


        [Outlet]
        UIKit.UIView _unreadCountView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _unreadCountWidthConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_chatButton != null) {
                _chatButton.Dispose ();
                _chatButton = null;
            }

            if (_dateLabel != null) {
                _dateLabel.Dispose ();
                _dateLabel = null;
            }

            if (_memberNameLabel != null) {
                _memberNameLabel.Dispose ();
                _memberNameLabel = null;
            }

            if (_messageLabel != null) {
                _messageLabel.Dispose ();
                _messageLabel = null;
            }

            if (_messageTrailConstraint != null) {
                _messageTrailConstraint.Dispose ();
                _messageTrailConstraint = null;
            }

            if (_presenceView != null) {
                _presenceView.Dispose ();
                _presenceView = null;
            }

            if (_profileImage != null) {
                _profileImage.Dispose ();
                _profileImage = null;
            }

            if (_profileImageButton != null) {
                _profileImageButton.Dispose ();
                _profileImageButton = null;
            }

            if (_separatorLineView != null) {
                _separatorLineView.Dispose ();
                _separatorLineView = null;
            }

            if (_silentImage != null) {
                _silentImage.Dispose ();
                _silentImage = null;
            }

            if (_silentImageWidthConstraint != null) {
                _silentImageWidthConstraint.Dispose ();
                _silentImageWidthConstraint = null;
            }

            if (_unreadCountHeightConstraint != null) {
                _unreadCountHeightConstraint.Dispose ();
                _unreadCountHeightConstraint = null;
            }

            if (_unreadCountLabel != null) {
                _unreadCountLabel.Dispose ();
                _unreadCountLabel = null;
            }

            if (_unreadCountView != null) {
                _unreadCountView.Dispose ();
                _unreadCountView = null;
            }

            if (_unreadCountWidthConstraint != null) {
                _unreadCountWidthConstraint.Dispose ();
                _unreadCountWidthConstraint = null;
            }
        }
    }
}