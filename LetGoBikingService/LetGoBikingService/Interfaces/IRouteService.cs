using System.Threading.Tasks;
using System.ServiceModel;
using LetGoBikingService.Models;

namespace LetGoBikingService.Interfaces
{
    [ServiceContract]
    public interface IRouteService
    {
        [OperationContract]
        Task<Itinary> GetItinerary(string origin, string destination);
    }
}