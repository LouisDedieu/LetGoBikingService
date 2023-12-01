using ProxyCacheSOAP.Models;
using ProxyCacheSOAP.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProxyCacheSOAP
{
    public class ProxyService : IProxyService
    {
        private GenericProxyCache<List<Contract>> contractCache = new GenericProxyCache<List<Contract>>();
        private GenericProxyCache<List<Station>> stationCache = new GenericProxyCache<List<Station>>();

        public ProxyService() { }

        public async Task<List<Contract>> GetListContract()
        {
            return contractCache.Get("contracts", () => JCDecauxAPI.GetContractsAsync().Result);
        }

        public async Task<List<Station>> GetListStations(string contractName)
        {
            return stationCache.Get($"stations_{contractName}", () => JCDecauxAPI.GetStationsAsync(contractName).Result);
        }
    }
}
