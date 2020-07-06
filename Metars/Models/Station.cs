using System;
using System.Collections.Generic;
using System.ComponentModel;
using Metars.Models.Responses;
using SQLite;

namespace Metars.Models
{
    [Table(TableName)]
    public class Station : INotifyPropertyChanged
    {
        public const string TableName = "Station";

        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string AirportIdentifier { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public int RunwayCount { get; set; }

        public void Copy(Station other)
        {
            Name = other.Name;
            Note = other.Note;
            AirportIdentifier = other.AirportIdentifier;
            Latitude = other.Latitude;
            Longitude = other.Longitude;
            Elevation = other.Elevation;
            RunwayCount = other.RunwayCount;
        }

        public static Station MapFromResponse(StationResponse stationResponse)
        {
            return new Station
            {
                Name = stationResponse.Name,
                Note = stationResponse.Note,
                AirportIdentifier = stationResponse.ICAO,
                Latitude = stationResponse.Latitude,
                Longitude = stationResponse.Longitude,
                Elevation = stationResponse.Elevation,
                RunwayCount = stationResponse.Runways.Count
            };
        }
    }
}
