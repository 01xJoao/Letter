// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
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
