using ProxyCacheSOAP.Models;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ProxyCacheSOAP
{
    [ServiceContract]
    public interface IProxyService
    {
        [OperationContract]
        Task<List<Station>> GetListStations(string contractName);

        [OperationContract]
        Task<List<Contract>> GetListContract();
    }
}

