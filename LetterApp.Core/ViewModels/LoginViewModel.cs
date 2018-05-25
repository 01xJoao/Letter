using System;
using LetterApp.Core.Localization;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class LoginViewModel : XViewModel
    {
        public LoginViewModel(){}

        #region Resources

        public string EmailLabel => L10N.Localize("LoginViewModel_EmailLabel");
        public string PasswordLabel => L10N.Localize("LoginViewModel_PasswordLabel");
        public string ForgotPasswordButton => L10N.Localize("LoginViewModel_ForgotPassButton");
        public string SignUpButton => L10N.Localize("LoginViewModel_SignUpButton");
        public string SignInButton => L10N.Localize("LoginViewModel_SignInButton");

        #endregion
    }
}
