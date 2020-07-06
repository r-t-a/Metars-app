using System;
using System.Net.Http;
using Foundation;
using Metars.Services.Interfaces;

namespace Metars.iOS.Services
{
    public class HttpClientBuilder : IHttpClientBuilder
    {
        public HttpClient GetHttpClient()
        {
            var config = NSUrlSessionConfiguration.DefaultSessionConfiguration;
            config.TimeoutIntervalForRequest = 30;
            config.TimeoutIntervalForResource = 30;
            var handler = new NSUrlSessionHandler(config);
            var client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
    }
}
