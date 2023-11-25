using LetGoBikingService.Interfaces;
using LetGoBikingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetGoBikingService.Services
{
    public class RouteService : IRouteService
    {
        private Contract contractNearestOrigin;
        private Contract contractNearestDestination;

        private List<Station> stationsNearestOrigin;
        private List<Station> stationsNearestDestination;
        private List<Station> stationsSelected = new List<Station>();

        private CoordinateNominatim coordinateOrigin;
        private CoordinateNominatim coordinateDestination;

        private ContractService contractService = new ContractService();

        public async Task<string> GetItinerary(string origin, string destination)
        {
            await contractService.InitializeAsync();
            contractNearestOrigin = await contractService.FindNearestContract(origin);
            contractNearestDestination = await contractService.FindNearestContract(destination);

            stationsNearestOrigin = await JCDecauxAPI.GetStationsAsync(contractNearestOrigin.name);
            stationsNearestDestination = contractNearestOrigin.Equals(contractNearestDestination)
                                         ? stationsNearestOrigin
                                         : await JCDecauxAPI.GetStationsAsync(contractNearestDestination.name);


            coordinateOrigin = await OpenStreetMapAPI.GetCoordinates(origin);

            coordinateDestination = await OpenStreetMapAPI.GetCoordinates(destination);

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

            return distanceWithStations < coordinateOrigin.GetDistanceTo(coordinateDestination);
        }
    }

}
