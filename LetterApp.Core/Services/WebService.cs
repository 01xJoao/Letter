using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Helpers;
using LetterApp.Core.Serialization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;
using Newtonsoft.Json;
using SharpRaven.Data;
using Xamarin.Essentials;

namespace LetterApp.Core.Services
{
    public class WebService : IWebService
    {
        private static IRavenService _ravenService;
        private static IRavenService RavenService => _ravenService ?? (_ravenService = App.Container.GetInstance<IRavenService>());

        private static readonly string _basePath = @"https://www.lettermessenger.com/api/";

        private HttpClient _httpClient;
        private HttpClient HttpClient => _httpClient ?? (_httpClient = new HttpClient(new RetryHandler(new HttpClientHandler())){ BaseAddress = new Uri(_basePath) });

        public async Task<T> PostAsync<T>(string resource, object body = null, CancellationToken ct = default(CancellationToken), bool needsHeaderCheck = true)
        {
            try
            {
                if(!VerifyInternetConnection())
                    throw new NoInternetException();

                var contentPost = CreateSerializedHttpContent(body);

                if (needsHeaderCheck)
                    await UpdateClientTokenHeaderAsync(HttpClient).ConfigureAwait(false);
                
                var response = await HttpClient.PostAsync(resource, contentPost, ct).ConfigureAwait(false);
                Debug.WriteLine(response);

                await EnsureSuccessRequest(response);
                var result = await DeserializeAsync<T>(response).ConfigureAwait(false);
                return result;
            }
            catch (Exception e)
            {
                if (!(e is WrongCredentialsException) && !VerifyInternetConnection())
                    throw new NoInternetException(e.Message);
                
                RavenService.Raven.Capture(new SentryEvent(e));
                throw;
            }
        }

        public async Task<T> GetAsync<T>(string requestUri, CancellationToken ct = default(CancellationToken), bool needsHeaderCheck = true)
        {
            try
            {
                if (!VerifyInternetConnection())
                    throw new NoInternetException();

                if (needsHeaderCheck)
                    await UpdateClientTokenHeaderAsync(HttpClient).ConfigureAwait(false);

                var response = await HttpClient.GetAsync(requestUri, ct);
                Debug.WriteLine(response);

                await EnsureSuccessRequest(response);
                var obj = await DeserializeAsync<T>(response);
                return obj;
            }
            catch (Exception e)
            {
                if (!(e is WrongCredentialsException) && !VerifyInternetConnection())
                    throw new NoInternetException(e.Message);

                RavenService.Raven.Capture(new SentryEvent(e));
                throw;
            }
        }

        private async Task EnsureSuccessRequest(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RavenService.Raven.Capture(new SentryEvent(new Exception($"EnsureSuccessRequestException: {content}")));

                if (response.ToString().Contains("Invalid Token") || response.ToString().Contains("Expired token"))
                    throw new SessionTimeoutException();
                else if (response.ToString().Contains("error"))
                    throw new ServerErrorException();
                else
                    throw new WrongCredentialsException();
            }

            if (!content.Contains("success"))
                throw new ServerErrorException();

            if (!response.IsSuccessStatusCode)
                throw new ServerErrorException();
        }

        private HttpContent CreateSerializedHttpContent(object body)
        {
            HttpContent contentPost = null;

            if (body != null)
            {
                var serializedObject = JsonConvert.SerializeObject(body);
                contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            }

            return contentPost;
        }

        private async Task UpdateClientTokenHeaderAsync(HttpClient client)
        {
            if (new DateTime(AppSettings.AuthTokenExpirationDate) < DateTime.UtcNow.AddDays(2))
                await RefreshTokenAsync().ConfigureAwait(false);
            else if (client.DefaultRequestHeaders.Authorization != null)
                return;

            if (!string.IsNullOrEmpty(AppSettings.AuthToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSettings.AuthToken);
        }

        private async Task<TokensModel> RefreshTokenAsync()
        {
            var email = AppSettings.UserEmail;
            var password = await SecureStorage.GetAsync("password");
            var requestModel = new UserRequestModel(email, password);

            var token = await PostAsync<TokensModel>("usercheck/tokens", requestModel, needsHeaderCheck: false).ConfigureAwait(false);

            if (token.StatusCode == 200)
            {
                AppSettings.AuthToken = token.AuthToken;
                AppSettings.AuthTokenExpirationDate = token.AuthTokenExpirationDate.Ticks;
                return token;
            }
            else
                throw new ServerErrorException();
        }

        private async Task<T> DeserializeAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
                return default(T);

            var json = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new NullToEmptyStringResolver(),
                Converters = { new JsonConvert<T>() }
            };

            var deserializedData = JsonConvert.DeserializeObject<T>(json, jsonSettings);
            return deserializedData;
        }

        public bool VerifyInternetConnection()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
