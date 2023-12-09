using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProxyCacheSOAP.Models
{
    [DataContract]
    public class Position
    {
        public Position() { }

        public Position(double lat, double lon)
        {
            this.latitude = lat;
            this.longitude = lon;
        }

        [JsonProperty("lat")]
        [DataMember]
        public double latitude { get; set; }

        [JsonProperty("lng")]
        [DataMember]
        public double longitude { get; set; }
    }
}
