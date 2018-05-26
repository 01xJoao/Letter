using System;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Views.CustomViews.Dialog;

namespace LetterApp.iOS.Services
{
    public class DialogService : IDialogService
    {
        public void ShowAlert(string title, AlertType alertType)
        {
            var alertVC = AlertView.Create();
            alertVC.Configure(title, alertType);
        }

        public Task<string> ShowInput(string title = "", string confirmButtonText = null, string cancelButtonText = null, string hint = "", InputType inputType = InputType.Number)
        {
            throw new NotImplementedException();
        }

        public Task<string> ShowOptions(string title = "", OptionsType optionsType = OptionsType.List, string cancelButtonText = "", params string[] options)
        {
            throw new NotImplementedException();
        }

        public void ShowLoading(string title = "", string text = "")
        {
            throw new NotImplementedException();
        }

        public void HideLoading()
        {
            throw new NotImplementedException();
        }
    }
}
