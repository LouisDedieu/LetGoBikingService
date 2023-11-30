using System.Threading.Tasks;
using System.ServiceModel;
using LetGoBikingService.Models;
using System.Collections.Generic;

namespace LetGoBikingService.Interfaces
{
    [ServiceContract]
    public interface IRouteService
    {
        [OperationContract]
        Task<List<Itinary>> GetItinerary(string origin, string destination);
    }
}