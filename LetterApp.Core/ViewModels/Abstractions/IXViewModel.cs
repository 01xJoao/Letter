using System;
using System.Threading.Tasks;

namespace LetterApp.Core.ViewModels.Abstractions
{
    public interface IXViewModel
    {
        void InitializeViewModel();
        Task Appearing();
        Task Appeared();
        Task Disappearing();
        void Prepare();
        void Prepare(object dataObject);
    }
}
