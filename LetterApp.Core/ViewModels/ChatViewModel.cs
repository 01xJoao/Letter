using System;
using System.Threading.Tasks;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class ChatViewModel : XViewModel<int>
    {
        private int _userId;
        private readonly IMessengerService _messengerService;
        private readonly IDialogService _dialogService;

        private XPCommand _closeViewCommand;
        public XPCommand CloseViewCommand => _closeViewCommand ?? (_closeViewCommand = new XPCommand(async () => await CloseView(), CanExecute));

        private bool CanExecute() => !IsBusy;

        public ChatViewModel(IMessengerService messengerService, IDialogService dialogService)
        {
            _messengerService = messengerService;
            _dialogService = dialogService;
        }

        protected override void Prepare(int userId)
        {
            _userId = userId;
        }

        public override Task Appearing()
        {
            return null;
        }

        private async Task CloseView()
        {
            await NavigationService.Close(this);
        }
    }
}
