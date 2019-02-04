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
	[Register ("ShowImageViewController")]
	partial class ShowImageViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint _buttonHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIButton _closeButton { get; set; }

		[Outlet]
		UIKit.UIImageView _imageView { get; set; }

		[Outlet]
		UIKit.UIButton _saveButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_imageView != null) {
				_imageView.Dispose ();
				_imageView = null;
			}

			if (_closeButton != null) {
				_closeButton.Dispose ();
				_closeButton = null;
			}

			if (_saveButton != null) {
				_saveButton.Dispose ();
				_saveButton = null;
			}

			if (_buttonHeightConstraint != null) {
				_buttonHeightConstraint.Dispose ();
				_buttonHeightConstraint = null;
			}
		}
	}
}
