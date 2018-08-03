using System;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class CallViewModel : XViewModel<int>
    {
        private int _userId;

        public CallViewModel()
        {
        }

        protected override void Prepare(int userId)
        {
            _userId = userId;
        }
    }
}
