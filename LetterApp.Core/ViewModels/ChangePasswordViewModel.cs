using System;
using System.Threading.Tasks;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class ChangePasswordViewModel : XViewModel
    {
        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        public ChangePasswordViewModel()
        {
        }

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }

        private bool CanExecute() => !IsBusy;
    }
}
