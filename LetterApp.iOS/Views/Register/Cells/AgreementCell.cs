using System;

using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.Register.Cells
{
    public partial class AgreementCell : UITableViewCell
    {
        RegisterFormModel _registerForm;
        public static readonly NSString Key = new NSString("AgreementCell");
        public static readonly UINib Nib = UINib.FromName("AgreementCell", NSBundle.MainBundle);
        protected AgreementCell(IntPtr handle) : base(handle) {}

        public void Configure(string text, RegisterFormModel registerForm)
        {
            _registerForm = registerForm;
            ButtonState();
            _label.AttributedText = StringExtensions.GetHTMLFormattedText(text, fontSize: 3);

            _button.TouchUpInside -= OnButton_TouchUpInside;
            _button.TouchUpInside += OnButton_TouchUpInside;
        }

        private void OnButton_TouchUpInside(object sender, EventArgs e)
        {
            _registerForm.UserAgreed = !_registerForm.UserAgreed;
            ButtonState();
        }

        private void ButtonState()
        {
            _button.SetImage(_registerForm.UserAgreed ? UIImage.FromBundle("checkbox_full") : UIImage.FromBundle("checkbox_empty"), UIControlState.Normal);
        }
    }
}
