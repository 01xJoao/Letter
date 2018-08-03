using System;
using LetterApp.Core.Localization;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class CallListViewModel : XViewModel
    {
        public CallListViewModel() {}
    
    
    

        #region Resources

        public string Title => L10N.Localize("MainViewModel_CallTab");

        #endregion
   
    }
}
