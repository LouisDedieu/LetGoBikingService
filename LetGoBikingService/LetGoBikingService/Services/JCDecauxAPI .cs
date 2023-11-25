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

namespace LetGoBikingService
{
    public class JCDecauxAPI
    {
        static string apiKey = "07a62d74ecdd34da4bfa4e0cc2d7968398aa3110";

        public static async Task<List<Station>> GetStationsAsync(string contractName)
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                string url = $"https://api.jcdecaux.com/vls/v1/stations?contract={contractName}&apiKey={apiKey}";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();


                //var response = await httpClient.GetStringAsync(url);
                List<Station> ls = JsonSerializer.Deserialize<List<Station>>(responseBody);
                return ls;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici (log, throw, etc.)
                Console.WriteLine($"Erreur lors de la récupération des stations : {ex.Message}");
                return null;
            }
        }

        public static async Task<Contract> GetContractUsingCityNameAsync(string cityName)
        {
            try
            {
                List<Contract> contracts = await GetContractsAsync();

                foreach (Contract contract in contracts)
                {
                    if(contract.cities != null && contract.cities.Contains(cityName))
                    {
                        return contract;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici (log, throw, etc.)
                Console.WriteLine($"Erreur lors de la récupération du contract : {ex.Message}");
                return null;
            }
        }

        public static async Task<List<Contract>> GetContractsAsync()
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                string url = $"https://api.jcdecaux.com/vls/v3/contracts?apiKey={apiKey}";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                List<Contract> contracts = JsonSerializer.Deserialize<List<Contract>>(responseBody);
                return contracts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des contracts : {ex.Message}");
                return null;
            }
        }


    }
}
