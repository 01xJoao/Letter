using System;
using System.Threading.Tasks;
using Airbnb.Lottie;
using CoreGraphics;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.Dialog;
using MBProgressHUD;
using UIKit;

namespace LetterApp.iOS.Services
{
    public class DialogService : IDialogService
    {
        LOTAnimationView _lottieAnimation;

        public void ShowAlert(string title, AlertType alertType, float duration)
        {
            var alertVC = AlertView.Create();
            alertVC.Configure(title, alertType, duration);
        }

        public Task<string> ShowTextInput(string title = "", string inputContent = "", string confirmButtonText = "", string hint = "", InputType inputType = InputType.Text)
        {
            var tcs = new TaskCompletionSource<string>();

            var inputTextView = new InputTextViewController(title, inputContent, confirmButtonText, hint, inputType, val => tcs.TrySetResult(val), () => tcs.TrySetResult(null));
            inputTextView.Show();

            return tcs.Task;
        }

        public Task<string> ShowOptions(string title = "", OptionsType optionsType = OptionsType.List, string cancelButtonText = "", params string[] options)
        {
            throw new NotImplementedException();
        }

        public void StartLoading()
        {
            StopLoading();

            _lottieAnimation = LOTAnimationView.AnimationNamed("loading");
            _lottieAnimation.ContentMode = UIViewContentMode.ScaleAspectFit;

            var view = ((AppDelegate)UIApplication.SharedApplication.Delegate).Window;
            _lottieAnimation.Frame = new CGRect(0, 0, view.Bounds.Size.Width * 0.5,  view.Bounds.Size.Height * 0.5);
            _lottieAnimation.Center = view.Center;

            view.AddSubview(_lottieAnimation);
            _lottieAnimation.LoopAnimation = true;
            _lottieAnimation.Play();
        }

        public void StopLoading()
        {
            _lottieAnimation?.Pause();
            _lottieAnimation?.RemoveFromSuperview();
            _lottieAnimation?.Dispose();
            _lottieAnimation = null;
        }
    }
}
