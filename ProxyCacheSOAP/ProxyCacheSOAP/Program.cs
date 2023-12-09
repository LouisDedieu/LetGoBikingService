using System;
using System.ServiceModel.Description;
using System.ServiceModel;

namespace ProxyCacheSOAP
{
    class Program
    {
        static void Main(string[] args)
        {

            // Définir l'URL de base pour le service
            Uri baseAddressRoute = new Uri("http://localhost:8091/ProxyCacheSOAP/Service1");

            //Create ServiceHost
            ServiceHost hostRoute = new ServiceHost(typeof(ProxyService), baseAddressRoute);

            //Add a service endpoint
            hostRoute.AddServiceEndpoint(typeof(IProxyService), new BasicHttpBinding(), "");
            // Ajouter un endpoint pour les métadonnées
            //hostRoute.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            //Enable metadata exchange
            ServiceMetadataBehavior smbRoute = new ServiceMetadataBehavior();
            smbRoute.HttpGetEnabled = true;
            smbRoute.HttpsGetEnabled = true;
            hostRoute.Description.Behaviors.Add(smbRoute);

            //Start the Service

           try
            {
                // Open the service host to accept incoming calls  
                hostRoute.Open();                // The service can now be accessed.  
                Console.WriteLine("The Proxy service is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();

                Console.ReadLine();
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine("There was a communication problem. " + commProblem.Message);
                Console.Read();
            }
        }
    }
}
