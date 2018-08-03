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
    [Register ("ProfileHeaderView")]
    partial class ProfileHeaderView
    {
        [Outlet]
        UIKit.UITextField _descriptionField { get; set; }


        [Outlet]
        UIKit.UILabel _nameLabel { get; set; }


        [Outlet]
        UIKit.UIButton _profileButton { get; set; }


        [Outlet]
        UIKit.UIImageView _profileImage { get; set; }


        [Outlet]
        UIKit.UIButton _settingsButton { get; set; }


        [Outlet]
        UIKit.UIImageView _settingsImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_descriptionField != null) {
                _descriptionField.Dispose ();
                _descriptionField = null;
            }

            if (_nameLabel != null) {
                _nameLabel.Dispose ();
                _nameLabel = null;
            }

            if (_profileButton != null) {
                _profileButton.Dispose ();
                _profileButton = null;
            }

            if (_profileImage != null) {
                _profileImage.Dispose ();
                _profileImage = null;
            }

            if (_settingsButton != null) {
                _settingsButton.Dispose ();
                _settingsButton = null;
            }

            if (_settingsImage != null) {
                _settingsImage.Dispose ();
                _settingsImage = null;
            }
        }
    }
}