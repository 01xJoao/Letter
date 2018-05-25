﻿using System;
using System.Threading.Tasks;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;
using SharpRaven.Data;

namespace LetterApp.Core.ViewModels
{
    public class LoginViewModel : XViewModel
    {
        private IAuthenticationService _authService;

        private XPCommand<Tuple<string,string>> _signInCommand;
        public XPCommand<Tuple<string, string>> SignInCommand => _signInCommand ?? (_signInCommand = new XPCommand<Tuple<string, string>>(async (value) => await SignIn(value), CanLogin));

        public LoginViewModel(IAuthenticationService authService)
        {
            _authService = authService;
        }

        private async Task SignIn(Tuple<string,string> value)
        {
            IsBusy = true;
            try
            {
                var userRequest = new UserRequestModel(value.Item1, value.Item2);
                var currentUser = await _authService.LoginAsync(userRequest);

                if(currentUser.Code == null)
                    Realm.Write(() => Realm.Add(currentUser, true));
                else
                {
                    //Wrong Email or Password
                }
            }
            catch (Exception ex)
            {
                RavenService.Raven.Capture(new SentryEvent(ex));
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanLogin(Tuple<string, string> value) => !IsBusy && !string.IsNullOrEmpty(value.Item1) && !string.IsNullOrEmpty(value.Item2);

        #region Resources

        public string EmailLabel => L10N.Localize("LoginViewModel_EmailLabel");
        public string PasswordLabel => L10N.Localize("LoginViewModel_PasswordLabel");
        public string ForgotPasswordButton => L10N.Localize("LoginViewModel_ForgotPassButton");
        public string SignUpButton => L10N.Localize("LoginViewModel_SignUpButton");
        public string SignInButton => L10N.Localize("LoginViewModel_SignInButton");

        #endregion
    }
}