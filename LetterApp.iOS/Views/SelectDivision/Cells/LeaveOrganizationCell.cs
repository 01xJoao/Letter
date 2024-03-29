﻿using System;
using Foundation;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class LeaveOrganizationCell : UITableViewCell
    {
        private EventHandler _onLeaveOrganization;

        public static readonly NSString Key = new NSString("LeaveOrganizationCell");
        public static readonly UINib Nib = UINib.FromName("LeaveOrganizationCell", NSBundle.MainBundle);
        protected LeaveOrganizationCell(IntPtr handle) : base(handle) {}

        public void Configure(string text, EventHandler eventHandler)
        {
            _onLeaveOrganization = eventHandler;

            UIButtonExtensions.SetupButtonUnderlineAppearance(_leaveButton, Colors.White, 14f, text);

            _leaveButton.TouchUpInside -= OnLeaveButton_TouchUpInside;
            _leaveButton.TouchUpInside += OnLeaveButton_TouchUpInside;
        }

        private void OnLeaveButton_TouchUpInside(object sender, EventArgs e)
        {
            _onLeaveOrganization?.Invoke(this, EventArgs.Empty);
        }
    }
}
