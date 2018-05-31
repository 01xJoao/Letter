using System;

using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class MainButtonCell : UITableViewCell
    {
        EventHandler<RegisterFormModel> _buttonClick;
        RegisterFormModel _form;
        public static readonly NSString Key = new NSString("MainButtonCell");
        public static readonly UINib Nib = UINib.FromName("MainButtonCell", NSBundle.MainBundle);
        protected MainButtonCell(IntPtr handle) : base(handle) {}

        public void Configure(string buttonName, EventHandler<RegisterFormModel> buttonClick, RegisterFormModel form)
        {
            _form = form;
            _buttonClick = buttonClick;
            UIButtonExtensions.SetupButtonAppearance(_button, Colors.White, 16f, buttonName);            
            this.BackgroundColor = Colors.MainBlue;

            _button.TouchUpInside -= OnButton_TouchUpInside;
            _button.TouchUpInside += OnButton_TouchUpInside;
        }

        private void OnButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonClick?.Invoke(sender, _form);
        }
    }
}
