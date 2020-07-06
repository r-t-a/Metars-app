using System;
using System.Net.Http;
using Metars.Services.Interfaces;
using Xamarin.Android.Net;

namespace Metars.Droid.Services
{
    public class HttpClientBuilder : IHttpClientBuilder
    {
        public HttpClient GetHttpClient()
        {
            var timeout = TimeSpan.FromSeconds(10);
            var handler = new AndroidClientHandler
            {
                ReadTimeout = timeout
            };
            var client = new HttpClient(handler) { Timeout = timeout };
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
    }
}
