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
	[Register ("ChatAlertViewController")]
	partial class ChatAlertViewController
	{
		[Outlet]
		UIKit.UIView _backgroundView { get; set; }

		[Outlet]
		UIKit.UILabel _descriptionLabel { get; set; }

		[Outlet]
		UIKit.UILabel _nameLabel { get; set; }

		[Outlet]
		UIKit.UIButton _openChatButton { get; set; }

		[Outlet]
		UIKit.UIImageView _pictureImage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_backgroundView != null) {
				_backgroundView.Dispose ();
				_backgroundView = null;
			}

			if (_pictureImage != null) {
				_pictureImage.Dispose ();
				_pictureImage = null;
			}

			if (_nameLabel != null) {
				_nameLabel.Dispose ();
				_nameLabel = null;
			}

			if (_descriptionLabel != null) {
				_descriptionLabel.Dispose ();
				_descriptionLabel = null;
			}

			if (_openChatButton != null) {
				_openChatButton.Dispose ();
				_openChatButton = null;
			}
		}
	}
}
