using LetGoBikingService;
using LetGoBikingService.Interfaces;
using LetGoBikingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class StationsService : IStationsService
{

    public async Task<List<Station>> GetStationsAsync(string contractName)
    {
        return await JCDecauxAPI.GetStationsAsync(contractName);
    }

    public async Task<Station> FindNearestStation(string address)
    {
        return await JCDecauxAPI.FindNearestStation();
    }

}
