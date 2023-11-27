using LetGoBikingService.Interfaces;
using LetGoBikingService.Models;
using LetGoBikingService.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace LetGoBikingService.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]

    public class RouteService : IRouteService
    {
        private Contract contractNearestOrigin;
        private Contract contractNearestDestination;

        private Station[] stationsNearestOrigin;
        private Station[] stationsNearestDestination;
        private List<Station> stationsSelected = new List<Station>();

        private CoordinateNominatim coordinateOrigin;
        private CoordinateNominatim coordinateDestination;

        private LocationService locationService = new LocationService();
        private ActiveMQService activeMQService = new ActiveMQService();

        public async Task<Itinary> GetItinerary(string origin, string destination)
        {
            ProxyServiceClient proxyServiceClient = new ProxyServiceClient();

            Contract[] contracts = proxyServiceClient.GetListContract();
            List<CoordinateNominatim> contractsCoordinates = new List<CoordinateNominatim>();
            foreach (Contract contract in contracts)
            {
                if (contract.cities == null) continue;
                CoordinateNominatim coordNom = await OpenStreetMapAPI.GetCoordinates(contract.name);
                contractsCoordinates.Add(coordNom);
            }

            //proxyServiceClient.setContractService(contracts, contractsCoordinates);

            coordinateOrigin = await OpenStreetMapAPI.GetCoordinates(origin);
            coordinateDestination = await OpenStreetMapAPI.GetCoordinates(destination);

            contractNearestOrigin = locationService.FindNearestContract(coordinateOrigin, contracts, contractsCoordinates);
            contractNearestDestination = locationService.FindNearestContract(coordinateDestination, contracts, contractsCoordinates);

            stationsNearestOrigin = proxyServiceClient.GetListStations(contractNearestOrigin.name);
            stationsNearestDestination = contractNearestOrigin.Equals(contractNearestDestination)
                                         ? stationsNearestOrigin
                                         : proxyServiceClient.GetListStations(contractNearestDestination.name);

            if (stationsNearestOrigin.Count() != 0)
            {
                Station stationNearestOrigin = locationService.FindNearestStation(coordinateOrigin, stationsNearestOrigin);
                stationsSelected.Add(stationNearestOrigin);
                Console.WriteLine($"Station la plus proche de l'addresse d'origine : {stationNearestOrigin.name}");
            }
            if (stationsNearestDestination.Count() != 0)
            {
                Station stationNearestDestination = locationService.FindNearestStation(coordinateDestination, stationsNearestDestination);
                stationsSelected.Add(stationNearestDestination);
                Console.WriteLine($"Station la plus proche de l'addresse de destination : {stationNearestDestination.name}");
            }

            bool WithOrWithoutBike = isItWorthToUseStations(stationsSelected, coordinateOrigin, coordinateDestination);
            Console.WriteLine($"Is it worth to use them ? {WithOrWithoutBike}");

            Itinary itineraire = await ComputeItinary(WithOrWithoutBike);
            return itineraire;
        }

        private async Task<Itinary> ComputeItinary(bool withOrWithoutBike)
        {
            if (withOrWithoutBike && stationsSelected.Count >= 2)
            {
                List<Itinary> itinaries = new List<Itinary>();
                // Calculer l'itinéraire à pied jusqu'à la première station
                Itinary walkToFirstStation = await OpenRouteAPI.GetDirections(coordinateOrigin, stationsSelected.First().position, "foot-walking");
                itinaries.Add(walkToFirstStation);

                // Calculer l'itinéraire à vélo entre les deux stations
                Itinary bikeRoute = await OpenRouteAPI.GetDirections(stationsSelected.First().position, stationsSelected.Last().position, "cycling-regular");
                itinaries.Add(bikeRoute);

                // Calculer l'itinéraire à pied de la dernière station à la destination
                Itinary walkToDestination = await OpenRouteAPI.GetDirections(stationsSelected.Last().position, coordinateDestination, "foot-walking");
                itinaries.Add(walkToDestination);

                // Combinez ces segments pour former un itinéraire complet
                Itinary combinedItinary = CombineItinaries(itinaries);

                await activeMQService.SendItineraryStepsToQueue(combinedItinary);

                return combinedItinary;
            }
            else
            {
                // Itinéraire à pied complet si le vélo n'est pas une option valable
                return await OpenRouteAPI.GetDirections(coordinateOrigin, coordinateDestination, "foot-walking");
            }
        }

        public static Itinary CombineItinaries(List<Itinary> itinaries)
        {
            Itinary combinedItinary = new Itinary { Features = new List<Feature>() };

            foreach (Itinary itinary in itinaries)
            {
                foreach (Feature feature in itinary.Features)
                {
                    // Ajouter les caractéristiques de chaque itinéraire dans l'itinéraire combiné
                    combinedItinary.Features.Add(feature);
                }
            }

            return combinedItinary;
        }


        private bool isItWorthToUseStations(List<Station> stations, CoordinateNominatim coordinateOrigin, CoordinateNominatim coordinateDestination)
        {
            if (stations.Count < 2) return false;

            double distanceWithStations = locationService.GetDistanceTo(stations.First().position, coordinateOrigin) +
                                          locationService.GetDistanceTo(stations.Last().position, coordinateDestination);

            return distanceWithStations < locationService.GetDistanceTo(coordinateOrigin, coordinateDestination);
        }
    }
}