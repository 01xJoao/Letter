// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.TabBar.ChatListViewController.Cells
{
	[Register ("ChatCell")]
	partial class ChatCell
	{
		[Outlet]
		UIKit.UIButton _chatButton { get; set; }

		[Outlet]
		UIKit.UILabel _dateLabel { get; set; }

		[Outlet]
		UIKit.UILabel _memberNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel _messageLabel { get; set; }

		[Outlet]
		UIKit.UIView _presenceView { get; set; }

		[Outlet]
		UIKit.UIImageView _profileImage { get; set; }

		[Outlet]
		UIKit.UIButton _profileImageButton { get; set; }

		[Outlet]
		UIKit.UIView _separatorLineView { get; set; }

		[Outlet]
		UIKit.UIImageView _silentImage { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _silentImageWidthConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_silentImageWidthConstraint != null) {
				_silentImageWidthConstraint.Dispose ();
				_silentImageWidthConstraint = null;
			}

			if (_chatButton != null) {
				_chatButton.Dispose ();
				_chatButton = null;
			}

			if (_dateLabel != null) {
				_dateLabel.Dispose ();
				_dateLabel = null;
			}

			if (_memberNameLabel != null) {
				_memberNameLabel.Dispose ();
				_memberNameLabel = null;
			}

			if (_messageLabel != null) {
				_messageLabel.Dispose ();
				_messageLabel = null;
			}

			if (_presenceView != null) {
				_presenceView.Dispose ();
				_presenceView = null;
			}

			if (_profileImage != null) {
				_profileImage.Dispose ();
				_profileImage = null;
			}

			if (_profileImageButton != null) {
				_profileImageButton.Dispose ();
				_profileImageButton = null;
			}

			if (_separatorLineView != null) {
				_separatorLineView.Dispose ();
				_separatorLineView = null;
			}

			if (_silentImage != null) {
				_silentImage.Dispose ();
				_silentImage = null;
			}
		}
	}
}
