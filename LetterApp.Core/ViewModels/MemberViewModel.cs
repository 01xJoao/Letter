using System;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels
{
    public class MemberViewModel : XViewModel<int>
    {
        private IDialogService _dialogService;
        private IMemberService _memberService;

        private int _userId;

        public MemberViewModel(IDialogService dialogService, IMemberService memberService)
        {
            _dialogService = dialogService;
            _memberService = memberService;
        }

        protected override void Prepare(int userId)
        {
            _userId = userId;
        }

        public override async Task InitializeAsync()
        {

        }

        public override async Task Appearing()
        {

        }


        #region Resources



        #endregion
    }
}
