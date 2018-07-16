using System;
using System.Threading.Tasks;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class UserSettingsViewModel : XViewModel
    {
        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public UserSettingsViewModel()
        {
        }

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }


        private bool CanExecute() => !IsBusy;

        #region Resources

        public string SettingsTitle => L10N.Localize("UserSettings_SettingsTitle");

        #endregion
    }
}
