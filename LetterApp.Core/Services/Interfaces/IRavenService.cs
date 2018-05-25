using System;
using SharpRaven;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IRavenService
    {
        RavenClient Raven { get; set; }
    }
}
