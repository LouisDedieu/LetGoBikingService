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
    private readonly JCDecauxAPI jcDecauxService;

    public StationsService(JCDecauxAPI jcDecauxService)
    {
        this.jcDecauxService = jcDecauxService;
    }

    public async Task<IEnumerable<Station>> GetStationsAsync(string contractName)
    {
        return await jcDecauxService.GetStationsAsync(contractName);
    }
}
