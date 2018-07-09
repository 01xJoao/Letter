using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LetterApp.iOS.Helpers
{
    public static class UIButtonExtensions
    {
        public static UIBarButtonItem SetupImageBarButton(nfloat size, string imageName, EventHandler onTouchEvent)
        {
            var button = new UIButton(new CGRect(0, 0, size, size)) { ContentMode = UIViewContentMode.ScaleAspectFit };
            button.AddConstraint(NSLayoutConstraint.Create(button, NSLayoutAttribute.Width, NSLayoutRelation.Equal, 1, size));
            button.AddConstraint(NSLayoutConstraint.Create(button, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, size));
            button.SetImage(UIImage.FromBundle(imageName), UIControlState.Normal);
            button.SetTitle(string.Empty, UIControlState.Normal);

            button.TouchUpInside -= onTouchEvent;
            button.TouchUpInside += onTouchEvent;

            return new UIBarButtonItem(button);
        }

        public static UIBarButtonItem SetupTextBarButton(nfloat size, string textButton, EventHandler onTouchEvent, UIColor color = null)
        {
            var button = new UIButton(new CGRect(0, 0, size, size));
            button.SetTitle(textButton, UIControlState.Normal);
            button.SetTitleColor(color ?? Colors.Black, UIControlState.Normal);

            button.TouchUpInside -= onTouchEvent;
            button.TouchUpInside += onTouchEvent;

            return new UIBarButtonItem(button);
        }

        public static void SetupButtonAppearance(UIButton button, UIColor color, nfloat textSize, string title, UIFontWeight fontWeight = UIFontWeight.Regular)
        {
            button.SetTitle(title, UIControlState.Normal);
            button.SetTitleColor(color, UIControlState.Normal);
            button.TitleLabel.Font = UIFont.SystemFontOfSize(textSize, fontWeight);
        }

        public static void SetupButtonUnderlineAppearance(UIButton button, UIColor color, nfloat textSize, string title, UIFontWeight fontWeight = UIFontWeight.Regular)
        {
            var customString = new NSMutableAttributedString(title, new UIStringAttributes
            {
                Font = UIFont.SystemFontOfSize(textSize, fontWeight),
                ForegroundColor = color,
                UnderlineStyle = NSUnderlineStyle.Single,
            });

            button.SetAttributedTitle(customString, UIControlState.Normal);
        }
    }
}
