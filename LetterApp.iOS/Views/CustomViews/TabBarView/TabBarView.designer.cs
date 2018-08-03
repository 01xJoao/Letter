// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.CustomViews.TabBarView
{
    partial class TabBarView
    {
        [Outlet]
        UIKit.UILabel _label { get; set; }


        [Outlet]
        UIKit.UIButton _tabButton { get; set; }


        [Outlet]
        UIKit.UIView _underLineView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_label != null) {
                _label.Dispose ();
                _label = null;
            }

            if (_tabButton != null) {
                _tabButton.Dispose ();
                _tabButton = null;
            }

            if (_underLineView != null) {
                _underLineView.Dispose ();
                _underLineView = null;
            }
        }
    }
}