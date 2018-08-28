using System;
using System.Threading.Tasks;

namespace LetterApp.Core.Services.Interfaces
{
    public interface ISettingsService
    {
        void Logout();
        bool CheckMicrophonePermissions();
        Task<bool> CheckNotificationPermissions();
        void OpenSettings();
    }
}
