using System;
using System.Reflection;
using UIKit;

namespace LetterApp.iOS.Helpers
{
    public static class UITextFieldExtensions
    {
        public static void SetupField(UIView view, int tag, string indicatorText, UITextField textField, UIView divider, NSLayoutConstraint heightConstraint, 
                                      UILabel indicator, UIReturnKeyType returnKeyType = UIReturnKeyType.Default, bool isPassword = false)
        {
            textField.ReturnKeyType = returnKeyType;
            textField.SecureTextEntry = isPassword;
            textField.Tag = tag;
            indicator.Alpha = 0;
            textField.Placeholder = indicatorText;
            indicator.Text = indicatorText;
            divider.BackgroundColor = Colors.GrayDivider;
            heightConstraint.Constant = 25;
            indicator.TextColor = Colors.GrayIndicator;

            textField.ShouldReturn -= (field) => TextFieldShouldReturn(field, view);
            textField.ShouldReturn += (field) => TextFieldShouldReturn(field, view);

            textField.EditingDidBegin += (sender, e) =>
            {
                divider.BackgroundColor = Colors.MainBlue;
                indicator.TextColor = Colors.MainBlue;
            };
            textField.EditingDidEnd += (sender, e) =>
            {
                divider.BackgroundColor = Colors.GrayDivider;
                indicator.TextColor = Colors.GrayIndicator;
            };
            textField.EditingChanged += (sender, e) => UIView.Animate(0.3, () => indicator.Alpha = textField.Text.Length != 0 ? 1 : 0);
        }

        private static bool TextFieldShouldReturn(UITextField textField, UIView view)
        {
            var nextTag = textField.Tag + 1;
            var nextResponder = view.ViewWithTag(nextTag);
            return nextResponder != null && textField.ReturnKeyType == UIReturnKeyType.Next ? nextResponder.BecomeFirstResponder() : textField.ResignFirstResponder();
        }

        public static void SetupTextFieldAppearance(UITextField textField, UIColor color, nfloat textSize, UIFontWeight fontWeight = UIFontWeight.Regular)
        {
            textField.TextColor = color;
            textField.Font = UIFont.SystemFontOfSize(textSize, fontWeight);
        }
    }
}
