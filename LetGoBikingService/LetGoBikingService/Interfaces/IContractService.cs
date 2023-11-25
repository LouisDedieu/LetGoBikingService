using LetGoBikingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LetGoBikingService.Interfaces
{
    [ServiceContract]
    internal interface IContractService
    {
        [OperationContract]
        Task<Contract> GetContractUsingCityNameAsync(string cityName);
    }
}

[DataContract]
public class Contract
{
    [DataMember]
    public string name { get; set; }


    [DataMember]
    public string[] cities { get; set; }
}
