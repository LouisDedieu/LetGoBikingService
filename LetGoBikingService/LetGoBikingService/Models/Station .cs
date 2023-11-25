using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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

