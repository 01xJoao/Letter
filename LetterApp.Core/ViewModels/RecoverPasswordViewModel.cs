using System;
using System.Threading.Tasks;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class RecoverPasswordViewModel : XViewModel<string>
    {
        public string Email { get; set; }

        private bool _isSubmiting;
        public bool IsSubmiting
        {
            get => _isSubmiting;
            set => SetProperty(ref _isSubmiting, value);
        }

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView()));

        public RecoverPasswordViewModel() {}

        protected override void Prepare(string email)
        {
            Email = email;
        }

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }

        #region Resources

        public string NewPassTitle          => L10N.Localize("RecoverPasswordViewModel_NewPassTitle");
        public string EmailAddressLabel     => L10N.Localize("RecoverPasswordViewModel_EmailAddress");
        public string ConfirmPassLabel      => L10N.Localize("RecoverPasswordViewModel_ConfirmPass");
        public string CodeLabel             => L10N.Localize("RecoverPasswordViewModel_Code");
        public string RequestAgainButton    => L10N.Localize("RecoverPasswordViewModel_RequestAgain");
        public string SubmitButton          => L10N.Localize("RecoverPasswordViewModel_Submit");
        public string ShowButton            => L10N.Localize("RecoverPasswordViewModel_ShowButton");
        public string HideButton            => L10N.Localize("RecoverPasswordViewModel_HideButton");

        #endregion
    }
}
