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
	[Register ("DateHeaderCell")]
	partial class DateHeaderCell
	{
		[Outlet]
		UIKit.UILabel _dateLabel { get; set; }

		[Outlet]
		UIKit.UIView _lineView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_dateLabel != null) {
				_dateLabel.Dispose ();
				_dateLabel = null;
			}

			if (_lineView != null) {
				_lineView.Dispose ();
				_lineView = null;
			}
		}
	}
}
