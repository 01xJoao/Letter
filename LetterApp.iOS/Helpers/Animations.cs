using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace LetterApp.iOS.Helpers
{
    public static class Animations
    {
        public static void Fade(this UIView view, bool isIn, double duration = 0.3, Action onFinished = null)
        {
            var minAlpha = (nfloat)0.0f;
            var maxAlpha = (nfloat)1.0f;

            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Transform = CGAffineTransform.MakeIdentity();
            UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                },
                onFinished
            );
        }

        public static void FlipHorizontaly(this UIView view, bool isIn, double duration = 0.3, Action onFinished = null)
        {
            var m34 = (nfloat)(-1 * 0.001);

            var minAlpha = (nfloat)0.0f;
            var maxAlpha = (nfloat)1.0f;

            view.Alpha = (nfloat)1.0;

            var minTransform = CATransform3D.Identity;
            minTransform.m34 = m34;
            minTransform = minTransform.Rotate((nfloat)((isIn ? 1 : -1) * Math.PI * 0.5), (nfloat)0.0f, (nfloat)1.0f, (nfloat)0.0f);
            var maxTransform = CATransform3D.Identity;
            maxTransform.m34 = m34;

            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Layer.Transform = isIn ? minTransform : maxTransform;
            UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    view.Layer.AnchorPoint = new CGPoint((nfloat)0.5, (nfloat)0.5f);
                    view.Layer.Transform = isIn ? maxTransform : minTransform;
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                },
                onFinished
            );
        }

        public static void FlipVerticaly(this UIView view, bool isIn, double duration = 0.3, Action onFinished = null)
        {
            var m34 = (nfloat)(-1 * 0.001);

            var minAlpha = (nfloat)0.0f;
            var maxAlpha = (nfloat)1.0f;

            var minTransform = CATransform3D.Identity;
            minTransform.m34 = m34;
            minTransform = minTransform.Rotate((nfloat)((isIn ? 1 : -1) * Math.PI * 0.5), (nfloat)1.0f, (nfloat)0.0f, (nfloat)0.0f);
            var maxTransform = CATransform3D.Identity;
            maxTransform.m34 = m34;

            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Layer.Transform = isIn ? minTransform : maxTransform;
            UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    view.Layer.AnchorPoint = new CGPoint((nfloat)0.5, (nfloat)0.5f);
                    view.Layer.Transform = isIn ? maxTransform : minTransform;
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                },
                onFinished
            );
        }

        public static void Rotate(this UIView view, bool isIn, bool fromLeft = true, double duration = 0.3, Action onFinished = null)
        {
            var minAlpha = (nfloat)0.0f;
            var maxAlpha = (nfloat)1.0f;
            var minTransform = CGAffineTransform.MakeRotation((nfloat)((fromLeft ? -1 : 1) * 720));
            var maxTransform = CGAffineTransform.MakeRotation((nfloat)0.0);

            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Transform = isIn ? minTransform : maxTransform;
            UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                    view.Transform = isIn ? maxTransform : minTransform;
                },
                onFinished
            );
        }

        public static void Scale(this UIView view, bool isIn, double duration = 0.3, Action onFinished = null)
        {
            var minAlpha = (nfloat)0.0f;
            var maxAlpha = (nfloat)1.0f;
            var minTransform = CGAffineTransform.MakeScale((nfloat)0.1, (nfloat)0.1);
            var maxTransform = CGAffineTransform.MakeScale((nfloat)1, (nfloat)1);

            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Transform = isIn ? minTransform : maxTransform;
            UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                    view.Transform = isIn ? maxTransform : minTransform;
                },
                onFinished
            );
        }

        public static void SlideHorizontaly(this UIView view, bool isIn, bool fromLeft, double duration = 0.3, Action onFinished = null)
        {
            var minAlpha = (nfloat)0.0f;
            var maxAlpha = (nfloat)1.0f;
            var minTransform = CGAffineTransform.MakeTranslation((fromLeft ? -1 : 1) * view.Bounds.Width, 0);
            var maxTransform = CGAffineTransform.MakeIdentity();

            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Transform = isIn ? minTransform : maxTransform;
            UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                    view.Transform = isIn ? maxTransform : minTransform;
                },
                onFinished
            );
        }

        public static void SlideVerticaly(this UIView view, bool isIn, bool fromTop, double duration = 0.3, Action onFinished = null, float delay = 0)
        {
            var minAlpha = (nfloat)0.0f;
            var maxAlpha = (nfloat)1.0f;
            var minTransform = CGAffineTransform.MakeTranslation(0, (fromTop ? -1 : 1) * view.Bounds.Height);
            var maxTransform = CGAffineTransform.MakeIdentity();

            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Transform = isIn ? minTransform : maxTransform;
            UIView.Animate(duration, delay, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                    view.Transform = isIn ? maxTransform : minTransform;
                },
                onFinished
            );
        }

        public static void Zoom(this UIView view, bool isIn, double duration = 0.3, Action onFinished = null)
        {
            var minAlpha = (nfloat)0.0f;
            var maxAlpha = (nfloat)1.0f;
            var minTransform = CGAffineTransform.MakeScale((nfloat)2.0, (nfloat)2.0);
            var maxTransform = CGAffineTransform.MakeScale((nfloat)1, (nfloat)1);

            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Transform = isIn ? minTransform : maxTransform;
            UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                    view.Transform = isIn ? maxTransform : minTransform;
                },
                onFinished
            );
        }

        public static void AnimateBackground(UIView bgView, nfloat value)
        {
            UIView.Animate(0.3f, 0, UIViewAnimationOptions.BeginFromCurrentState, () =>
            {
                bgView.Frame = new CGRect(bgView.Frame.X, bgView.Frame.Y - value, bgView.Frame.Width, bgView.Frame.Height);
            }, null);
        }

        public static void BackgroundToDefault(UIView bgView, CGRect defaultPosition)
        {
            UIView.Animate(0.3f, 0, UIViewAnimationOptions.BeginFromCurrentState, () =>
            {
                bgView.Frame = defaultPosition;
            }, null);
        }
    }
}
