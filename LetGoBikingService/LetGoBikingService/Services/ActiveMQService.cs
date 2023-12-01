using Apache.NMS;
using Apache.NMS.ActiveMQ;
using LetGoBikingService.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace LetGoBikingService.Services
{
    public class ActiveMQService
    {
        public async Task SendItineraryStepsToQueue(List<Itinary> itinaries)
        {
            IConnectionFactory factory = new NMSConnectionFactory("activemq:tcp://localhost:61616");
            using (IConnection connection = factory.CreateConnection())
            using (Apache.NMS.ISession session = connection.CreateSession())
            {
                IDestination destination = session.GetQueue("ItineraryQueue");
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
                                    string messageText = JsonConvert.SerializeObject(step.Instruction);
                                    ITextMessage message = session.CreateTextMessage(messageText);
                                    producer.Send(message);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
