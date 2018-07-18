using System;

using Foundation;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.UserSettings.Cells
{
    public partial class AllowPhoneCallsCell : UITableViewCell
    {
        private XPCommand<bool> _switchCommand;
        public static readonly NSString Key = new NSString("AllowPhoneCallsCell");
        public static readonly UINib Nib = UINib.FromName("AllowPhoneCallsCell", NSBundle.MainBundle);
        protected AllowPhoneCallsCell(IntPtr handle) : base(handle) {}

        public void Configure(SettingsAllowCallsModel settings)
        {
            _switchCommand = settings.AllowCalls;
            UILabelExtensions.SetupLabelAppearance(_title, settings.AllowCallsTitle, Colors.Black, 15f);
            UILabelExtensions.SetupLabelAppearance(_description, settings.AllowCallsDescription, Colors.Black, 12f);
            _switch.On = settings.IsActive;

            _switch.ValueChanged -= OnSwitch_ValueChanged;
            _switch.ValueChanged += OnSwitch_ValueChanged;
        }

        private void OnSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (_switchCommand.CanExecute(_switch.On))
                _switchCommand.Execute(_switch.On);
        }
    }
}
