// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.TabBar.ChatListViewController
{
    [Register ("ChatListViewController")]
    partial class ChatListViewController
    {
        [Outlet]
        UIKit.UITableView _tableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _tableViewTopConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_tableView != null) {
                _tableView.Dispose ();
                _tableView = null;
            }

            if (_tableViewTopConstraint != null) {
                _tableViewTopConstraint.Dispose ();
                _tableViewTopConstraint = null;
            }
        }
    }
}