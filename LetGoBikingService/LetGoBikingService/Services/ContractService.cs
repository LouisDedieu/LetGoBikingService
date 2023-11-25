using LetGoBikingService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetGoBikingService.Services
{
    public class ContractService
    {
        private List<Contract> contracts;
        private List<CoordinateNominatim> contractsCoordinates;

        public ContractService()
        {
            contractsCoordinates = new List<CoordinateNominatim>();
        }

        public async Task InitializeAsync()
        {
            contracts = await JCDecauxAPI.GetContractsAsync();

            foreach (Contract contract in contracts)
            {
                if (contract.cities == null) continue;

                CoordinateNominatim contractCoordinates = await OpenStreetMapAPI.GetCoordinates(contract.name);
                contractsCoordinates.Add(contractCoordinates);
            }


        }

        public async Task<Contract> FindNearestContract(string address)
        {
            Contract nearestContract = null;
            double minDistance = double.MaxValue;

            CoordinateNominatim addressCoordinate = await OpenStreetMapAPI.GetCoordinates(address);
            CoordinateNominatim contractCoordinates;
            int i = 0;

            foreach (Contract contract in this.contracts)
            {
                if (contract.cities == null) continue;

                contractCoordinates = contractsCoordinates[i++];

                double distance = addressCoordinate.GetDistanceTo(contractCoordinates);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestContract = contract;
                }
            }
            return nearestContract;
        }
    }
}
