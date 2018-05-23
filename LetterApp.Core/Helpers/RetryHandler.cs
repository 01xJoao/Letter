using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LetterApp.Core.Helpers
{
    public class RetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 3;
        private readonly TimeSpan RetryTimeout = TimeSpan.FromSeconds(1);

        public RetryHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            for (int i = 0; i < MaxRetries; i++)
            {
                Debug.Write($"RetryHandler try number: {i} \n");
                var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                    return response;

                await Task.Delay(RetryTimeout, cancellationToken).ConfigureAwait(false);
            }

            return null;
        }
    }
}
