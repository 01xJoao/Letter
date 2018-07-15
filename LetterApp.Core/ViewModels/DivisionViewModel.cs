using System;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class DivisionViewModel : XViewModel<int>
    {
        public DivisionViewModel()
        {
        }

        protected override void Prepare(int divisionId)
        {
            throw new NotImplementedException();
        }
    }
}
