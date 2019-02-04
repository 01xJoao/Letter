// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
	[Register ("ShowChatOptionsViewController")]
	partial class ShowChatOptionsViewController
	{
		[Outlet]
		UIKit.UIImageView _backgroundImage { get; set; }

		[Outlet]
		UIKit.UIView _backgroundView { get; set; }

		[Outlet]
		UIKit.UIButton _button { get; set; }

		[Outlet]
		UIKit.UIView _buttonView { get; set; }

		[Outlet]
		UIKit.UILabel _nameLabel { get; set; }

		[Outlet]
		UIKit.UIImageView _profileImage { get; set; }

		[Outlet]
		UIKit.UITableView _tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_backgroundView != null) {
				_backgroundView.Dispose ();
				_backgroundView = null;
			}

			if (_backgroundImage != null) {
				_backgroundImage.Dispose ();
				_backgroundImage = null;
			}

			if (_profileImage != null) {
				_profileImage.Dispose ();
				_profileImage = null;
			}

			if (_tableView != null) {
				_tableView.Dispose ();
				_tableView = null;
			}

			if (_nameLabel != null) {
				_nameLabel.Dispose ();
				_nameLabel = null;
			}

			if (_buttonView != null) {
				_buttonView.Dispose ();
				_buttonView = null;
			}

			if (_button != null) {
				_button.Dispose ();
				_button = null;
			}
		}
	}
}
