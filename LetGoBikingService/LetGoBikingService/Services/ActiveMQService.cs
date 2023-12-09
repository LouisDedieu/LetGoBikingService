using Apache.NMS;
using LetGoBikingService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LetGoBikingService.Services
{
    public class ActiveMQService
    {
        public bool SendItineraryStepsToQueue(List<Itinary> itinaries)
        {
            try
            {
                IConnectionFactory factory = new NMSConnectionFactory("activemq:tcp://localhost:61616");
                using (IConnection connection = factory.CreateConnection())
                using (Apache.NMS.ISession session = connection.CreateSession())
                {
                    string queueName = itinaries.First().metadata.uuid;
                    IDestination destination = session.GetQueue(queueName);
                    using (IMessageProducer producer = session.CreateProducer(destination))
                    {
                        foreach (var itinary in itinaries)
                        {
                            foreach (var feature in itinary.Features)
                            {
                                foreach (var segment in feature.Properties.Segments)
                                {
                                    foreach (var step in segment.Steps)
                                    {
                                        string messageText = JsonConvert.SerializeObject(step.Instruction)+ " " 
                                                                + JsonConvert.SerializeObject(step.Distance) + "m";
                                        ITextMessage message = session.CreateTextMessage(messageText);
                                        producer.Send(message);
                                    }
                                }
                            }
                        }
                    }
                }
                return true; // Connexion et envoi réussis
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'envoi à la queue ActiveMQ: {ex.Message}");
                return false; // Connexion ou envoi échoué
            }
        }
    }
}
