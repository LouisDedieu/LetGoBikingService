using LetGoBikingService.Models;
using LetGoBikingService.ServiceReference1;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LetGoBikingService.Services
{
    public class OpenRouteAPI
    {
        public static async Task<Itinary> GetDirections(CoordinateNominatim start, CoordinateNominatim end, string profile)
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                string startLng = start.LongitudeNominatim; 
                string startLat = start.LatitudeNominatim;
                string endLng = end.LongitudeNominatim;
                string endLat = end.LatitudeNominatim;
                string apiKey = "5b3ce3597851110001cf6248bbff9cbbd09140819224dfa03a06a2c4";
                string url = $"https://api.openrouteservice.org/v2/directions/{profile}?api_key={apiKey}&start={startLng},{startLat}&end={endLng},{endLat}";

                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Itinary it = JsonConvert.DeserializeObject<Itinary>(responseBody);
                Utils.Utils.PopulateStepCoordinates(it);
                return it;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici (log, throw, etc.)
                Console.WriteLine($"Erreur lors de la récupération des itineraires : {ex.Message}");
                return null;
            }
        }

        public static async Task<Itinary> GetDirections(Position start, CoordinateNominatim end, string profile)
        {

            CoordinateNominatim coordStart = Utils.Utils.ConvertPositionToCoordinateNominatim(start);
            return await GetDirections(coordStart, end, profile);
        }

        internal static async Task<Itinary> GetDirections(CoordinateNominatim start, Position end, string profile)
        {
            CoordinateNominatim coordEnd = Utils.Utils.ConvertPositionToCoordinateNominatim(end);

            return await GetDirections(start, coordEnd, profile);
        }

        internal static async Task<Itinary> GetDirections(Position start, Position end, string profile)
        {
            CoordinateNominatim startCoord = Utils.Utils.ConvertPositionToCoordinateNominatim(start);
            CoordinateNominatim endCoord = Utils.Utils.ConvertPositionToCoordinateNominatim(end);

            return await GetDirections(startCoord, endCoord, profile);
        }

        public static async Task<double> GetDurationOnly(CoordinateNominatim start, CoordinateNominatim end, string profile)
        {
            Itinary it = await GetDirections(start, end, profile);
            return it.Features.First().Properties.Segments.First().Duration;
        }
    }
}
