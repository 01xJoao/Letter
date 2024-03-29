﻿using System;
using Airbnb.Lottie;
using CoreGraphics;
using UIKit;

namespace LetterApp.iOS.Helpers
{
    public static class UIViewAnimationExtensions
    {
        private static int _mainViewSize => (int)UIScreen.MainScreen.Bounds.Width;
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
                _lottieAnimation.Frame = new CGRect(0, 0, view.Frame.Width, view.Frame.Height);
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
                var size = _mainViewSize / 4 + 8;
                _lottieAnimation.Frame = new CGRect(size, view.Frame.Height - 2.7f, _mainViewSize - size * 2, 2.5f);
                _lottieAnimation.Layer.CornerRadius = 0.8f;
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
