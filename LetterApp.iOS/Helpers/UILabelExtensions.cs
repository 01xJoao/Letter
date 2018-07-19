using System;
using UIKit;

namespace LetterApp.iOS.Helpers
{
    public static class UILabelExtensions
    {
        public static void SetupLabelAppearance(UILabel label, string text, UIColor color, nfloat textSize, UIFontWeight fontWeight = UIFontWeight.Regular, bool italic = false)
        {
            if (string.IsNullOrEmpty(text))
                text = "";
            
            label.Text = text;
            label.TextColor = color;

            if (italic)
                label.Font = UIFont.ItalicSystemFontOfSize(textSize);
            else
                label.Font = UIFont.SystemFontOfSize(textSize, fontWeight);
        }
    }
}
