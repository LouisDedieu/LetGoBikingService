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
        private Contract contractNearestOrigin;
        private Contract contractNearestDestination;

        private List<Station> stationsNearestOrigin;
        private List<Station> stationsNearestDestination;
        private List<Station> stationsSelected = new List<Station>();

        private CoordinateNominatim coordinateOrigin;
        private CoordinateNominatim coordinateDestination;

        public async Task<string> GetItinerary(string origin, string destination)
        {
            /*            Contract contractNearestOrigin = await JCDecauxAPI.GetContractUsingCityNameAsync("Marseille");
                        Contract contractNearestDestination = await JCDecauxAPI.GetContractUsingCityNameAsync("Lyon");*/
            contractNearestOrigin = await FindNearestContract(origin);
            contractNearestDestination = await FindNearestContract(destination);

            stationsNearestOrigin = await JCDecauxAPI.GetStationsAsync(contractNearestOrigin.name);
            stationsNearestDestination = await JCDecauxAPI.GetStationsAsync(contractNearestDestination.name);

            coordinateOrigin = await OpenStreetMapAPI.GetCoordinates(origin);
            Console.WriteLine($"{coordinateOrigin.LongitudeNominatim},{coordinateOrigin.LatitudeNominatim}");

            coordinateDestination = await OpenStreetMapAPI.GetCoordinates(destination);
            Console.WriteLine($"{coordinateDestination.LongitudeNominatim},{coordinateOrigin.LatitudeNominatim}");

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

            bool WithOrWithoutBike = isItWorthToUseStations(stationsSelected, coordinateOrigin, coordinateDestination);
            Console.WriteLine($"Is it worth to use them ? {WithOrWithoutBike}");

            string itineraire = await ComputeItinary(WithOrWithoutBike);
            return itineraire;
        }

        private async Task<string> ComputeItinary(bool withOrWithoutBike)
        {
            if (withOrWithoutBike && stationsSelected.Count >= 2)
            {
                // Calculer l'itinéraire à pied jusqu'à la première station
                var walkToFirstStation = await OpenRouteAPI.GetDirections(coordinateOrigin, stationsSelected.First().position, "foot-walking");

                // Calculer l'itinéraire à vélo entre les deux stations
                var bikeRoute = await OpenRouteAPI.GetDirections(stationsSelected.First().position, stationsSelected.Last().position, "cycling-regular");

                // Calculer l'itinéraire à pied de la dernière station à la destination
                var walkToDestination = await OpenRouteAPI.GetDirections(stationsSelected.Last().position, coordinateDestination, "foot-walking");

                // Combinez ces segments pour former un itinéraire complet
                return $"{walkToFirstStation}\n{bikeRoute}\n{walkToDestination}";
            }
            else
            {
                // Itinéraire à pied complet si le vélo n'est pas une option valable
                return await OpenRouteAPI.GetDirections(coordinateOrigin, coordinateDestination, "foot-walking");
            }
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
