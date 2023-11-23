using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using LetGoBikingService.Services;
using LetGoBikingService.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LetGoBikingService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Définir l'URL de base pour le service
            Uri baseAddressStations = new Uri("http://localhost:8090/StationsService");
            Uri baseAddressContracts = new Uri("http://localhost:8090/ContractService");

            //Create ServiceHost
            ServiceHost hostStations = new ServiceHost(typeof(StationsService), baseAddressStations);
            ServiceHost hostContracts = new ServiceHost(typeof(ContractService), baseAddressContracts);

            // Multiple end points can be added to the Service using AddServiceEndpoint() method.
            // Host.Open() will run the service, so that it can be used by any client.

            //Add a service endpoint
            hostStations.AddServiceEndpoint(typeof(IStationsService), new WSHttpBinding(), "");
            hostContracts.AddServiceEndpoint(typeof(IContractService), new WSHttpBinding(), "");

            //Enable metadata exchange
            ServiceMetadataBehavior smbStations = new ServiceMetadataBehavior();
            ServiceMetadataBehavior smbContracts = new ServiceMetadataBehavior();
            smbStations.HttpGetEnabled = true;
            smbContracts.HttpGetEnabled = true;
            hostStations.Description.Behaviors.Add(smbStations);
            hostContracts.Description.Behaviors.Add(smbContracts);

            //Start the Service
            hostStations.Open();
            hostContracts.Open();

            Console.WriteLine("StationsService is hosted at " + DateTime.Now.ToString());
            Console.WriteLine("ContractService is hosted at " + DateTime.Now.ToString());
            Console.WriteLine("Hosts are running... Press <Enter> key to stop");

            RouteService rs = new RouteService();
            Contract c = await rs.FindNearestContract("25b avenue de nice");
            Console.WriteLine(c.name);
          

            Console.ReadLine();
        }
    }
}
