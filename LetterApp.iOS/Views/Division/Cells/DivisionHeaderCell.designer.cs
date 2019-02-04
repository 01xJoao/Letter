// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.Division.Cells
{
    [Register ("DivisionHeaderCell")]
    partial class DivisionHeaderCell
    {
        [Outlet]
        UIKit.UIButton _backButton { get; set; }


        [Outlet]
        UIKit.UIImageView _backImage { get; set; }


        [Outlet]
        UIKit.UILabel _descriptionLabel { get; set; }


        [Outlet]
        UIKit.UIImageView _membersImage { get; set; }


        [Outlet]
        UIKit.UILabel _membersLabel { get; set; }


        [Outlet]
        UIKit.UILabel _nameLabel { get; set; }


        [Outlet]
        UIKit.UIImageView _profileImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_backButton != null) {
                _backButton.Dispose ();
                _backButton = null;
            }

            if (_backImage != null) {
                _backImage.Dispose ();
                _backImage = null;
            }

            if (_descriptionLabel != null) {
                _descriptionLabel.Dispose ();
                _descriptionLabel = null;
            }

            if (_membersImage != null) {
                _membersImage.Dispose ();
                _membersImage = null;
            }

            if (_membersLabel != null) {
                _membersLabel.Dispose ();
                _membersLabel = null;
            }

            if (_nameLabel != null) {
                _nameLabel.Dispose ();
                _nameLabel = null;
            }

            if (_profileImage != null) {
                _profileImage.Dispose ();
                _profileImage = null;
            }
        }
    }
}