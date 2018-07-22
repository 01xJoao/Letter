using System;
using System.Drawing;
using System.Reflection;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LetterApp.iOS.Helpers
{
    public static class UITextFieldExtensions
    {
        public static void SetupField(UIView view, int tag, string indicatorText, UITextField textField, UIView divider, NSLayoutConstraint heightConstraint, UILabel indicator, 
                                      UIReturnKeyType returnKeyType = UIReturnKeyType.Default, UIButton keyboardButton = null)
        {
            textField.ReturnKeyType = returnKeyType;
            textField.Tag = tag;
            indicator.Alpha = 0;
            textField.Placeholder = indicatorText;
            textField.TintColor = Colors.MainBlue;
            textField.AttributedPlaceholder = new NSAttributedString(indicatorText, new UIStringAttributes() { ForegroundColor = Colors.GrayIndicator } );
            indicator.Text = indicatorText;
            divider.BackgroundColor = Colors.GrayDivider;
            heightConstraint.Constant = 25;
            indicator.TextColor = Colors.GrayIndicator;
                
            textField.ShouldReturn -= (field) => TextFieldShouldReturn(field, view);
            textField.ShouldReturn += (field) => TextFieldShouldReturn(field, view);

            textField.EditingDidBegin += (sender, e) =>
            {
                if (keyboardButton != null)
                    AddViewToKeyboard(textField, keyboardButton, Colors.MainBlue);
                
                divider.BackgroundColor = Colors.MainBlue;
                indicator.TextColor = Colors.MainBlue;
            };

            textField.EditingDidEnd += (sender, e) =>
            {
                textField.InputAccessoryView = null;

                if (string.IsNullOrEmpty(textField.Text))
                    divider.BackgroundColor = Colors.Red;
                else
                    divider.BackgroundColor = Colors.GrayDivider;

                indicator.TextColor = Colors.GrayIndicator;
            };

            textField.EditingChanged += (sender, e) => UIView.Animate(0.3, () => indicator.Alpha = textField.Text.Length != 0 ? 1 : 0);
        }

        private static bool TextFieldShouldReturn(UITextField textField, UIView view)
        {
            var nextTag = textField.Tag + 1;
            var nextResponder = view?.ViewWithTag(nextTag);
            return nextResponder != null && textField.ReturnKeyType == UIReturnKeyType.Next ? nextResponder.BecomeFirstResponder() : textField.ResignFirstResponder();
        }

        public static void AddViewToKeyboard(UITextField textField, UIButton button, UIColor buttonColor)
        {
            UIToolbar toolbar = new UIToolbar(new RectangleF(0f, 0f, 0f, 50f));
            toolbar.Translucent = false;
            toolbar.BarTintColor = buttonColor;
            button.Frame = toolbar.Frame;
            toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(button) };
            textField.InputAccessoryView = toolbar;
        }

        public static void AddDoneButtonToNumericKeyboard(UITextField textField)
        {
            UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { textField.ResignFirstResponder(); });
            toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),doneButton };
            textField.InputAccessoryView = toolbar;
        }

        public static void SetupTextFieldAppearance(UITextField textField, UIColor textColor, nfloat textSize, string hint, UIColor hintColor, UIColor cursorColor, UIColor backgroundColor,
                                                    UIFontWeight fontWeight = UIFontWeight.Regular, UIReturnKeyType returnKeyType = UIReturnKeyType.Default, UIView view = null, int tag = 0)
        {
            textField.TextColor = textColor;
            textField.Font = UIFont.SystemFontOfSize(textSize, fontWeight);
            textField.AttributedPlaceholder = new NSAttributedString(hint, new UIStringAttributes(){ ForegroundColor = hintColor } );
            textField.TintColor = cursorColor;
            textField.BackgroundColor = backgroundColor;
            textField.ReturnKeyType = returnKeyType;

            if(view != null)
            {
                textField.Tag = tag;
                textField.ShouldReturn -= (field) => TextFieldShouldReturn(field, view);
                textField.ShouldReturn += (field) => TextFieldShouldReturn(field, view);
            }
        }
    }
}
