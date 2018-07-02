using System;

using Foundation;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class InsertDivisionFieldCell : UITableViewCell
    {
        private EventHandler<bool> _scrollToRow;
        private EventHandler<string> _onSubmitButton;

        public static readonly NSString Key = new NSString("InsertDivisionFieldCell");
        public static readonly UINib Nib = UINib.FromName("InsertDivisionFieldCell", NSBundle.MainBundle);
        protected InsertDivisionFieldCell(IntPtr handle) : base(handle) {}

        public void Configure(string hint, string submit, EventHandler<string> eventHandler, EventHandler<bool> scrollToRow)
        {
            _onSubmitButton = eventHandler;
            _scrollToRow = scrollToRow;

            UITextFieldExtensions.SetupTextFieldAppearance(_textField, Colors.White, 22f, hint, Colors.White70, Colors.White, Colors.SelectBlue);
            UIButtonExtensions.SetupButtonAppearance(_submitButton, Colors.White, 18f, submit);

            _buttonView.Layer.CornerRadius = 2;
            _buttonView.ClipsToBounds = true;
            _buttonView.Layer.BorderWidth = 1;
            _buttonView.Layer.BorderColor = Colors.White.CGColor;

            _textField.AutocorrectionType = UITextAutocorrectionType.No;
            _textField.TextContentType = new NSString("");
            _textField.ReturnKeyType = UIReturnKeyType.Join;

            _submitButton.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _submitButton.TouchUpInside += OnSubmitButton_TouchUpInside;

            _textField.EditingDidBegin -= OnTextField_EditingDidBegin;
            _textField.EditingDidBegin += OnTextField_EditingDidBegin;

            _textField.EditingDidEnd -= OnTextField_EditingDidEnd;
            _textField.EditingDidEnd += OnTextField_EditingDidEnd;

            _textField.ShouldReturn -= OnTextField_ShouldReturn;
            _textField.ShouldReturn += OnTextField_ShouldReturn;
        }

        private bool OnTextField_ShouldReturn(UITextField textField)
        {
            _onSubmitButton?.Invoke(this, textField.Text);
            textField.ResignFirstResponder();
            return false;
        }

        private void OnTextField_EditingDidBegin(object sender, EventArgs e)
        {
            _scrollToRow?.Invoke(this, true);
        }

        private void OnTextField_EditingDidEnd(object sender, EventArgs e)
        {
            _scrollToRow?.Invoke(this, false);
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            _onSubmitButton?.Invoke(this, _textField.Text);
        }
    }
}
