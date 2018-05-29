using System;
namespace LetterApp.Core.Services.Interfaces
{
    public interface IStatusCodeService
    {
        string GetStatusCodeDescription(int? code);
    }
}
