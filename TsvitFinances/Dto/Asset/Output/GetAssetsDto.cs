using Data.Models.Enums;

namespace TsvitFinances.Dto.Asset.Output;

public class GetAssetsDto
{
    public required Guid PublicId { get; set; }

    public required string Sector { get; set; }

    public required string Term { get; set; }

    public required string Market { get; set; }

    public required string Name { get; set; }

    public required string Ticker { get; set; }

    public required decimal CurrentPrice { get; set; }

    public required decimal Quantity { get; set; }

    public required decimal BoughtFor { get; set; }

    public required DateTime AddedAt { get; set; }

    public required decimal InterestOnCurrentDeposit { get; set; }

   public IReadOnlyList<string> ChartsPath { get; set; }
}
