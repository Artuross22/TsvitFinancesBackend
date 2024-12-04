using Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class PositionManagement
{
    public int Id { get; init; }

    public required Guid PublicId {  get; init; }

    public required decimal? ScalingOut { get; set; }

    public required decimal? ScalingIn { get; set; }

    public required double AverageLevel { get; set; }

    public required IEnumerable<SalesLevels> SalesLevels { get; set; }

    public required IEnumerable<PurchaseLevel> PurchaseLevels { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<PositionManagement>
    {
        public void Configure(EntityTypeBuilder<PositionManagement> builder)
        {
            builder.ToTable("PositionEntries");

            builder.HasMany(s => s.SalesLevels)
                .WithOne(a => a.PositionManagement)
                .HasForeignKey(a => a.PositionManagementId);

            builder.HasMany(s => s.PurchaseLevels)
                .WithOne(a => a.PositionManagement)
                .HasForeignKey(a => a.PositionManagementId);
        }
    }
}
