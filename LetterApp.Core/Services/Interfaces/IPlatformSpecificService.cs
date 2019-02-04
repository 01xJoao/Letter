using System;
namespace LetterApp.Core.Services.Interfaces
{
    public interface IPlatformSpecificService
    {
        string PlatformLanguage();
        void ExitApp();
    }
}
