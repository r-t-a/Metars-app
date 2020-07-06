using System;
using System.Net.Http;

namespace Metars.Services.Interfaces
{
    public interface IHttpClientBuilder
    {
        HttpClient GetHttpClient();
    }
}
