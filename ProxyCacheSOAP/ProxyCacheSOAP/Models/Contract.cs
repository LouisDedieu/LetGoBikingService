using System.Runtime.Serialization;

namespace ProxyCacheSOAP.Models
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
