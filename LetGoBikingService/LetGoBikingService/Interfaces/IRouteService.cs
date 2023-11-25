using System.Threading.Tasks;
using System.ServiceModel;


namespace LetGoBikingService.Interfaces
{
    [ServiceContract]
    public interface IRouteService
    {
        [OperationContract]
        Task<string> GetItinerary(string origin, string destination);
    }
}
