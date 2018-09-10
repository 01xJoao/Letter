using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class ChatListViewModel : XViewModel
    {
        private readonly IMessengerService _messagerService;

        public ChatListViewModel(IMessengerService messagerService) 
        {
            _messagerService = messagerService;

            //Tests
            CreateChannel();
        }

        private async Task CreateChannel()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            try
            {
                var channel = await _messagerService.CreateChannel(new List<string> { "85-33" });

                if(channel != null)
                    await _messagerService.SendMessage(channel, "Hello World!");

            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }
    }
}
