// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.TabBar.ContactListViewController
{
	[Register ("ContactListViewController")]
	partial class ContactListViewController
	{
		[Outlet]
		UIKit.UIButton _cancelButton { get; set; }

		[Outlet]
		UIKit.UIView _pageView { get; set; }

		[Outlet]
		UIKit.UIView _presentView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _presentViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _scrollBarTopConstraint { get; set; }

		[Outlet]
		UIKit.UIView _separatorView { get; set; }

		[Outlet]
		UIKit.UIView _tabBarView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _tabBarViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _tabScrollTopConstraint { get; set; }

		[Outlet]
		UIKit.UIScrollView _tabScrollView { get; set; }

		[Outlet]
		UIKit.UILabel _titleLabel { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _titleLabelHeightConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_cancelButton != null) {
				_cancelButton.Dispose ();
				_cancelButton = null;
			}

			if (_pageView != null) {
				_pageView.Dispose ();
				_pageView = null;
			}

			if (_presentView != null) {
				_presentView.Dispose ();
				_presentView = null;
			}

			if (_presentViewHeightConstraint != null) {
				_presentViewHeightConstraint.Dispose ();
				_presentViewHeightConstraint = null;
			}

			if (_scrollBarTopConstraint != null) {
				_scrollBarTopConstraint.Dispose ();
				_scrollBarTopConstraint = null;
			}

			if (_separatorView != null) {
				_separatorView.Dispose ();
				_separatorView = null;
			}

			if (_tabBarView != null) {
				_tabBarView.Dispose ();
				_tabBarView = null;
			}

			if (_tabBarViewHeightConstraint != null) {
				_tabBarViewHeightConstraint.Dispose ();
				_tabBarViewHeightConstraint = null;
			}

			if (_tabScrollTopConstraint != null) {
				_tabScrollTopConstraint.Dispose ();
				_tabScrollTopConstraint = null;
			}

			if (_tabScrollView != null) {
				_tabScrollView.Dispose ();
				_tabScrollView = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}

			if (_titleLabelHeightConstraint != null) {
				_titleLabelHeightConstraint.Dispose ();
				_titleLabelHeightConstraint = null;
			}
		}
	}
}
