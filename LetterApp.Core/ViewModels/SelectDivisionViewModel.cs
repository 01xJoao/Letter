using System;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class SelectDivisionViewModel : XViewModel<int>
    {
        public SelectDivisionViewModel()
        {
        }

        protected override void Prepare(int organizationId)
        {
            throw new NotImplementedException();
        }
    }
}
