using LetGoBikingService;
using LetGoBikingService.Interfaces;
using LetGoBikingService.Models;
using LetGoBikingService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
public class StationsService : IStationsService
{

    public async Task<List<Station>> GetStationsAsync(string contractName)
    {
        return await JCDecauxAPI.GetStationsAsync(contractName);
    }

    public static Station FindNearestStation(List<Station> stations, CoordinateNominatim coordinateNominatim)
    {
        HttpClient httpClient = new HttpClient();
        try
        {
            Station nearestStation = null;
            double minDistance = double.MaxValue;

            foreach (Station station in stations)
            {
                Position stationCoordinates = station.position;

                double distance = stationCoordinates.GetDistanceTo(coordinateNominatim);
                if (distance < minDistance && station.hasAvailableBike())
                {
                    minDistance = distance;
                    nearestStation = station;
                }
            }
            return nearestStation;
        }

        catch (Exception ex)
        {
            // Gérer l'exception ici (log, throw, etc.)
            Console.WriteLine($"Erreur lors de la récupération de la station la plus proche : {ex.Message}");
            return null;
        }
    }


}
