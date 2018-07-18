using System;

using Foundation;
using LetterApp.Core.Models.Generic;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class SwitchCell : UITableViewCell
    {
        private EventHandler<bool> _booleanEvent;
        public static readonly NSString Key = new NSString("SwitchCell");
        public static readonly UINib Nib = UINib.FromName("SwitchCell", NSBundle.MainBundle);
        protected SwitchCell(IntPtr handle) : base(handle) {}

        public void Configure(DescriptionAndBoolEventModel cell)
        {
            _booleanEvent = cell.BooleanEvent;
            UILabelExtensions.SetupLabelAppearance(_label, cell.Description, Colors.Black, 15f);
            _switch.On = cell.IsActive;

            _switch.ValueChanged -= OnSwitch_ValueChanged;
            _switch.ValueChanged += OnSwitch_ValueChanged;
        }

        private void OnSwitch_ValueChanged(object sender, EventArgs e)
        {
            _booleanEvent?.Invoke(this, _switch.On);
        }
    }
}
