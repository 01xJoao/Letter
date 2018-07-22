// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.Organization.Cells
{
	[Register ("OrganizationDivisionsCell")]
	partial class OrganizationDivisionsCell
	{
		[Outlet]
		UIKit.UIScrollView _scrollView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_scrollView != null) {
				_scrollView.Dispose ();
				_scrollView = null;
			}
		}
	}
}
