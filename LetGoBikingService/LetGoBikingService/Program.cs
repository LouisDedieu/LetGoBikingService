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
            Uri baseAddress = new Uri("http://localhost:8090/StationsService");

            // Créer une instance de ServiceHost pour StationsService
            using (ServiceHost host = new ServiceHost(typeof(StationsService), baseAddress))
            {
                try
                {
                    // Ajouter un point de terminaison au service
                    host.AddServiceEndpoint(typeof(IStationsService), new WSHttpBinding(), "");

                    // Activer l'échange de métadonnées
                    ServiceMetadataBehavior smb = new ServiceMetadataBehavior
                    {
                        HttpGetEnabled = true
                    };
                    host.Description.Behaviors.Add(smb);

                    // Démarrer le service
                    host.Open();
                    Console.WriteLine("Le service StationsService est démarré à : " + baseAddress);

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
