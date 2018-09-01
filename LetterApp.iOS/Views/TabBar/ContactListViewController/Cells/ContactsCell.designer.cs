// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.TabBar.ContactListViewController.Cells
{
    [Register ("ContactsCell")]
    partial class ContactsCell
    {
        [Outlet]
        UIKit.UIButton _callButton { get; set; }


        [Outlet]
        UIKit.UIImageView _callImage { get; set; }


        [Outlet]
        UIKit.UIButton _chatButton { get; set; }


        [Outlet]
        UIKit.UIImageView _chatImage { get; set; }


        [Outlet]
        UIKit.UIImageView _imageView { get; set; }


        [Outlet]
        UIKit.UILabel _nameLabel { get; set; }


        [Outlet]
        UIKit.UIButton _profileButton { get; set; }


        [Outlet]
        UIKit.UILabel _roleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_callButton != null) {
                _callButton.Dispose ();
                _callButton = null;
            }

            if (_callImage != null) {
                _callImage.Dispose ();
                _callImage = null;
            }

            if (_chatButton != null) {
                _chatButton.Dispose ();
                _chatButton = null;
            }

            if (_chatImage != null) {
                _chatImage.Dispose ();
                _chatImage = null;
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