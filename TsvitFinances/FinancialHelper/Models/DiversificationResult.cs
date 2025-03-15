namespace TsvitFinances.FinancialHelper.Models;

public class DiversificationResult
{
    public decimal TotalNicheSum { get; set; }

    public decimal RecommendedNichePercentage { get; set; }

    public required string Sector { get; set; }

    public required decimal TotalPercentage { get; set; }

    public bool CurrentSector { get; internal set; }
}
