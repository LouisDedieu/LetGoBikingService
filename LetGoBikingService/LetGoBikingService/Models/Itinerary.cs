using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace LetGoBikingService.Models
{
    [DataContract]
    public class Itinerary
    {
        [DataMember]
        public string StartLocation { get; set; }

        [DataMember]
        public string EndLocation { get; set; }

        // Autres propriétés...
    }
}
