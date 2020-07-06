using System;
using System.Net;
using Metars.Models.Responses;
using Prism.Events;

namespace Metars.Events
{
    public struct MetarResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public MetarResponse MetarResponse { get; set; }
        public string AirportIdentifier { get; set; }

        public MetarResult(MetarResponse response, HttpStatusCode statusCode, string airportIdentifier)
        {
            MetarResponse = response;
            StatusCode = statusCode;
            AirportIdentifier = airportIdentifier;
        }
    }

    public class MetarResponseEvent : PubSubEvent<MetarResult>
    {
    }
}
