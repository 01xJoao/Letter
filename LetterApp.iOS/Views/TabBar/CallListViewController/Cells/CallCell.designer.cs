// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.TabBar.CallListViewController.Cells
{
	[Register ("CallCell")]
	partial class CallCell
	{
		[Outlet]
		UIKit.UIImageView _callInfoImage { get; set; }

		[Outlet]
		UIKit.UILabel _dateLabel { get; set; }

		[Outlet]
		UIKit.UIImageView _imageView { get; set; }

		[Outlet]
		UIKit.UILabel _nameLabel { get; set; }

		[Outlet]
		UIKit.UIButton _openProfileButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_callInfoImage != null) {
				_callInfoImage.Dispose ();
				_callInfoImage = null;
			}

			if (_dateLabel != null) {
				_dateLabel.Dispose ();
				_dateLabel = null;
			}

			if (_imageView != null) {
				_imageView.Dispose ();
				_imageView = null;
			}

			if (_nameLabel != null) {
				_nameLabel.Dispose ();
				_nameLabel = null;
			}

			if (_openProfileButton != null) {
				_openProfileButton.Dispose ();
				_openProfileButton = null;
			}
		}
	}
}
