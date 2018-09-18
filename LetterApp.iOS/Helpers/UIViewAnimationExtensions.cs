﻿using System;
using System.Linq;
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

        public static void CustomViewLoadingAnimation(string animation, UIView mainView, UIView view, bool shouldAnimate = false)
        {
            if (shouldAnimate)
            {
                var center = UIScreen.MainScreen.Bounds.Width / 2;
                _lottieAnimation = LOTAnimationView.AnimationNamed(animation);
                _lottieAnimation.Frame = view.Frame;
                mainView.AddSubview(_lottieAnimation);
                _lottieAnimation.LoopAnimation = true;
                _lottieAnimation.ContentMode = UIViewContentMode.ScaleAspectFit;
                _lottieAnimation.Hidden = false;
                _lottieAnimation.AnimationProgress = 0;
                _lottieAnimation.Center = new CGPoint(center, mainView.Center.Y);
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

        public static void LottieMadeSimple(string animationName, UIView view, bool shouldAnimate)
        {
            if (shouldAnimate)
            {
                view.Hidden = false;
                _lottieAnimation = LOTAnimationView.AnimationNamed(animationName);
                _lottieAnimation.Frame = new CGRect(-view.Frame.Width, -view.Frame.Height, 100, 100);
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
