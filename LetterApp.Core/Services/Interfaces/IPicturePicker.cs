using System;
using System.IO;
using System.Threading.Tasks;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IPicturePicker
    {
        Task<string> GetImageStreamSync(bool resize = true);
    }
}
