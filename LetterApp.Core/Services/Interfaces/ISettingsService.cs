using System;
using System.Threading.Tasks;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.Services.Interfaces
{
    public interface ISettingsService
    {
        void Logout();
        bool CheckMicrophonePermissions();
        Task<bool> CheckNotificationPermissions();
        void OpenSettings();
        Task<BaseModel> SendPushNotificationToken(string token);
    }
}
