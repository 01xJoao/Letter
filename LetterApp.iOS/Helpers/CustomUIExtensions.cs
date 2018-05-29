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

        public static void ViewShadow(UIView view)
        {
            view.ClipsToBounds = false;
            view.Layer.ShadowColor = Colors.Black.CGColor;
            view.Layer.ShadowOpacity = 2f;
            view.Layer.ShadowOffset = new CGSize(0.5f, 0.5f);
            view.Layer.ShadowRadius = 2f;
        }
    }
}
