using System;
using Airbnb.Lottie;
using CoreGraphics;
using UIKit;

namespace LetterApp.iOS.Helpers
{
    public static class UIViewAnimationExtensions
    {
        private static LOTAnimationView _lottieAnimation;

        public static void CustomButtomLoadingAnimation(string animation, UIButton button, string viewText, bool shouldAnimate)
        {
            if (shouldAnimate)
            {
                _lottieAnimation = LOTAnimationView.AnimationNamed(animation);
                _lottieAnimation.ContentMode = UIViewContentMode.ScaleAspectFit;
                _lottieAnimation.Frame = button.Frame;
                button.AddSubview(_lottieAnimation);
                _lottieAnimation.LoopAnimation = true;

                button.SetTitle("", UIControlState.Normal);
                _lottieAnimation.AnimationProgress = 0;
                _lottieAnimation.Hidden = false;
                _lottieAnimation.Play();
            }
            else
            {
                if (_lottieAnimation != null)
                {
                    _lottieAnimation.Hidden = true;
                    _lottieAnimation.Pause();
                    _lottieAnimation?.Dispose();
                    _lottieAnimation = null;
                }

                button.SetTitle(viewText, UIControlState.Normal);
            }
        }

        public static void LoadingInView(string animationName, UIView view, bool shouldAnimate)
        {
            if (shouldAnimate)
            {
                view.Hidden = false;
                _lottieAnimation = LOTAnimationView.AnimationNamed(animationName);
                _lottieAnimation.Frame = new CGRect(0, 0, 50, 50);
                view.AddSubview(_lottieAnimation);
                _lottieAnimation.LoopAnimation = true;
                _lottieAnimation.Hidden = false;
                _lottieAnimation.AnimationProgress = 0;
                _lottieAnimation.Play();
            }
            else
            {
                view.Hidden = true;

                if (_lottieAnimation != null)
                {
                    _lottieAnimation.Hidden = true;
                    _lottieAnimation.Pause();
                    _lottieAnimation?.Dispose();
                    _lottieAnimation = null;
                }
            }
        }

        public static void LoadingInChat(UIView view, bool shouldAnimate)
        {
            if (shouldAnimate)
            {
                _lottieAnimation = LOTAnimationView.AnimationNamed("progress_refresh");
                _lottieAnimation.Frame = new CGRect((UIScreen.MainScreen.Bounds.Width - view.Frame.Width / 2f) / 2,
                                                    view.Frame.Height - 4, view.Frame.Width / 2f, 3);

                _lottieAnimation.Layer.CornerRadius = 2f;
                view.AddSubview(_lottieAnimation);
                _lottieAnimation.LoopAnimation = true;
                _lottieAnimation.ContentMode = UIViewContentMode.Redraw;
                _lottieAnimation.Hidden = false;
                _lottieAnimation.AnimationProgress = 0;
                _lottieAnimation.Play();
            }
            else
            {
                if (_lottieAnimation != null)
                {
                    _lottieAnimation.Hidden = true;
                    _lottieAnimation.Pause();
                    _lottieAnimation?.Dispose();
                    _lottieAnimation = null;
                }
            }
        }

        public static void AnimateBackgroundView(UIView view, nfloat animationHeight, bool shouldAnimate)
        {
            if (shouldAnimate)
                Animations.AnimateBackground(view, animationHeight);
            else
                Animations.BackgroundToDefault(view, UIScreen.MainScreen.Bounds);
        }

        public static void AnimateView(UIView view, nfloat animationHeight, bool shouldAnimate)
        {
            if (shouldAnimate)
                Animations.AnimateBackground(view, animationHeight);
            else
                Animations.AnimateBackground(view, -animationHeight);
        }
    }
}
