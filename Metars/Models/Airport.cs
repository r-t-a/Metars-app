using System;
using System.ComponentModel;
using Metars.Models.Responses;
using SQLite;

namespace Metars.Models
{
    [Table(TableName)]
    public class Airport : INotifyPropertyChanged
    {
        public const string TableName = "Airport";

        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
        public string RAWMetar { get; set; }
        public string LastUpdate { get; set; }
        public string FlightRules { get; set; }
        public double? AltimeterSetting { get; set; }
        public double? Visibility { get; set; }
        public double? Temperature { get; set; }
        public double? DewPoint { get; set; }
        public double? WindDirection { get; set; }
        public double? WindSpeed { get; set; }

        public void Copy(Airport other)
        {
            Name = other.Name;
            RAWMetar = other.RAWMetar;
            LastUpdate = other.LastUpdate;
            AltimeterSetting = other.AltimeterSetting;
            Visibility = other.Visibility;
            FlightRules = other.FlightRules;
            Temperature = other.Temperature;
            DewPoint = other.DewPoint;
            WindDirection = other.WindDirection;
            WindSpeed = other.WindSpeed;
        }

        public static Airport MapFromResponse(string identifier, MetarResponse response)
        {
            return new Airport
            {
                Name = identifier,
                RAWMetar = response.RAWMetar,
                LastUpdate = response.Meta.TimeStamp,
                AltimeterSetting = response.Altimeter?.AltimeterValue ?? 0,
                Visibility = response.Visibility?.VisibilityValue ?? 0,
                FlightRules = response.FlightRules,
                Temperature = response.Temperature?.TemperatureValue ?? 0,
                DewPoint = response.DewPoint?.DewValue ?? 0,
                WindDirection = response.WindDirection?.WindDirectionValue ?? 0,
                WindSpeed = response.WindSpeed?.WindSpeedValue ?? 0,
            };
        }
    }
}
