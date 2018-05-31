// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.Register.Cells
{
	[Register ("HeaderCell")]
	partial class HeaderCell
	{
		[Outlet]
		UIKit.UIImageView _imageView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_imageView != null) {
				_imageView.Dispose ();
				_imageView = null;
			}
		}
	}
}
