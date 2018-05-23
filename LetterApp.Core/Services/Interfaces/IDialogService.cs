using System;
using System.Threading.Tasks;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IDialogService
    {
        Task<bool> ShowAlert(string title = "", string message = "", AlertType alertType = AlertType.Error, string confirmButtonText = "", string cancelButtonText = null);
        Task<string> ShowInput(string title = "", string confirmButtonText = null, string cancelButtonText = null, string hint = "", InputType inputType = InputType.Number);
        Task<string> ShowOptions(string title = "", OptionsType optionsType = OptionsType.List, string cancelButtonText = "", params string[] options);
        //Task<DateTime> ShowDatePicker(DateTime minimumDate = default(DateTime), DateTime maximumDate = default(DateTime), DateTime defaultDate = default(DateTime));
        void ShowLoading(string title = "", string text = "");
        void HideLoading();
    }

    public enum AlertType
    {
        Null,
        Success,
        Error,
        Timeout,
        Info
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
