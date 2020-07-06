using System;
namespace Metars
{
    public class Constants
    {
        public const string IdentifierChar = "K";

        public static class APIConstants
        {
            public const string APIKey = "je2tJRYWkeLld3qw5oHFiQxh2r8nso0L3I_V5EVcCMA";
            public const string BaseURI = "https://avwx.rest/api/";
            public const string GetMetar = "metar";
            public const string GetStation = "station";
        }

        public static class NavigationPages
        {
            public const string RootNavigation = "RootNavigation";
            public const string AirportPage = "AirportPage";
            public const string AirportDetailPage = "AirportDetailPage";
        }

        public static class NavParams
        {
            public const string SelectedAirport = "SelectedAirport";
        }

        public static class AppCenterAPIKeys
        {
            public const string IOSAppCenterKey = "";
            public const string AndroidAppCenterKey = "";
        }
    }
}
