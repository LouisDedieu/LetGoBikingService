using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using LetGoBikingService.Interfaces;
using LetGoBikingService.Models;

namespace LetGoBikingService
{
    public class JCDecauxAPI : IStationsService
    {
        private readonly string apiKey;
        private readonly HttpClient httpClient;

        public JCDecauxAPI(string apiKey)
        {
            this.apiKey = apiKey;
            httpClient = new HttpClient();
        }

        public async Task<IEnumerable<Station>> GetStationsAsync(string contractName)
        {
            try
            {
                string url = $"https://api.jcdecaux.com/vls/v1/stations?contract={contractName}&apiKey={apiKey}";
                var response = await httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<IEnumerable<Station>>(response);
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici (log, throw, etc.)
                Console.WriteLine($"Erreur lors de la récupération des stations : {ex.Message}");
                return null;
            }
        }
    }
}
