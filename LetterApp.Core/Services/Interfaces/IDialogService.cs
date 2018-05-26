using System;
using System.Threading.Tasks;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IDialogService
    {
        Task<string> ShowInput(string title = "", string confirmButtonText = null, string cancelButtonText = null, string hint = "", InputType inputType = InputType.Number);
        Task<string> ShowOptions(string title = "", OptionsType optionsType = OptionsType.List, string cancelButtonText = "", params string[] options);
        void ShowAlert(string title, AlertType alertType);
        void ShowLoading();
        void HideLoading();
    }

    public enum AlertType
    {
        Null,
        Success,
        Error,
        Info,
    }

    public enum InputType
    {
        Null,
        Text,
        Number,
        Phome
    }

    public enum OptionsType
    {
        Null,
        List,
        Radio
    }
}
