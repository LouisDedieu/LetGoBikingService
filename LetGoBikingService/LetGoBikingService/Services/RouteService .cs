using LetGoBikingService.Interfaces;
using LetGoBikingService.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public async Task<Contract> FindNearestContract(string origin)
        {
            Contract nearestContract = null;
            double minDistance = double.MaxValue;

            CoordinateNominatim originCoordinate = await OpenStreetMapAPI.GetCoordinates(origin);
            List<Contract> contracts = await JCDecauxAPI.GetContractsAsync();


            foreach (Contract contract in contracts)
            {
                //Contract named jcdecauxbike and besancon are null
                if (contract.name == "jcdecauxbike" || contract.name == "besancon") continue;

                CoordinateNominatim contractCoordinates = await OpenStreetMapAPI.GetCoordinates(contract.name);
                double distance = GetDistanceTo(originCoordinate, contractCoordinates);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestContract = contract;
                }
            }
            return nearestContract;
        }

        private double GetDistanceTo(CoordinateNominatim originCoordinate, CoordinateNominatim contractCoordinates)
        {
            double R = 6371e3; // Rayon de la Terre en mètres

            //Conversion de string en double
            if (!double.TryParse(originCoordinate.LatitudeNominatim, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat1) ||
                !double.TryParse(originCoordinate.LongitudeNominatim, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon1) ||
                !double.TryParse(contractCoordinates.LatitudeNominatim, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat2) ||
                !double.TryParse(contractCoordinates.LongitudeNominatim, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon2))
            {
                throw new ArgumentException("La conversion des coordonnées d'origine en double a échoué.");
            }

            lat2 = ToRadians(lat2);
            lon2 = ToRadians(lon2);

            lat1 = ToRadians(lat1);
            lon1 = ToRadians(lon1);

            double deltaLat = lat2 - lat1;
            double deltaLon = lon2 - lon1;

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Distance en mètres
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

    }


}
