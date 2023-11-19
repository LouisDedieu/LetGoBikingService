using LetGoBikingService.Interfaces;
using LetGoBikingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetGoBikingService.Services
{
    public class RouteService : IRouteService
    {
        public Itinerary GetItinerary(string origin, string destination)
        {
            // Implémentez la logique pour créer et retourner un itinéraire.
            return new Itinerary
            {
                StartLocation = origin,
                EndLocation = destination
                // Initialisez les autres propriétés...
            };
        }
    }
}
