using System;
using System.Net;
using Metars.Models.Responses;
using Prism.Events;

namespace Metars.Events
{
    public struct Station
    {
        public HttpStatusCode StatusCode { get; set; }
        public StationResponse StationResponse { get; set; }
        public Station(StationResponse response, HttpStatusCode statusCode)
        {
            StationResponse = response;
            StatusCode = statusCode;
        }
    }

    public class StationResponseEvent : PubSubEvent<Station>
    {
        
    }
}
