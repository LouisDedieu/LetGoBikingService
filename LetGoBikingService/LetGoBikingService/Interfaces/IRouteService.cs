using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using LetGoBikingService.Models;


namespace LetGoBikingService.Interfaces
{
    [ServiceContract]
    public interface IRouteService
    {
        [OperationContract]
        Task<Itinerary> GetItinerary(string origin, string destination);
    }
}
