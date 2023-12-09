using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using LetGoBikingService.Services;
using LetGoBikingService.Interfaces;

namespace LetGoBikingService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Définir l'URL de base pour le service
            Uri baseAddressRoute = new Uri("http://localhost:8090/RouteService");

            var routeService = new RouteService();
            //Create ServiceHost
            ServiceHost hostRoute = new ServiceHost(routeService, baseAddressRoute);

            // Créer une instance de WSHttpBinding avec une taille maximale de message augmentée
            BasicHttpBinding binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = int.MaxValue, // Taille maximale des messages entrants
                ReaderQuotas = { MaxArrayLength = int.MaxValue, MaxStringContentLength = int.MaxValue } // Ajuster si nécessaire
            };
            //Add a service endpoint
            hostRoute.AddServiceEndpoint(typeof(IRouteService), binding, "");

            //Enable metadata exchange
            ServiceMetadataBehavior smbRoute = new ServiceMetadataBehavior();
            smbRoute.HttpGetEnabled = true;
            hostRoute.Description.Behaviors.Add(smbRoute);

            //Start the Service

            hostRoute.Open();

            Console.WriteLine("RouteService is hosted at " + DateTime.Now.ToString());
            Console.WriteLine("Hosts are running... Press <Enter> key to stop");
            Console.ReadLine();
        }
    }
}
