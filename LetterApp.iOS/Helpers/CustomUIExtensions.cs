using System;
using CoreGraphics;
using UIKit;

namespace LetterApp.iOS.Helpers
{
    public static class CustomUIExtensions
    {
        public static NSShadow TextShadow()
        {
            NSShadow textShadow = new NSShadow();
            textShadow.ShadowColor = Colors.Black;
            textShadow.ShadowBlurRadius = 1.2f;
            textShadow.ShadowOffset = new CGSize(0.6f, 0.6f);
            return textShadow;
        }

        public static void LabelShadow(UILabel label)
        {
            label.ShadowColor = Colors.Black;
            label.ShadowOffset = new CGSize(0.3f, 0.3f);
        }

        public static void ViewShadow(UIView view)
        {
            view.ClipsToBounds = false;
            view.Layer.ShadowColor = Colors.Black.CGColor;
            view.Layer.ShadowOpacity = 2f;
            view.Layer.ShadowOffset = new CGSize(0.5f, 0.5f);
            view.Layer.ShadowRadius = 2f;
        }

        public static void ImageShadow(UIImageView view)
        {
            view.ClipsToBounds = false;
            view.Layer.ShadowColor = Colors.Black.CGColor;
            view.Layer.ShadowOpacity = 6f;
            view.Layer.ShadowOffset = new CGSize(0, 1f);
            view.Layer.ShadowRadius = 3f;
        }

        public static void RoundShadow(UIView view)
        {
            view.Layer.CornerRadius = 2;
            view.Layer.MasksToBounds = true;
            view.Layer.ShadowRadius = 5;
            view.Layer.ShadowOpacity = 0.3f;
            view.ClipsToBounds = false;
            view.Layer.ShadowOffset = new CGSize(0.0f, 0.0f);
        }

        public static void SelectButton(UIView button, UIColor color)
        {
            button.Layer.CornerRadius = 3;
            button.ClipsToBounds = true;
            button.Layer.BorderWidth = 1;
            button.Layer.BorderColor = color.CGColor;
        }

        public static void RoundView(UIView view)
        {
            view.Layer.CornerRadius = view.Frame.Height/2;
            view.Layer.MasksToBounds = true;
        }
    }
}
