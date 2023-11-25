using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using LetGoBikingService.Services;
using LetGoBikingService.Interfaces;
using System.Threading.Tasks;

namespace LetGoBikingService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Définir l'URL de base pour le service
            Uri baseAddressRoute = new Uri("http://localhost:8090/RouteService");

            //Create ServiceHost
            ServiceHost hostRoute = new ServiceHost(typeof(RouteService), baseAddressRoute);

            // Multiple end points can be added to the Service using AddServiceEndpoint() method.
            // Host.Open() will run the service, so that it can be used by any client.

            //Add a service endpoint
            hostRoute.AddServiceEndpoint(typeof(IRouteService), new WSHttpBinding(), "");

            //Enable metadata exchange
            ServiceMetadataBehavior smbRoute = new ServiceMetadataBehavior();
            smbRoute.HttpGetEnabled = true;
            hostRoute.Description.Behaviors.Add(smbRoute);

            //Start the Service

            hostRoute.Open();

            Console.WriteLine("RouteService is hosted at " + DateTime.Now.ToString());
            Console.WriteLine("Hosts are running... Press <Enter> key to stop");

/*            String[] addressOrigin = { "lyon", "place bellecour" };
            String[] addressDestination = { "lyon", "5 rue Félix Rollet" };

            RouteService rs = new RouteService();
            await rs.GetItinerary(addressOrigin[1], addressDestination[1]);*/

            Console.ReadLine();
        }
    }
}
