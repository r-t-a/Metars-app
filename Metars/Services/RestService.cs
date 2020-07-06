using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Metars.Models;
using Metars.Services.Interfaces;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Events;
using Unity;

namespace Metars.Services
{
    public class RestService : IRestService
    {
        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        private readonly IHttpClientBuilder _clientBuilder;
        private HttpClient _httpClient;

        public RestService(IHttpClientBuilder clientBuilder)
        {
            _clientBuilder = clientBuilder;
        }

        public async Task<MobileResponse<TResponseType>> Post<TResponseType>(string requestUri, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var request = PrepareRequest(requestUri, HttpMethod.Get, cancellationToken);
            return await ExecuteRequestAsync<TResponseType>(request, cancellationToken).ConfigureAwait(false);
        }

        private async Task<MobileResponse<TOut>> ExecuteRequestAsync<TOut>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var client = GetClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("basic", Constants.APIConstants.APIKey);

            var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var responseReturn = new MobileResponse<TOut>
                {
                    StatusCode = response.StatusCode,
                };
                return responseReturn;
            }

            try
            {
                var content = await response.Content.ReadAsStringAsync();
                var deserialized = JsonConvert.DeserializeObject<TOut>(content);
                var responseReturn = new MobileResponse<TOut>
                {
                    StatusCode = response.StatusCode,
                    Response = deserialized,
                };
                return responseReturn;

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            return default;
        }

        private HttpRequestMessage PrepareRequest(string requestUri, HttpMethod httpMethod, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var fullRequestUri = new Uri(new Uri(Constants.APIConstants.BaseURI), requestUri);
            return new HttpRequestMessage(httpMethod, fullRequestUri);
        }

        private HttpClient GetClient(bool newClient = false)
        {
            return newClient ? _clientBuilder.GetHttpClient() : _httpClient ?? (_httpClient = _clientBuilder.GetHttpClient());
        }
    }
}
