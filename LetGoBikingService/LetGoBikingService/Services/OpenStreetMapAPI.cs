using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using LetGoBikingService.Interfaces;
using LetGoBikingService.Models;
using System.Net;
using System.Globalization;

namespace LetGoBikingService
{
    public class OpenStreetMapAPI
    {
        public static async Task<CoordinateNominatim> GetCoordinates(string address)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "LetsGoBkingLouis");

            try
            {
                string baseUrl = "https://nominatim.openstreetmap.org/search?format=json&q=";
                string url = baseUrl + Uri.EscapeDataString(address);
                HttpResponseMessage response = await httpClient.GetAsync(url); 
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                CoordinateNominatim[] coordinates = JsonSerializer.Deserialize<CoordinateNominatim[]>(responseBody);
                return coordinates[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des coordonneess : {ex.Message}");
                return null;

            }
        }
    }
}
