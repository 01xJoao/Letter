using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class CallListViewModel : XViewModel
    {
        private readonly IDialogService _dialogService;

        private List<CallModel> _calls;
        public List<CallModel> Calls
        {
            get => _calls;
            set => SetProperty(ref _calls, value);
        }

        public CallListViewModel(IDialogService dialogService) 
        {
            _dialogService = dialogService;
            SetL10NResources();
        }

        public override async Task Appearing()
        {
            Calls = Realm.All<CallModel>().ToList();
        }

        #region Resources

        public string Title => L10N.Localize("MainViewModel_CallTab");

        private Dictionary<string, string> LocationResources = new Dictionary<string, string>();
        private string TitleDialog => L10N.Localize("ContactDialog_Title");
        private string LetterDialog => L10N.Localize("ContactDialog_TitleLetter");
        private string LetterDescriptionDialog => L10N.Localize("ContactDialog_DescriptionLetter");
        private string PhoneDialog => L10N.Localize("ContactDialog_TitlePhone");
        private string PhoneDescriptionDialog => L10N.Localize("ContactDialog_DescriptionPhone");

        private void SetL10NResources()
        {
            LocationResources.Add("Title", TitleDialog);
            LocationResources.Add("TitleLetter", LetterDialog);
            LocationResources.Add("DescriptionLetter", LetterDescriptionDialog);
            LocationResources.Add("TitlePhone", PhoneDialog);
            LocationResources.Add("DescriptionPhone", PhoneDescriptionDialog);
        }
        #endregion

    }
}
