using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Metars.Models.Responses
{
    public class MetarResponse
    {
        public Meta Meta { get; set; }

        public Altimeter Altimeter { get; set; }

        public List<Cloud> Clouds { get; set; }

        public Visibility Visibility { get; set; }

        public DewPoint DewPoint { get; set; }

        public Temperature Temperature { get; set; }

        [JsonProperty("wind_direction")]
        public WindDirection WindDirection { get; set; }

        [JsonProperty("wind_speed")]
        public WindSpeed WindSpeed { get; set; }

        [JsonProperty("flight_rules")]
        public string FlightRules { get; set; }

        [JsonProperty("raw")]
        public string RAWMetar { get; set; }
    }

    public class Meta
    {
        [JsonProperty("timestamp")]
        public string TimeStamp { get; set; }
    }

    public class Altimeter
    {
        [JsonProperty("value")]
        public double? AltimeterValue { get; set; }
    }

    public class Cloud
    {
        [JsonProperty("repr")]
        public string CloudRaw { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("altitude")]
        public double? Altitude { get; set; }
        [JsonProperty("modifier")]
        public string Modifier { get; set; }
        [JsonProperty("direction")]
        public string Direction { get; set; }
    }

    public class Visibility
    {
        [JsonProperty("value")]
        public double? VisibilityValue { get; set; }
    }

    public class WindDirection
    {
        [JsonProperty("value")]
        public double? WindDirectionValue { get; set; }
    }

    public class WindSpeed
    {
        [JsonProperty("value")]
        public double? WindSpeedValue { get; set; }
    }

    public class DewPoint
    {
        [JsonProperty("value")]
        public double? DewValue { get; set; }
    }

    public class Temperature
    {
        [JsonProperty("value")]
        public double? TemperatureValue { get; set; }
    }
}
