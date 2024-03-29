﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Airbnb.Lottie;
using CoreGraphics;
using LetterApp.Core.Models;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Views.CustomViews.Dialog;
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

        public Task<Tuple<ChatOptions, bool>> ShowChatOptions(string name = "", string photo = "", bool muted = false, string[] resources = null)
        {
            var tcs = new TaskCompletionSource<Tuple<ChatOptions, bool>>();

            var chatOptionsView = new ShowChatOptionsViewController(name, photo, muted, resources, val => tcs.TrySetResult(val));
            chatOptionsView.Show();

            return tcs.Task;
        }

        public Task<bool> ShowMessageAlert(string photo = "", string name = "", string message = "")
        {
            var tcs = new TaskCompletionSource<bool>();

            var messageAlertView = new ChatAlertViewController(photo, name, message, val => tcs.TrySetResult(val));
            messageAlertView.Show();

            return tcs.Task;
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

        public Task<Tuple<bool,bool>> ShowFilter(string title = "", List<ContactDialogFilter> filters = null, string buttonText = "")
        {
            var tcs = new TaskCompletionSource<Tuple<bool, bool>>();

            var filterView = new FilterContactsViewController(title, filters, buttonText, val => tcs.TrySetResult(val));
            filterView.Show();

            return tcs.Task;
        }

        public Task<CallingType> ShowContactOptions(Dictionary<string, string> locationResources, bool showPhoneOption = true)
        {
            var tcs = new TaskCompletionSource<CallingType>();

            var contacView = new ContactViewController(locationResources, showPhoneOption, val => tcs.TrySetResult(val));
            contacView.Show();

            return tcs.Task;
        }

        public Task<bool> ShowPicture(string image, string send, string cancel)
        {
            var tcs = new TaskCompletionSource<bool>();

            var pictureView = new PictureViewController(image, send, cancel, val => tcs.TrySetResult(val));
            pictureView.Show();

            return tcs.Task;
        }

        public Task<bool> ShowChatImage(string image, string save)
        {
            var tcs = new TaskCompletionSource<bool>();

            var imageView = new ShowImageViewController(image, save, val => tcs.TrySetResult(val));
            imageView.Show();

            return tcs.Task;
        }

        public void ShowCallStack(string title, List<CallStackModel> calls)
        {
            var callStackView = new CallStackViewController(title, calls);
            callStackView.Show();
        }

        public void StartLoading(LoadingColor color)
        {
            StopLoading();

            _lottieAnimation = color == LoadingColor.Blue ? LOTAnimationView.AnimationNamed("load_blue") : LOTAnimationView.AnimationNamed("load_white");
            _lottieAnimation.ContentMode = UIViewContentMode.ScaleAspectFit;

            if(_view == null)
                _view = ((AppDelegate)UIApplication.SharedApplication.Delegate).Window;

            _lottieAnimation.Frame = new CGRect(0, 0, 90, 90);
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
