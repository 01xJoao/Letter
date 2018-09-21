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
            view.Layer.ShadowColor = Colors.Black.ColorWithAlpha(0.4f).CGColor;
            view.Layer.ShadowOpacity = 4f;
            view.Layer.ShadowOffset = new CGSize(0.5f, 0.5f);
            view.Layer.ShadowRadius = 6f;
        }

        public static void ViewShadowForChatPupUp(UIView view)
        {
            view.ClipsToBounds = false;
            view.Layer.ShadowColor = Colors.Black.ColorWithAlpha(0.2f).CGColor;
            view.Layer.ShadowOpacity = 2f;
            view.Layer.ShadowOffset = new CGSize(0.5f, 0.5f);
            view.Layer.ShadowRadius = 2f;
            view.Layer.CornerRadius = 4f;
        }

        public static void ImageShadow(UIView view)
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

        public static void CornerView(UIView view, int cornerSize)
        {
            view.Layer.CornerRadius = cornerSize;
            view.ClipsToBounds = true;
        }


        public static void BorderView(UIView view)
        {
            view.Layer.CornerRadius = view.Frame.Height / 2;
            view.Layer.BorderColor = Colors.MainBlue.CGColor;
            view.Layer.BorderWidth = 1f;
        }

        public static UIView SetupNavigationBarWithSubtitle(string title, string subtitle, nfloat maxSize)
        {
            var titleLabel = new UILabel(new CGRect(0, 2, 0, 0))
            {
                BackgroundColor = UIColor.Clear,
                TextColor = Colors.White,
                Font = UIFont.BoldSystemFontOfSize(17),
                Text = title,
                LineBreakMode = UILineBreakMode.MiddleTruncation
            };

            titleLabel.SizeToFit();

            var subtitleLabel = new UILabel(new CGRect(0, 26, 0, 0))
            {
                BackgroundColor = UIColor.Clear,
                TextColor = Colors.White,
                Font = UIFont.SystemFontOfSize(12),
                Text = subtitle,
                LineBreakMode = UILineBreakMode.MiddleTruncation
            };

            subtitleLabel.SizeToFit();

            if(titleLabel.Frame.Width > maxSize)
                titleLabel.Frame = new CGRect(titleLabel.Frame.X, titleLabel.Frame.Y, maxSize, titleLabel.Frame.Height);

            if (subtitleLabel.Frame.Width > maxSize)
                subtitleLabel.Frame = new CGRect(subtitleLabel.Frame.X, subtitleLabel.Frame.Y, maxSize, subtitleLabel.Frame.Height);

            var titleView = new UIView(new CGRect(0, 0, Math.Max(titleLabel.Frame.Size.Width, subtitleLabel.Frame.Size.Width), 30));

            titleView.AddSubview(titleLabel);
            titleView.AddSubview(subtitleLabel);

            var widthDiff = subtitleLabel.Frame.Size.Width - titleLabel.Frame.Size.Width;

            if(widthDiff < 0)
            {
                var newX = widthDiff / 2;
                subtitleLabel.Frame = new CGRect(Math.Abs(newX), subtitleLabel.Frame.Y, subtitleLabel.Frame.Width, subtitleLabel.Frame.Height);
            }
            else
            {
                var newX = widthDiff / 2;
                titleLabel.Frame = new CGRect(Math.Abs(newX), titleLabel.Frame.Y, titleLabel.Frame.Width, titleLabel.Frame.Height);
            }

            return titleView;
        }
    }
}
