using System;
using Foundation;
using LetterApp.Core.Models.Generic;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class SwitchCell : UITableViewCell
    {
        private EventHandler<bool> _muteEvent;
        private DescriptionAndBoolEventModel _cell;
        public static readonly NSString Key = new NSString("SwitchCell");
        public static readonly UINib Nib = UINib.FromName("SwitchCell", NSBundle.MainBundle);
        protected SwitchCell(IntPtr handle) : base(handle) {}

        public void Configure(DescriptionAndBoolEventModel cell)
        {
            _cell = cell;
            UILabelExtensions.SetupLabelAppearance(_label, cell.Description, Colors.Black, 15f);
            _switch.On = cell.IsActive;

            _switch.ValueChanged -= OnSwitch_ValueChanged;
            _switch.ValueChanged += OnSwitch_ValueChanged;
        }

        private void OnSwitch_ValueChanged(object sender, EventArgs e)
        {
            _cell.BooleanEvent?.Invoke(this, _switch.On);
            _cell.IsActive = _switch.On;
        }

        public void ConfigureForChat(string text, bool isOn, EventHandler<bool> muteEvent)
        {
            _muteEvent = muteEvent;
            UILabelExtensions.SetupLabelAppearance(_label, text, Colors.Black, 15f);
            _switch.On = isOn;
            _switch.OnTintColor = Colors.Red;
  
            _switch.ValueChanged -= OnSwitch_ChatEvent;
            _switch.ValueChanged += OnSwitch_ChatEvent;
        }

        private void OnSwitch_ChatEvent(object sender, EventArgs e)
        {
            _muteEvent?.Invoke(this, _switch.On);
        }
    }
}
