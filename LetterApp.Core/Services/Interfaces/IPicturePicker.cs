using System.Threading.Tasks;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IPicturePicker
    {
        Task<string> GetImageStreamSync(bool resize = true);
        Task<string> GetImageFilePath();
        Task<bool> SaveImageInFiles(string url);
    }
}
