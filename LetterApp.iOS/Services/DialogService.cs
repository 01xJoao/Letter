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
        //MTMBProgressHUD progressDialog;
        LOTAnimationView _lottieAnimation;

        public void ShowAlert(string title, AlertType alertType)
        {
            var alertVC = AlertView.Create();
            alertVC.Configure(title, alertType);
        }

        public Task<string> ShowInput(string title = "", string confirmButtonText = null, string cancelButtonText = null, string hint = "", InputType inputType = InputType.Number)
        {
            throw new NotImplementedException();
        }

        public Task<string> ShowOptions(string title = "", OptionsType optionsType = OptionsType.List, string cancelButtonText = "", params string[] options)
        {
            throw new NotImplementedException();
        }

        public void ShowLoading()
        {
            HideLoading();

            _lottieAnimation = LOTAnimationView.AnimationNamed("loading");
            _lottieAnimation.ContentMode = UIViewContentMode.ScaleAspectFit;

            var view = ((AppDelegate)UIApplication.SharedApplication.Delegate).Window;
            _lottieAnimation.Frame = new CGRect(0, 0, view.Bounds.Size.Width, view.Bounds.Size.Height);

            view.AddSubview(_lottieAnimation);
            _lottieAnimation.Play();
        }

        public void HideLoading()
        {
            _lottieAnimation?.Pause();
            _lottieAnimation?.RemoveFromSuperview();
            _lottieAnimation = null;
        }
    }
}
