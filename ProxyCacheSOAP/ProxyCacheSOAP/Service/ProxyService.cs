using ProxyCacheSOAP.Models;
using ProxyCacheSOAP.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProxyCacheSOAP
{
    public class ProxyService : IProxyService
    {
        private GenericProxyCache<List<Contract>> contractCache = new GenericProxyCache<List<Contract>>();
        private GenericProxyCache<List<Station>> stationCache = new GenericProxyCache<List<Station>>();
        private readonly double contractExpirationTime = 10080; //10080min = 1 week
        private readonly double stationExpirationTime = 10;
        public ProxyService() { }

        public async Task<List<Contract>> GetListContract()
        {
            return contractCache.Get("contracts", () => JCDecauxAPI.GetContractsAsync().Result, contractExpirationTime);
        }

        public async Task<List<Station>> GetListStations(string contractName)
        {
            return stationCache.Get($"stations_{contractName}", () => JCDecauxAPI.GetStationsAsync(contractName).Result, stationExpirationTime);
        }
    }
}
