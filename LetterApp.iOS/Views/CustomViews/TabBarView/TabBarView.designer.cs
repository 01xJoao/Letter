// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.CustomViews.TabBarView
{
	partial class TabBarView
	{
		[Outlet]
		UIKit.UILabel _label { get; set; }

		[Outlet]
		UIKit.UIButton _tabButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_tabButton != null) {
				_tabButton.Dispose ();
				_tabButton = null;
			}

			if (_label != null) {
				_label.Dispose ();
				_label = null;
			}
		}
	}
}
