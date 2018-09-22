// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.Chat.Cells
{
	[Register ("ImageWithUserCell")]
	partial class ImageWithUserCell
	{
		[Outlet]
		UIKit.UIImageView _imageView { get; set; }

		[Outlet]
		UIKit.UILabel _nameLabel { get; set; }

		[Outlet]
		UIKit.UIButton _pictureButton { get; set; }

		[Outlet]
		UIKit.UIImageView _pictureImage { get; set; }

		[Outlet]
		UIKit.UIView _presenceView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_imageView != null) {
				_imageView.Dispose ();
				_imageView = null;
			}

			if (_nameLabel != null) {
				_nameLabel.Dispose ();
				_nameLabel = null;
			}

			if (_pictureButton != null) {
				_pictureButton.Dispose ();
				_pictureButton = null;
			}

			if (_pictureImage != null) {
				_pictureImage.Dispose ();
				_pictureImage = null;
			}

			if (_presenceView != null) {
				_presenceView.Dispose ();
				_presenceView = null;
			}
		}
	}
}
