using System;
using System.Threading.Tasks;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IXNavigationService
    {
        void Initialize();
        Task NavigateAsync<TViewModel, TObject>(TObject data) where TViewModel : class, IXViewModel;
        Task Close<TViewModel>(TViewModel viewModel) where TViewModel : class, IXViewModel;
    }
}
