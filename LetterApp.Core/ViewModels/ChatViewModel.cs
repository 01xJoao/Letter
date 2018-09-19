using System;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class ChatViewModel : XViewModel<int>
    {
        private int _userId;
        private readonly IMessengerService _messengerService;
        private readonly IDialogService _dialogService;

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
    }
}
