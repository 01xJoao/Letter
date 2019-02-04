// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.SelectDivision
{
    [Register ("SelectDivisionViewController")]
    partial class SelectDivisionViewController
    {
        [Outlet]
        UIKit.UIView _backgroundView { get; set; }


        [Outlet]
        UIKit.UIButton _closeButton { get; set; }


        [Outlet]
        UIKit.UITableView _tableView { get; set; }


        [Outlet]
        UIKit.UILabel _titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_backgroundView != null) {
                _backgroundView.Dispose ();
                _backgroundView = null;
            }

            if (_closeButton != null) {
                _closeButton.Dispose ();
                _closeButton = null;
            }

            if (_tableView != null) {
                _tableView.Dispose ();
                _tableView = null;
            }

            if (_titleLabel != null) {
                _titleLabel.Dispose ();
                _titleLabel = null;
            }
        }
    }
}