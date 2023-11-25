using System.Runtime.Serialization;

namespace LetGoBikingService.Models
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

        internal bool hasAvailableBike()
        {
            return available_bikes > 0;
        }
    }
}

