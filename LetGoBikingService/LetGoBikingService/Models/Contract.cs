using System.Runtime.Serialization;

namespace LetGoBikingService.Models
{
    [DataContract]
    public class Contract
    {
        [DataMember]
        public string name { get; set; }


        [DataMember]
        public string[] cities { get; set; }
    }
}
