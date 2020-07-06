using System;
using System.Net;
using Metars.Models.Responses;
using Prism.Events;

namespace Metars.Events
{
    public struct StationResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public StationResponse StationResponse { get; set; }

        public StationResult(StationResponse response, HttpStatusCode statusCode)
        {
            StationResponse = response;
            StatusCode = statusCode;
        }
    }

    public class StationResponseEvent : PubSubEvent<StationResult>
    {
    }
}
