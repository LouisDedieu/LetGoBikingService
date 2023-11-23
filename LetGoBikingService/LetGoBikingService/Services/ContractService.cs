using LetGoBikingService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetGoBikingService.Services
{
    internal class ContractService : IContractService
    {
        public async Task<Contract> GetContractUsingCityNameAsync(string cityName)
        {
            return await JCDecauxAPI.GetContractUsingCityNameAsync(cityName);
        }
    }
}
