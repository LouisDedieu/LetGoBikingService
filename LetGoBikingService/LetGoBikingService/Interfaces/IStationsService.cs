using LetGoBikingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LetGoBikingService.Interfaces
{
    [ServiceContract]
    public interface IStationsService
    {
        [OperationContract]
        Task<List<Station>> GetStationsAsync(string contractName);
    }
}

