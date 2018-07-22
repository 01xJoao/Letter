// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
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
			if (_imageView != null) {
				_imageView.Dispose ();
				_imageView = null;
			}

			if (_descriptionLabel != null) {
				_descriptionLabel.Dispose ();
				_descriptionLabel = null;
			}

			if (_tableView != null) {
				_tableView.Dispose ();
				_tableView = null;
			}

			if (_buttonView1 != null) {
				_buttonView1.Dispose ();
				_buttonView1 = null;
			}

			if (_buttonView2 != null) {
				_buttonView2.Dispose ();
				_buttonView2 = null;
			}

			if (_button1 != null) {
				_button1.Dispose ();
				_button1 = null;
			}

			if (_button2 != null) {
				_button2.Dispose ();
				_button2 = null;
			}
		}
	}
}
