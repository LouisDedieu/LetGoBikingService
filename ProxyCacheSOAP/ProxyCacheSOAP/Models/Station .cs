using System.Runtime.Serialization;

namespace ProxyCacheSOAP.Models
{
    [DataContract]
    public class Station
    {
        [DataMember]
        public int number { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public Position position { get; set; }
        [DataMember]
        public double available_bikes { get; set; }
    }
}

