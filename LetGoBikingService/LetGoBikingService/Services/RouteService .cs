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
        private List<Contract> contracts = JCDecauxAPI.GetContractsAsync().GetAwaiter().GetResult();

        public async Task<Itinerary> GetItinerary(string origin, string destination)
        {
            /*            Contract contractNearestOrigin = await JCDecauxAPI.GetContractUsingCityNameAsync("Marseille");
                        Contract contractNearestDestination = await JCDecauxAPI.GetContractUsingCityNameAsync("Lyon");*/
            Contract contractNearestOrigin = await FindNearestContract(origin);
            Contract contractNearestDestination = await FindNearestContract(destination);

            List<Station> stationsNearestOrigin = await JCDecauxAPI.GetStationsAsync(contractNearestOrigin.name);
            List<Station> stationsNearestDestination = await JCDecauxAPI.GetStationsAsync(contractNearestDestination.name);

            CoordinateNominatim coordinateOrigin = await OpenStreetMapAPI.GetCoordinates(origin);
            CoordinateNominatim coordinateDestination = await OpenStreetMapAPI.GetCoordinates(destination);

            List<Station> stationsSelected = new List<Station>();

            if (stationsNearestOrigin.Count != 0)
            {
                Station stationNearestOrigin = StationsService.FindNearestStation(stationsNearestOrigin, coordinateOrigin);
                stationsSelected.Add(stationNearestOrigin);
                Console.WriteLine($"Station la plus proche de l'addresse d'origine : {stationNearestOrigin.name}");
            }
            if (stationsNearestDestination.Count != 0)
            {
                Station stationNearestDestination = StationsService.FindNearestStation(stationsNearestDestination, coordinateDestination);
                stationsSelected.Add(stationNearestDestination);
                Console.WriteLine($"Station la plus proche de l'addresse de destination : {stationNearestDestination.name}");
            }

             Console.WriteLine($"Is it worth to use them ? {isItWorthToUseStations(stationsSelected, coordinateOrigin, coordinateDestination)}");

            // Implémentez la logique pour créer et retourner un itinéraire.
            return new Itinerary
            {
                StartLocation = origin,
                EndLocation = destination
                // Initialisez les autres propriétés...
            };
        }

        private bool isItWorthToUseStations(List<Station> stations, CoordinateNominatim coordinateOrigin, CoordinateNominatim coordinateDestination)
        {
            if (stations.Count < 2) return false;

            double distanceWithStations = stations.First().position.GetDistanceTo(coordinateOrigin) +
                                          stations.Last().position.GetDistanceTo(coordinateDestination);

            return distanceWithStations < GetDistanceTo(coordinateOrigin, coordinateDestination);
        }


        public async Task<Contract> FindNearestContract(string address)
        {
            Contract nearestContract = null;
            double minDistance = double.MaxValue;

            CoordinateNominatim addressCoordinate = await OpenStreetMapAPI.GetCoordinates(address);

            foreach (Contract contract in this.contracts)
            {
                //Contract named jcdecauxbike and besancon are null
                if (contract.name == "jcdecauxbike" || contract.name == "besancon") continue;

                CoordinateNominatim contractCoordinates = await OpenStreetMapAPI.GetCoordinates(contract.name);
                double distance = GetDistanceTo(addressCoordinate, contractCoordinates);

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
