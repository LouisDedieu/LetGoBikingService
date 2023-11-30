using ProxyCacheSOAP.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProxyCacheSOAP
{
    public class ProxyService : IProxyService
    {
        public ProxyService() { }

        public async Task<List<Contract>> GetListContract()
        {
            var listContract = await JCDecauxAPI.GetContractsAsync();
            return listContract;
        }

        public async Task<List<Station>> GetListStations(string contractName)
        {
            var listStation = await JCDecauxAPI.GetStationsAsync(contractName);
            return listStation;
        }
    }
}
