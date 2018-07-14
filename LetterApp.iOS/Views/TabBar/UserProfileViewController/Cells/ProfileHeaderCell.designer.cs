// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.TabBar.UserProfileViewController.Cells
{
    [Register ("ProfileHeaderCell")]
    partial class ProfileHeaderCell
    {
        [Outlet]
        UIKit.UIView _backgroundView { get; set; }


        [Outlet]
        UIKit.UIButton _configButton { get; set; }


        [Outlet]
        UIKit.UIImageView _configImage { get; set; }


        [Outlet]
        UIKit.UITextField _descriptionField { get; set; }


        [Outlet]
        UIKit.UILabel _nameLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _pictureHeightConstraint { get; set; }


        [Outlet]
        UIKit.UIButton _profileButton { get; set; }


        [Outlet]
        UIKit.UIImageView _profileImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_configButton != null) {
                _configButton.Dispose ();
                _configButton = null;
            }

            if (_configImage != null) {
                _configImage.Dispose ();
                _configImage = null;
            }

            if (_descriptionField != null) {
                _descriptionField.Dispose ();
                _descriptionField = null;
            }

            if (_nameLabel != null) {
                _nameLabel.Dispose ();
                _nameLabel = null;
            }

            if (_pictureHeightConstraint != null) {
                _pictureHeightConstraint.Dispose ();
                _pictureHeightConstraint = null;
            }

            if (_profileButton != null) {
                _profileButton.Dispose ();
                _profileButton = null;
            }

            if (_profileImage != null) {
                _profileImage.Dispose ();
                _profileImage = null;
            }
        }
    }
}