using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace LetGoBikingService.Models
{
    [DataContract]
    public class Position
    {
        public Position() { }

        [DataMember]
        public double latitude { get; set; }

        [DataMember]
        public double longitude { get; set; }
    }

    [DataContract]

    public class CoordinateNominatim
    {
        [JsonPropertyName("lat")]
        [DataMember]
        public string LatitudeNominatim { get; set; }

        [JsonPropertyName("lon")]
        [DataMember]
        public string LongitudeNominatim { get; set; }
    }
}
