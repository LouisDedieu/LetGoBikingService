using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LetGoBikingService.Models;

namespace LetGoBikingService.Interfaces
{
    [ServiceContract]
    public interface IStationsService
    {
        [OperationContract]
        Task<IEnumerable<Station>> GetStationsAsync(string contractName);
    }
}
