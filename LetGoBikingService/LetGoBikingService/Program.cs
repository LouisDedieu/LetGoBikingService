using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using LetGoBikingService.Services;
using LetGoBikingService.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using LetGoBikingService.Models;
using System.Data;

namespace LetGoBikingService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Définir l'URL de base pour le service
            Uri baseAddressStations = new Uri("http://localhost:8090/StationsService");
            Uri baseAddressContracts = new Uri("http://localhost:8090/ContractService");
            Uri baseAddressRoute = new Uri("http://localhost:8090/RouteService");

            //Create ServiceHost
            ServiceHost hostStations = new ServiceHost(typeof(StationsService), baseAddressStations);
            ServiceHost hostContracts = new ServiceHost(typeof(ContractService), baseAddressContracts);
            ServiceHost hostRoute = new ServiceHost(typeof(RouteService), baseAddressRoute);

            // Multiple end points can be added to the Service using AddServiceEndpoint() method.
            // Host.Open() will run the service, so that it can be used by any client.

            //Add a service endpoint
            hostStations.AddServiceEndpoint(typeof(IStationsService), new WSHttpBinding(), "");
            hostContracts.AddServiceEndpoint(typeof(IContractService), new WSHttpBinding(), "");
            hostRoute.AddServiceEndpoint(typeof(IRouteService), new WSHttpBinding(), "");

            //Enable metadata exchange
            ServiceMetadataBehavior smbStations = new ServiceMetadataBehavior();
            ServiceMetadataBehavior smbContracts = new ServiceMetadataBehavior();
            ServiceMetadataBehavior smbRoute = new ServiceMetadataBehavior();
            smbStations.HttpGetEnabled = true;
            smbContracts.HttpGetEnabled = true;
            smbRoute.HttpGetEnabled = true;
            hostStations.Description.Behaviors.Add(smbStations);
            hostContracts.Description.Behaviors.Add(smbContracts);
            hostRoute.Description.Behaviors.Add(smbRoute);

            //Start the Service
            hostStations.Open();
            hostContracts.Open();
            hostRoute.Open();

            Console.WriteLine("StationsService is hosted at " + DateTime.Now.ToString());
            Console.WriteLine("ContractService is hosted at " + DateTime.Now.ToString());
            Console.WriteLine("Hosts are running... Press <Enter> key to stop");

/*            List<Station> stations = await JCDecauxAPI.GetStationsAsync("lyon");
            CoordinateNominatim coordinateOrigin = await OpenStreetMapAPI.GetCoordinates("5 rue Félix Rollet");
            Console.WriteLine("calling FIndneareststation");

            Station s = StationsService.FindNearestStation(stations, coordinateOrigin);

            Console.WriteLine(s.name);*/

            Console.ReadLine();
        }
    }
}
