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
    [Register ("DetailsCell")]
    partial class DetailsCell
    {
        [Outlet]
        UIKit.UILabel _detailLabel { get; set; }


        [Outlet]
        UIKit.UILabel _detailValueLabel { get; set; }


        [Outlet]
        UIKit.UIView _separatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_detailLabel != null) {
                _detailLabel.Dispose ();
                _detailLabel = null;
            }

            if (_detailValueLabel != null) {
                _detailValueLabel.Dispose ();
                _detailValueLabel = null;
            }

            if (_separatorView != null) {
                _separatorView.Dispose ();
                _separatorView = null;
            }
        }
    }
}