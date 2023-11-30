using LetGoBikingService.Models;
using System;
using System.Collections.Generic;

public class StationsService
{
    public static Station FindNearestStation(List<Station> stations, CoordinateNominatim coordinateNominatim)
    {
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
