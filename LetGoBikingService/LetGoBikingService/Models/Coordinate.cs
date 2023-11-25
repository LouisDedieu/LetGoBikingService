using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace LetGoBikingService.Models
{
    [DataContract]
    public class Position
    {
        public Position() { }

        [JsonPropertyName("lat")]
        [DataMember]
        public double latitude { get; set; }

        [JsonPropertyName("lng")]
        [DataMember]
        public double longitude { get; set; }

        internal double GetDistanceTo(CoordinateNominatim coordinateNominatim)
        {
            //Conversion de string en double
            if (!double.TryParse(coordinateNominatim.LatitudeNominatim, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat2) ||
                !double.TryParse(coordinateNominatim.LongitudeNominatim, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon2))
            {
                throw new ArgumentException("La conversion des coordonnées d'origine en double a échoué.");
            }

            var R = 6371e3; // Rayon de la Terre en mètres

            var lat1 = this.latitude * Math.PI / 180; // Convertit en radians
            lat2 = lat2 * Math.PI / 180;

            var deltaLat = (lat2 - lat1);
            var deltaLon = (lon2 * Math.PI / 180) - (this.longitude * Math.PI / 180);

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Distance en mètres

        }
    }

    [DataContract]

    public class CoordinateNominatim
    {
        public CoordinateNominatim() { }

        public CoordinateNominatim(string startLng, string startLat)
        {
            LongitudeNominatim = startLng;
            LatitudeNominatim = startLat;
        }

        [JsonPropertyName("lat")]
        public string LatitudeNominatim { get; set; }

        [JsonPropertyName("lon")]
        public string LongitudeNominatim { get; set; }
    }

}
