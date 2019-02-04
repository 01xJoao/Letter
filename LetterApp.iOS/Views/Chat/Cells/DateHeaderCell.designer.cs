// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
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