using System;
namespace LetterApp.Core.Models.Cells
{
    public class FormModel
    {
        public FormModel(string textFieldValue, string indicatorText, FieldType fieldType, ReturnKeyType returnKeyType, string[] buttonText = null, Action buttonAction = null, string keyboardButtonText = "", Action submitKeyboardButton = null)
        {
            TextFieldValue = textFieldValue;
            IndicatorText = indicatorText;
            ButtonText = buttonText;
            ButtonAction = buttonAction;
            FieldType = fieldType;
            ReturnKeyType = returnKeyType;
            KeyboardButtonText = keyboardButtonText;
            SubmitKeyboardButtonAction = submitKeyboardButton;
        }

        public string TextFieldValue { get; set; }
        public string IndicatorText { get; set; }

        public string[] ButtonText { get; set; }
        public Action ButtonAction { get; set; }

        public FieldType FieldType { get; set; }
        public ReturnKeyType ReturnKeyType { get; set; }

        public string KeyboardButtonText { get; set; }
        public Action SubmitKeyboardButtonAction { get; set; }
    }

    public enum FieldType
    {
        Email,
        Password,
        Phone,
        Code,
        Null
    }

    public enum ReturnKeyType
    {
        Default,
        Next,
        Null
    }
}
