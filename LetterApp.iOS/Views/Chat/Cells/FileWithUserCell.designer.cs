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
	[Register ("FileWithUserCell")]
	partial class FileWithUserCell
	{
		[Outlet]
		UIKit.UIButton _fileButton { get; set; }

		[Outlet]
		UIKit.UIImageView _fileImage { get; set; }

		[Outlet]
		UIKit.UILabel _fileLabel { get; set; }

		[Outlet]
		UIKit.UIImageView _imageView { get; set; }

		[Outlet]
		UIKit.UILabel _nameLabel { get; set; }

		[Outlet]
		UIKit.UIView _presenceView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_fileImage != null) {
				_fileImage.Dispose ();
				_fileImage = null;
			}

			if (_fileLabel != null) {
				_fileLabel.Dispose ();
				_fileLabel = null;
			}

			if (_imageView != null) {
				_imageView.Dispose ();
				_imageView = null;
			}

			if (_nameLabel != null) {
				_nameLabel.Dispose ();
				_nameLabel = null;
			}

			if (_presenceView != null) {
				_presenceView.Dispose ();
				_presenceView = null;
			}

			if (_fileButton != null) {
				_fileButton.Dispose ();
				_fileButton = null;
			}
		}
	}
}
