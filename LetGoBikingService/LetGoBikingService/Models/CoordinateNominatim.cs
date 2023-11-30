using LetGoBikingService.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LetGoBikingService.Models
{
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
        [DataMember]
        public string LatitudeNominatim { get; set; }

        [JsonPropertyName("lon")]
        [DataMember]
        public string LongitudeNominatim { get; set; }

    }
}
