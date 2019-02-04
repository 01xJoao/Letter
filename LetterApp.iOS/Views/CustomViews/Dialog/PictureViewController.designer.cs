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
	[Register ("PictureViewController")]
	partial class PictureViewController
	{
		[Outlet]
		UIKit.UIView _blurView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _bottomHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIButton _cancelButton { get; set; }

		[Outlet]
		UIKit.UIImageView _imageView { get; set; }

		[Outlet]
		UIKit.UIButton _sendButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_blurView != null) {
				_blurView.Dispose ();
				_blurView = null;
			}

			if (_cancelButton != null) {
				_cancelButton.Dispose ();
				_cancelButton = null;
			}

			if (_imageView != null) {
				_imageView.Dispose ();
				_imageView = null;
			}

			if (_sendButton != null) {
				_sendButton.Dispose ();
				_sendButton = null;
			}

			if (_bottomHeightConstraint != null) {
				_bottomHeightConstraint.Dispose ();
				_bottomHeightConstraint = null;
			}
		}
	}
}
