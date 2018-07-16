using System;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class DivisionViewModel : XViewModel<int>
    {
        private int _divisionId;

        public DivisionViewModel() {}

        protected override void Prepare(int divisionId)
        {
            _divisionId = divisionId;
        }
    }
}
