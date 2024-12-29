using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class PurchaseLevel
{
    public int Id { get; set; }

    public required Guid PublicId { get; set; }

    public string? Description { get; set; }

    public required decimal Level { get; set; }

    public required decimal? AverageLevel { get; set; }

    public int? AssetId { get; set; }

    public required Asset Asset { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<PurchaseLevel>
    {
        public void Configure(EntityTypeBuilder<PurchaseLevel> builder)
        {
            builder.ToTable("PurchaseLevel");
        }
    }
}
