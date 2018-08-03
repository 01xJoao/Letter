// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.CustomViews.ProfileDivision
{
    partial class OrganizationDivisionsView
    {
        [Outlet]
        UIKit.UIImageView _imageView { get; set; }


        [Outlet]
        UIKit.UILabel _membersLabel { get; set; }


        [Outlet]
        UIKit.UILabel _nameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_imageView != null) {
                _imageView.Dispose ();
                _imageView = null;
            }

            if (_membersLabel != null) {
                _membersLabel.Dispose ();
                _membersLabel = null;
            }

            if (_nameLabel != null) {
                _nameLabel.Dispose ();
                _nameLabel = null;
            }
        }
    }
}