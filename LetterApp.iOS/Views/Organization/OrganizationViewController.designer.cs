// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.Organization
{
    [Register ("OrganizationViewController")]
    partial class OrganizationViewController
    {
        [Outlet]
        UIKit.UIButton _button1 { get; set; }


        [Outlet]
        UIKit.UIButton _button2 { get; set; }


        [Outlet]
        UIKit.UIView _buttonView1 { get; set; }


        [Outlet]
        UIKit.UIView _buttonView2 { get; set; }


        [Outlet]
        UIKit.UILabel _descriptionLabel { get; set; }


        [Outlet]
        UIKit.UIImageView _imageView { get; set; }


        [Outlet]
        UIKit.UITableView _tableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_button1 != null) {
                _button1.Dispose ();
                _button1 = null;
            }

            if (_button2 != null) {
                _button2.Dispose ();
                _button2 = null;
            }

            if (_buttonView1 != null) {
                _buttonView1.Dispose ();
                _buttonView1 = null;
            }

            if (_buttonView2 != null) {
                _buttonView2.Dispose ();
                _buttonView2 = null;
            }

            if (_descriptionLabel != null) {
                _descriptionLabel.Dispose ();
                _descriptionLabel = null;
            }

            if (_imageView != null) {
                _imageView.Dispose ();
                _imageView = null;
            }

            if (_tableView != null) {
                _tableView.Dispose ();
                _tableView = null;
            }
        }
    }
}