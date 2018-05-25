using System;
using System.Threading;
using System.Threading.Tasks;

namespace LetterApp.Core.Services.Interfaces
{
    public interface IWebService
    {
        Task<T> PostAsync<T>(string resource, object body = null, CancellationToken ct = default(CancellationToken), bool needsHeaderCheck = true);
    }
}
