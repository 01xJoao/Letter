// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.UserSettings
{
    [Register ("UserSettingsViewController")]
    partial class UserSettingsViewController
    {
        [Outlet]
        UIKit.UITableView _tableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_tableView != null) {
                _tableView.Dispose ();
                _tableView = null;
            }
        }
    }
}