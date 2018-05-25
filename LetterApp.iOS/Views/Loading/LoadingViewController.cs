using LetterApp.Core.ViewModels;
using LetterApp.iOS.Interfaces;
using LetterApp.iOS.Views.Base;

namespace LetterApp.iOS.Views.Loading
{
    public partial class LoadingViewController : XViewController<LoadingViewModel>, IRootView
    {
        public LoadingViewController() : base("LoadingViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}

