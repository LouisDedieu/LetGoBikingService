using LetGoBikingService.Models;
using LetGoBikingService.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetGoBikingService.Services
{
    public class LocationService
    {
        private double R = 6371e3;

        public Contract FindNearestContract(CoordinateNominatim addressCoordinate, Contract[] contracts, CoordinateNominatim[] contractsCoordinates)
        {
            Contract nearestContract = null;
            double minDistance = double.MaxValue;

            int i = 0;

            foreach (Contract contract in contracts)
            {
                if (contract.cities == null) continue;

                double distance = GetDistanceTo(addressCoordinate, contractsCoordinates[i++]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestContract = contract;
                }
            }
            return nearestContract;
        }

        public Station FindNearestStation(CoordinateNominatim addressCoordinate, Station[] stationsNearestaddressCoordinate)
        {
            Station nearestStation = null;
            double minDistance = double.MaxValue;

            foreach (Station station in stationsNearestaddressCoordinate)
            {
                Position stationCoordinates = station.position;

                double distance = GetDistanceTo(stationCoordinates, addressCoordinate);
                if (distance < minDistance && station.available_bikes > 0)
                {
                    minDistance = distance;
                    nearestStation = station;
                }
            }
            return nearestStation;
        }

        public double GetDistanceTo(Position positionStart, CoordinateNominatim coordinateDestination)
        {
            //Conversion de string en double
            if (!double.TryParse(coordinateDestination.LatitudeNominatim, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat2) ||
                !double.TryParse(coordinateDestination.LongitudeNominatim, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon2))
            {
                throw new ArgumentException("La conversion des coordonnées d'origine en double a échoué.");
            }


            var lat1 = positionStart.latitude * Math.PI / 180; // Convertit en radians
            lat2 = lat2 * Math.PI / 180;

            var deltaLat = (lat2 - lat1);
            var deltaLon = (lon2 * Math.PI / 180) - (positionStart.longitude * Math.PI / 180);

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return this.R * c; // Distance en mètres
        }

        public double GetDistanceTo(CoordinateNominatim coordinateStart, CoordinateNominatim coordinateDestination)
        {
            //Conversion de string en double
            if (!double.TryParse(coordinateStart.LatitudeNominatim, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat) ||
                !double.TryParse(coordinateStart.LongitudeNominatim, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon))
            {
                throw new ArgumentException("La conversion des coordonnées d'origine en double a échoué.");
            }

            Position positionStart = new Position();
            positionStart.latitude = lat;
            positionStart.longitude = lon;

            return GetDistanceTo(positionStart, coordinateDestination);
        }
    }
}
