using System;
using System.Threading.Tasks;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IDialogService
    {
        void ShowAlert(string title, AlertType alertType);
        Task<string> ShowTextInput(string title = "", string inputContent = "", string confirmButtonText = "", string hint = "", InputType inputType = InputType.Text);
        Task<string> ShowOptions(string title = "", OptionsType optionsType = OptionsType.List, string cancelButtonText = "", params string[] options);
        void StartLoading();
        void StopLoading();
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
        Email,
        Text,
        Number,
        Phone
    }

    public enum OptionsType
    {
        Null,
        List,
        Radio
    }
}
