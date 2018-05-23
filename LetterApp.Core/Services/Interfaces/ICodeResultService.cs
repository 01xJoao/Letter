using System;
namespace LetterApp.Core.Services.Interfaces
{
    public interface ICodeResultService
    {
        void SetCodes();
        string GetCodeDescription(int code);
    }
}
