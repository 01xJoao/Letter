using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LetterApp.iOS.Helpers
{
    public static class StringExtensions
    {
        public static nfloat StringHeight(this string text, UIFont font, nfloat width)
        {
            return text.StringRect(font, width).Height;
        }

        public static CGRect StringRect(this string text, UIFont font, nfloat width)
        {
            var nativeString = new NSString(text);

            return nativeString.GetBoundingRect(
                new CGSize(width, float.MaxValue),
                NSStringDrawingOptions.UsesLineFragmentOrigin,
                new UIStringAttributes { Font = font },
                null);
        }

        public static NSAttributedString GetHTMLFormattedText(string toBeFormatted, string fontFace = "HelveticaNeue", float fontSize = 5, string fontColor = "#757575", bool center = false)
        {
            NSError error = null;
            var formattedText = center ? $"<font face ='{fontFace}' size ={fontSize} color='{fontColor}'<center>{toBeFormatted}</center></font>"
                                       : $"<font face ='{fontFace}' size ={fontSize} color='{fontColor}'<justify>{toBeFormatted}</justify></font>";
            var attributes = new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML, StringEncoding = NSStringEncoding.UTF8 };
            return new NSAttributedString(formattedText, attributes, ref error);
        }
    }
}
