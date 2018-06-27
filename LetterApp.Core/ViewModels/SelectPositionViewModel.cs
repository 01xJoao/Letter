using System;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class SelectPositionViewModel : XViewModel<int>
    {

        //IMPORTANT 
        //At the end add: AppSettings.IsUserLogged = true;

        public SelectPositionViewModel()
        {
        }

        protected override void Prepare(int organizationId)
        {
            throw new NotImplementedException();
        }
    }
}
