using System;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.UserSettings.Cells
{
    public partial class AllowPhoneCallsCell : UITableViewCell
    {
        private SettingsAllowCallsModel _settings;
        public static readonly NSString Key = new NSString("AllowPhoneCallsCell");
        public static readonly UINib Nib = UINib.FromName("AllowPhoneCallsCell", NSBundle.MainBundle);
        protected AllowPhoneCallsCell(IntPtr handle) : base(handle) {}

        public void Configure(SettingsAllowCallsModel settings)
        {
            _settings = settings;
            UILabelExtensions.SetupLabelAppearance(_title, settings.AllowCallsTitle, Colors.Black, 15f);
            UILabelExtensions.SetupLabelAppearance(_description, settings.AllowCallsDescription, Colors.Black, 12f);
            _switch.On = settings.IsActive;

            _switch.ValueChanged -= OnSwitch_ValueChanged;
            _switch.ValueChanged += OnSwitch_ValueChanged;
        }

        private void OnSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (_settings.AllowCalls.CanExecute(_switch.On))
            {
                _settings.AllowCalls.Execute(_switch.On);
                _settings.IsActive = _switch.On;
            }
        }
    }
}
