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
	[Register ("LabelCell")]
	partial class LabelCell
	{
		[Outlet]
		UIKit.UILabel _textLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_textLabel != null) {
				_textLabel.Dispose ();
				_textLabel = null;
			}
		}
	}
}
