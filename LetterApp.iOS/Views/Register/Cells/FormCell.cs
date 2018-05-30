using System;

using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.Register.Cells
{
    public partial class FormCell : UITableViewCell
    {
        private RegisterFormModel _registerForm;
        public static readonly NSString Key = new NSString("FormCell");
        public static readonly UINib Nib = UINib.FromName("FormCell", NSBundle.MainBundle);
        protected FormCell(IntPtr handle) : base(handle) {}

        public void Configure(string text, RegisterFormModel registerForm, UIView view, int row, bool isLast)
        {
            _registerForm = registerForm;
            UITextFieldExtensions.SetupField(view, row, text, _textField, _indicatorView, _textFieldHeightConstraint, _indicatorLabel, isLast ? UIReturnKeyType.Default : UIReturnKeyType.Next);

            string value = registerForm.ReturnValue(row);

            if(!string.IsNullOrEmpty(value))
            {
                _textField.Text = value;
                _indicatorView.BackgroundColor = Colors.GrayDivider;
                _indicatorLabel.TextColor = Colors.GrayIndicator;
                _indicatorLabel.Alpha = 1;
            }
        }
    }
}
