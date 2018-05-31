using System;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class ActivateAccountViewModel : XViewModel<string>
    {
        private string _email;

        public ActivateAccountViewModel() {}

        protected override void Prepare(string email) => _email = email;

    }
}
