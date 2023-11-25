using LetGoBikingService.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LetGoBikingService.Services
{
    public class OpenRouteAPI
    {
        public static async Task<string> GetDirections(CoordinateNominatim start, CoordinateNominatim end, string profile)
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

                Console.WriteLine(url);

                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici (log, throw, etc.)
                Console.WriteLine($"Erreur lors de la récupération des itineraires : {ex.Message}");
                return null;
            }
        }

        public static async Task<string> GetDirections(Position start, CoordinateNominatim end, string profile)
        {
            string startLng = Convert.ToString(start.longitude).Replace(',', '.');
            string startLat = Convert.ToString(start.latitude).Replace(',', '.');

            CoordinateNominatim coord = new CoordinateNominatim(startLng, startLat);

            return await GetDirections(coord, end, profile);
        }

        internal static async Task<string> GetDirections(CoordinateNominatim start, Position end, string profile)
        {
            string endLng = Convert.ToString(end.longitude).Replace(',', '.');
            string endLat = Convert.ToString(end.latitude).Replace(',', '.');

            CoordinateNominatim coord = new CoordinateNominatim(endLng, endLat);

            return await GetDirections(start, coord, profile);
        }

        internal static async Task<string> GetDirections(Position start, Position end, string profile)
        {
            string startLng = Convert.ToString(start.longitude).Replace(',', '.');
            string startLat = Convert.ToString(start.latitude).Replace(',', '.');
            CoordinateNominatim startCoord = new CoordinateNominatim(startLng, startLat);

            string endLng = Convert.ToString(end.longitude).Replace(',', '.');
            string endLat = Convert.ToString(end.latitude).Replace(',', '.');
            CoordinateNominatim endCoord = new CoordinateNominatim(endLng, endLat);

            return await GetDirections(startCoord, endCoord, profile);
        }
    }
}
