// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.TabBar.UserProfileViewController.Cells
{
    [Register ("DivisionsCell")]
    partial class DivisionsCell
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