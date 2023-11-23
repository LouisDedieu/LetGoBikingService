using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LetGoBikingService.Interfaces
{
    [ServiceContract]
    public interface IStationsService
    {
        [OperationContract]
        Task<List<Station>> GetStationsAsync(string contractName);

        [OperationContract]
        Task<Station> FindNearestStation(string address);
    }
}

[DataContract]
public class Station
{
    [DataMember]
    public int number { get; set; }
    [DataMember]
    public string name { get; set; }
    [DataMember]
    public Position position { get; set; }

}

[DataContract]
public class Position
{
    public Position(double lat, double lng)
    {
        this.lat = lat;
        this.lng = lng;
    }

    [DataMember]
    public double lat { get; set; }
    [DataMember]
    public double lng { get; set; }

    internal double GetDistanceTo(Position addressCoordinates)
    {
/*        const double EarthRadius = 6371.0; // Rayon de la Terre en kilomètres

        double latDelta = DegreesToRadians(lat2 - lat1);
        double lonDelta = DegreesToRadians(lon2 - lon1);

        double a = Math.Sin(latDelta / 2) * Math.Sin(latDelta / 2) +
                   Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                   Math.Sin(lonDelta / 2) * Math.Sin(lonDelta / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));*/
        return 5;
    }
}