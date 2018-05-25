using System;
using LetterApp.Core.Services.Interfaces;
using SharpRaven;

namespace LetterApp.Core.Services
{
    public class RavenService : IRavenService
    {
        private RavenClient _raven;
        public RavenClient Raven
        {
            get => _raven;
            set => _raven = value;
        }

        public RavenService()
        {
            _raven = new RavenClient("https://94048915b42e44a89bef9f4be64377fa@sentry.io/1211959");
        }
    }
}
