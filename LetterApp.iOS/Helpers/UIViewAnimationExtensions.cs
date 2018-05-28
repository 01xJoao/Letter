using System;
using System.Linq;
using Airbnb.Lottie;
using CoreGraphics;
using UIKit;

namespace LetterApp.iOS.Helpers
{
    public static class UIViewAnimationExtensions
    {
        public static LOTAnimationView CustomButtomLoadingAnimation(LOTAnimationView lottieAnimation, string animationName, UIButton button, string viewText, bool shouldAnimate)
        {
            if (shouldAnimate)
            {
                if (lottieAnimation == null)
                {
                    lottieAnimation = LOTAnimationView.AnimationNamed(animationName);
                    lottieAnimation.ContentMode = UIViewContentMode.ScaleAspectFit;
                    lottieAnimation.Frame = button.Frame;
                    button.AddSubview(lottieAnimation);
                    lottieAnimation.LoopAnimation = true;
                }

                button.SetTitle("", UIControlState.Normal);
                lottieAnimation.AnimationProgress = 0;
                lottieAnimation.Hidden = false;
                lottieAnimation.Play();
            }
            else
            {
                if (lottieAnimation != null)
                {
                    lottieAnimation.Hidden = true;
                    lottieAnimation.Pause();
                }

                button.SetTitle(viewText, UIControlState.Normal);
            }

            return lottieAnimation;
        }

        public static void AnimateBackgroundView(UIView view, nfloat animationHeight, bool shouldAnimate)
        {
            if (shouldAnimate)
                Animations.AnimateBackground(view, animationHeight);
            else
                Animations.BackgroundToDefault(view, UIScreen.MainScreen.Bounds);
        }
    }
}
