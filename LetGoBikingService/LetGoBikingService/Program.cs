using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Uri baseAddress = new Uri("http://localhost:8090/RoutingService");

            // Créer une instance de ServiceHost
            using (ServiceHost host = new ServiceHost(typeof(RouteService), baseAddress))
            {
                try
                {
                    // Ajouter un point de terminaison au service
                    host.AddServiceEndpoint(typeof(IRouteService), new WSHttpBinding(), "RoutingService");

                    // Activer l'échange de métadonnées
                    ServiceMetadataBehavior smb = new ServiceMetadataBehavior
                    {
                        HttpGetEnabled = true
                    };
                    host.Description.Behaviors.Add(smb);

                    // Démarrer le service
                    host.Open();
                    Console.WriteLine("Le service est démarré à : " + baseAddress);

                    // Attendre que l'utilisateur appuie sur une touche pour arrêter le service
                    Console.WriteLine("Appuyez sur <Enter> pour arrêter le service...");
                    Console.ReadLine();

                    // Arrêter le service
                    host.Close();
                }
                catch (CommunicationException ce)
                {
                    Console.WriteLine("Une exception s'est produite : {0}", ce.Message);
                    host.Abort();
                }
            }
        }
    }
}

