// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
	[Register ("DivisionCell")]
	partial class DivisionCell
	{
		[Outlet]
		UIKit.UIImageView _imageView { get; set; }

		[Outlet]
		UIKit.UILabel _subtitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel _titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_imageView != null) {
				_imageView.Dispose ();
				_imageView = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}

			if (_subtitleLabel != null) {
				_subtitleLabel.Dispose ();
				_subtitleLabel = null;
			}
		}
	}
}
