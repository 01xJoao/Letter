using System;

using Foundation;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class InsertDivisionFieldCell : UITableViewCell
    {
        public EventHandler<string> OnSubmitButton;
        public static readonly NSString Key = new NSString("InsertDivisionFieldCell");
        public static readonly UINib Nib = UINib.FromName("InsertDivisionFieldCell", NSBundle.MainBundle);
        protected InsertDivisionFieldCell(IntPtr handle) : base(handle) {}

        public void Configure(string hint, string submit, EventHandler<string> eventHandler)
        {
            OnSubmitButton = eventHandler;

            UITextFieldExtensions.SetupTextFieldAppearance(_textField, Colors.White, 22f, hint, Colors.White70, Colors.White, Colors.SelectBlue);
            UIButtonExtensions.SetupButtonAppearance(_submitButton, Colors.White, 18f, submit);

            _buttonView.Layer.CornerRadius = 2;
            _buttonView.ClipsToBounds = true;
            _buttonView.Layer.BorderWidth = 1;
            _buttonView.Layer.BorderColor = Colors.White.CGColor;

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            OnSubmitButton?.Invoke(this, _textField.Text);
        }
    }
}
