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
        public int Number { get; set; }                     
        [DataMember]
        public string Name { get; set; }                    
        [DataMember]
        public Position Position { get; set; }              
        [DataMember]
        public int BikeStands { get; set; }                 
        [DataMember]
        public int AvailableBikeStands { get; set; }        
        [DataMember]
        public int AvailableBikes { get; set; }             
        [DataMember]
        public bool Banking { get; set; }                   
        [DataMember]
        public bool Bonus { get; set; }                     
        [DataMember]
        public string Status { get; set; }                  
        [DataMember]
        public long LastUpdate { get; set; }
    }

    [DataContract]
    public class Position
    {     
        [DataMember]
        public double Lat { get; set; }
        [DataMember]
        public double Lng { get; set; }
    }
}

