namespace Data.Models;

public class Asset
{
    public int Id { get; set; }

    public required Guid PublicId { get; set; }

    public required string Name { get; set; }

    public required decimal CurrentPrice { get; set; }

    public required decimal BoughtFor { get; set; }

    public required DateTime AddedAt { get; set; }

    public required bool IsActive { get; set; }

    public DateTime? ClosedAt { get; set; }

    public decimal? SoldFor { get; set; }

    public int? SeasonalityId { get; set; }
    public Seasonality? Seasonalities { get; set; }

    public virtual IEnumerable<Chart> Charts { get; set; } = [];
}
