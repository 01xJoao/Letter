using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using SharpRaven.Data;

namespace LetterApp.Core.Helpers
{
    public class RetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 3;
        private readonly TimeSpan RetryTimeout = TimeSpan.FromSeconds(1);

        public static IRavenService _ravenService;
        public static IRavenService RavenService => _ravenService ?? (_ravenService = App.Container.GetInstance<IRavenService>());

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
            RavenService.Raven.Capture(new SentryEvent(new Exception("Retry Handler Exeception")));
            return null;
        }
    }
}
