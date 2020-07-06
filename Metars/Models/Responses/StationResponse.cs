using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Metars.Models.Responses
{
    public class StationResponse
    {
        public string ICAO { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [JsonProperty("elevation_ft")]
        public double Elevation { get; set; }
        public List<Runway> Runways { get; set; }
    }

    public class Runway
    {
        [JsonProperty("length_ft")]
        public long? RunwayLength { get; set; }
        [JsonProperty("width_ft")]
        public long? RunwayWidth { get; set; }
        [JsonProperty("ident1")]
        public string Identifier1 { get; set; }
        [JsonProperty("ident2")]
        public string Identifier2 { get; set; }
    }
}
