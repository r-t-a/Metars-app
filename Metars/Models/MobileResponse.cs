using System;
using System.Net;

namespace Metars.Models
{
    public class MobileResponse<TResponse>
    {
        public HttpStatusCode StatusCode { get; set; }
        public TResponse Response { get; set; }
    }
}
