using Data.Models.Enums;

namespace TsvitFinances.Dto.Asset;

public class AddAssetDto
{
    public required string UserPublicId { get; set; }

    public required string Name { get; set; }

    public required string Ticker { get; set; }

    public decimal CurrentPrice { get; set; }

    public decimal BoughtFor { get; set; }

    public decimal Quantity { get; set; }

    public required int Sector { get; set; }

    public required int InvestmentTerm { get; set; }

    public required int Market { get; set; }

    public required List<IFormFile> Charts { get; set; }
}
