﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Models;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IDialogService
    {
        void ShowAlert(string title, AlertType alertType, float duration = 3.5f);
        Task<string> ShowTextInput(string title = "", string subtitle = "", string inputContent = "", string confirmButtonText = "", string hint = "", InputType inputType = InputType.Text, QuestionType questionType = QuestionType.Normal);
        Task<bool> ShowInformation(string title = "", string text1 = "", string text2 = "",string text3 = "", string confirmButtonText = "");
        Task<bool> ShowQuestion(string title = "", string buttonText = "", QuestionType questionType = QuestionType.Normal);
        Task<Tuple<bool,bool>> ShowFilter(string title = "", List<ContactDialogFilter> filters = null, string buttonText = "");
        Task<CallingType> ShowContactOptions(Dictionary<string, string> locationResources, bool showPhoneOption = true);
        void ShowCallStack(string title = "", List<CallStackModel> calls = null);
        Task<bool> ShowMessageAlert(string photo = "", string name = "", string message = "");
        Task<Tuple<ChatOptions,bool>> ShowChatOptions(string name = "", string photo = "", bool muted = false, string[] resources = null);
        Task<bool> ShowPicture(string image = "", string send = "", string cancel = "");
        Task<bool> ShowChatImage(string image = "", string save = "");

        void StartLoading(LoadingColor color = LoadingColor.Blue);
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
        Phone,
        Password
    }

    public enum QuestionType
    {
        Null,
        Good,
        Bad,
        Normal
    }

    public enum CallingType
    {
        Letter,
        Cellphone,
        Close
    }

    public enum LoadingColor
    {
        Blue,
        White
    }

    public enum ChatOptions
    {
        SeeProfile,
        SendEmail,
        MuteChat,
        ArchiveChat,
        Count
    }
}
