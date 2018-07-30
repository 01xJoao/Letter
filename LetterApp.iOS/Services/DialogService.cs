﻿using System;
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
        private LOTAnimationView _lottieAnimation;
        private UIView _view;

        public void ShowAlert(string title, AlertType alertType, float duration)
        {
            var alertVC = AlertView.Create();
            alertVC.Configure(title, alertType, duration);
        }

        public Task<string> ShowTextInput(string title = "", string subtitle = "", string inputContent = "", string confirmButtonText = "", string hint = "", InputType inputType = InputType.Text, QuestionType questionType = QuestionType.Normal)
        {
            var tcs = new TaskCompletionSource<string>();

            var inputTextView = new InputTextViewController(title, subtitle, inputContent, confirmButtonText, hint, inputType, questionType, val => tcs.TrySetResult(val), () => tcs.TrySetResult(null));
            inputTextView.Show();

            return tcs.Task;
        }

        public Task<string> ShowOptions(string title = "", OptionsType optionsType = OptionsType.List, string cancelButtonText = "", params string[] options)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowInformation(string title = "", string text1 = "", string text2 = "", string text3 = "", string confirmButtonText = "")
        {
            var tcs = new TaskCompletionSource<bool>();

            var informationView = new InformationViewController(title, text1, text2, text3, confirmButtonText, val => tcs.TrySetResult(val));
            informationView.Show();

            return tcs.Task;
        }

        public Task<bool> ShowQuestion(string title = "", string buttonText = "", QuestionType questionType = QuestionType.Normal)
        {
            var tcs = new TaskCompletionSource<bool>();

            var questionView = new QuestionViewController(title, buttonText, val => tcs.TrySetResult(val), questionType);
            questionView.Show();

            return tcs.Task;
        }

        public Task<bool> ShowFilter(string title = "", string switchText = "", string descriptionText = "", string buttonText = "", bool isActive = true)
        {
            var tcs = new TaskCompletionSource<bool>();

            var filterView = new FilterContactsViewController(title, switchText, descriptionText, buttonText, isActive ,val => tcs.TrySetResult(val));
            filterView.Show();

            return tcs.Task;
        }

        public void StartLoading()
        {
            StopLoading();

            _lottieAnimation = LOTAnimationView.AnimationNamed("loading");
            _lottieAnimation.ContentMode = UIViewContentMode.ScaleAspectFit;

            if(_view == null)
                _view = ((AppDelegate)UIApplication.SharedApplication.Delegate).Window;

            _lottieAnimation.Frame = new CGRect(0, 0, _view.Bounds.Size.Width * 0.5,  _view.Bounds.Size.Height * 0.5);
            _lottieAnimation.Center = _view.Center;

            _view.AddSubview(_lottieAnimation);
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
