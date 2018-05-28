using System;
using System.Threading.Tasks;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class RecoverPasswordViewModel : XViewModel
    {
        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView()));

        public RecoverPasswordViewModel() {}

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }

        #region Resources

        public string ChangePassTitle => L10N.Localize("RecoverPasswordViewModel_ChangePassTitle");

        #endregion
    }
}
