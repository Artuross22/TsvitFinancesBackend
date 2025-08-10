using Data.Models;
using Data.Models.Enums;
using Microsoft.OpenApi.Extensions;
using TsvitFinances.FinancialHelper.Models;

namespace TsvitFinances.FinancialHelper
{
    public static class CalculateDiversification
    {
        public static List<DiversificationResult> Result(List<Diversification> diversifications, Sector currentSector, params Asset[] assets)
        {
            var result = new List<DiversificationResult>();

            var total = assets.Sum(a => a.CurrentPrice * a.CurrentQuantity);

            foreach (var diversification in diversifications)
            {
                var totalNicheSum = assets
                    .Where(a => a.Sector == diversification.Sector)
                    .Sum(a => a.CurrentPrice * a.CurrentQuantity);

                result.Add(new DiversificationResult
                {

                    TotalNicheSum = totalNicheSum,
                    TotalPercentage = totalNicheSum / total * 100,
                    RecommendedNichePercentage = diversification.NichePercentage,
                    Sector = diversification.Sector.GetDisplayName(),
                    CurrentSector = diversification.Sector == currentSector
                });
            }

            return result;
        }
    }
}