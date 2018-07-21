// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.Division
{
	[Register ("DivisionViewController")]
	partial class DivisionViewController
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
		UIKit.UIImageView _memberImage { get; set; }

		[Outlet]
		UIKit.UILabel _membersLabel { get; set; }

		[Outlet]
		UIKit.UIView _profileHeaderView { get; set; }

		[Outlet]
		UIKit.UIImageView _profileImage { get; set; }

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

			if (_tableView != null) {
				_tableView.Dispose ();
				_tableView = null;
			}

			if (_profileImage != null) {
				_profileImage.Dispose ();
				_profileImage = null;
			}

			if (_memberImage != null) {
				_memberImage.Dispose ();
				_memberImage = null;
			}

			if (_membersLabel != null) {
				_membersLabel.Dispose ();
				_membersLabel = null;
			}

			if (_descriptionLabel != null) {
				_descriptionLabel.Dispose ();
				_descriptionLabel = null;
			}

			if (_profileHeaderView != null) {
				_profileHeaderView.Dispose ();
				_profileHeaderView = null;
			}
		}
	}
}
